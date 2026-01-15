using AIChatBot.API.Factory;
using AIChatBot.API.Hubs;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models;
using AIChatBot.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIChatBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatHistoryService _chatHistoryService;
        private readonly ChatModelServiceFactory _factory;
        private readonly AgentService _agentService;
        private readonly IChatSessionServices _chatSessionServices;
        private readonly IModelService _modelService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly RagChatService _ragChatService;

        public ChatService(
            IChatHistoryService chatHistoryService,
            ChatModelServiceFactory factory,
            AgentService agentService,
            IChatSessionServices chatSessionServices,
            IModelService modelService,
            IHubContext<ChatHub> hubContext,
            RagChatService ragChatService)
        {
            _chatHistoryService = chatHistoryService;
            _factory = factory;
            _agentService = agentService;
            _chatSessionServices = chatSessionServices;
            _modelService = modelService;
            _hubContext = hubContext;
            _ragChatService = ragChatService;
        }

        public IActionResult GetHistory(Guid userId, Guid chatSessionIdentity)
        {
            var history = _chatHistoryService.GetHistory(userId, chatSessionIdentity);
            return new OkObjectResult(history);
        }

        public async Task<IActionResult> PostChat(ChatRequest request)
        {
            var sessionWithoutMessages = await _chatSessionServices.GetSessionWithoutMessages(request.UserId, request.ChatSessionIdentity);
            var selectedModel = _modelService.GetModelById(request.ModelId);
            if (selectedModel == null)
            {
                return new BadRequestObjectResult(new { error = "Target AI model activation failed. The requested model core is not registered in the system database." });
            }

            // Validate request
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "user",
                    ChatSessionId = sessionWithoutMessages.Id,
                    Content = request.Message,
                    TimeStamp = DateTime.UtcNow
                }
            };

            _chatHistoryService.SaveHistory(request.UserId, messages);

            string responseText = string.Empty;
            bool saveHistory = true;

            try
            {
                if (request.AIMode == "tools")
                {
                    var prompt = preparePrompt(request.Message);
                    var service = _factory.GetService(selectedModel.ModelName);
                    var aiResponse = await service.SendMessageAsync(selectedModel.ModelName, prompt, request.ConnectionId);
                    responseText = await _agentService.RunToolAsync(aiResponse, request.UserId, sessionWithoutMessages.Id, request.ConnectionId);
                }
                else if (request.AIMode == "agent")
                {
                    var service = _factory.GetService(selectedModel.ModelName);

                    var msgObject = new List<Dictionary<string, string>>
                    {
                        new() { ["role"] = "user", ["content"] = request.Message }
                    };
                    var response = await service.ChatWithFunctionSupportAsync(selectedModel.ModelName, msgObject, request.ConnectionId);
                    responseText = await _agentService.RunAgentAsync(response, request.UserId, sessionWithoutMessages.Id, request.ConnectionId);
                }
                else if (request.AIMode == "planner")
                {
                    var service = _factory.GetService(selectedModel.ModelName);
                    var funcExecLog = new List<FunctionCallResult>();
                    var isDone = false;
                    var step = 0;
                    const int MaxIterations = 10; // Maximum allowed iterations to prevent infinite loops
                    do
                    {
                        if (step >= MaxIterations)
                        {
                            Console.WriteLine("⚠️ Maximum iteration limit reached in planner mode.");
                            break;
                        }
                        var msgObject = preparePlannerPrompt(request, step, funcExecLog);

                        //Serialize the message object for logging
                        var msgJson = JsonSerializer.Serialize(msgObject, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        var response = await service.ChatWithFunctionSupportAsync(selectedModel.ModelName, msgObject, request.ConnectionId);

                        var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });
                        Console.WriteLine(responseJson);
                        if (!response.Any(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                        {
                            isDone = true;
                            break;
                        }
                        var iterationResponse = await _agentService.RunAgentAsync(response, request.UserId, sessionWithoutMessages.Id, request.ConnectionId);
                        foreach (var funcCall in response.Where(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                        {
                            funcExecLog.Add(new FunctionCallResult()
                            {
                                FunctionName = funcCall.FunctionName,
                                ArgumentsJson = funcCall.ArgumentsJson,
                                TextResponse = iterationResponse
                            });
                        }
                        messages = new List<ChatMessage>
                        {
                            new ChatMessage
                            {
                                Role = "assistant",
                                ChatSessionId = sessionWithoutMessages.Id,
                                Content = iterationResponse,
                                TimeStamp = DateTime.UtcNow
                            }
                        };

                        foreach (var msg in messages.Where(m => m.Role == "assistant"))
                        {
                            // Send the message to the client via SignalR
                            if (!string.IsNullOrEmpty(request.ConnectionId))
                            {
                                await _hubContext.Clients.Client(request.ConnectionId).SendAsync("ReceiveMessage", msg);
                            }
                        }

                        _chatHistoryService.SaveHistory(request.UserId, messages);

                        responseText += $"Recursion Step: {step} \n" + iterationResponse;
                        step++;
                    } while (!isDone);
                    saveHistory = false; // Don't save history for planner mode
                    if (string.IsNullOrWhiteSpace(responseText))
                    {
                        responseText = "⚠️ Agent loop ended without clear conclusion.";
                    }
                }
                else if (request.AIMode == "rag")
                {
                    responseText = await _ragChatService.GenerateRagResponseAsync(
                        request.UserId.ToString(),
                        request.Message,
                        selectedModel.ModelName,
                        request.ChatSessionIdentity,
                        request.ConnectionId);
                }
                else
                {
                    var service = _factory.GetService(selectedModel.ModelName);
                    
                    // Inject Global Context
                    var globalContext = await GetGlobalContextAsync();
                    var fullMessage = request.Message;
                    if (!string.IsNullOrWhiteSpace(globalContext))
                    {
                        fullMessage = $"Context:\n{globalContext}\n\nUser Message:\n{request.Message}\n\nIMPORTANT: Answer in the same language as the User Message above.";
                    }

                    responseText = await service.SendMessageAsync(selectedModel.ModelName, fullMessage, request.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                responseText = $"⚠️ System Fault: A critical error occurred during message processing. Mode: {request.AIMode}, Model: {selectedModel?.ModelName}. Error details: {ex.Message}";
                saveHistory = true; // Still save the error in history so the user knows why it failed
            }

            if (saveHistory)
            {
                messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Role = "assistant",
                        ChatSessionId = sessionWithoutMessages.Id,
                        Content = responseText,
                        TimeStamp = DateTime.UtcNow
                    }
                };

                _chatHistoryService.SaveHistory(request.UserId, messages);
                return new OkObjectResult(new ChatResponse { ShowInHistory = true, Response = responseText });
            }
            else
            {
                return new OkObjectResult(new ChatResponse { ShowInHistory = false, Response = responseText });
            }
        }

        private string preparePrompt(string userInput)
        {
            // Construct prompt with tools
            var toolList = "Tools: CreateFile, FetchWebData, SendEmail\n";
            var systemPrompt = $"{toolList}\nUnderstand user's intent and call one tool. Provide me the response in json string with properties 'tool' & 'parameters', and in 'parameters', respective properties will be available. Do not include any code expressions.";

            return $"{systemPrompt}\nUser: {userInput}";
        }

        private List<Dictionary<string, string>> preparePlannerPrompt(ChatRequest request, int step, List<FunctionCallResult> functionCalls)
        {
            const int MaxMessagesToInclude = 15; // Limit to prevent token overflow
            
            var chatSession = _chatHistoryService.GetHistory(request.UserId, request.ChatSessionIdentity);

            // Take only the most recent messages to stay within token limits
            var recentMessages = chatSession.Messages
                .OrderByDescending(m => m.TimeStamp)
                .Take(MaxMessagesToInclude)
                .OrderBy(m => m.TimeStamp)
                .ToList();

            var msgObject = recentMessages.Select(m => new Dictionary<string, string>
            {
                ["role"] = m.Role.ToLower(),  // "user" or "assistant"
                ["content"] = m.Content
            }).ToList();

            if (msgObject.Count == 0 || step == 0)
            {
                // If no history, start with the user message
                if (msgObject.Count == 0)
                {
                    msgObject = new List<Dictionary<string, string>>();
                }
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "system", 
                    ["content"] = "You are a tool-using agent. When your task is complete, end with a natural sentence and do not call a function."
                });
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "user", 
                    ["content"] = request.Message
                });
            }
            else
            {
                foreach (var item in functionCalls.Where(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                {
                    msgObject.Add(new Dictionary<string, string>
                    {
                        ["role"] = "function",
                        ["name"] = item.FunctionName,
                        ["content"] = item.TextResponse
                    });
                }
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "user",
                    ["content"] = "Continue"
                });
            }

            return msgObject;
        }

        private static string? _cachedGlobalContext;
        private static DateTime _lastCacheUpdate = DateTime.MinValue;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10); // Refresh every 10 mins if needed, or keep static.

        private async Task<string> GetGlobalContextAsync()
        {
            // Simple caching strategy
            if (_cachedGlobalContext != null && (DateTime.UtcNow - _lastCacheUpdate) < _cacheDuration)
            {
                return _cachedGlobalContext;
            }

            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "KnowledgeFiles", "extracted_text.txt");
                if (System.IO.File.Exists(filePath))
                {
                    _cachedGlobalContext = await System.IO.File.ReadAllTextAsync(filePath);
                    _lastCacheUpdate = DateTime.UtcNow;
                    return _cachedGlobalContext;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading global context file: {ex.Message}");
            }
            
            return string.Empty;
        }
    }
}
