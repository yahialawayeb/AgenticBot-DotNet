using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.Data
{
    public class ChatBotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<AIModel> AIModels { get; set; }
        public DbSet<ChatMode> ChatModes { get; set; }
        public DbSet<AIModelChatMode> AIModelChatModes { get; set; }
        public DbSet<AgentFile> AgentFiles { get; set; }

        public ChatBotDbContext(DbContextOptions<ChatBotDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatSession>()
                .HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ChatSessionId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatSessions)
                .WithOne()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<AgentFile>()
                .HasOne(af => af.User)
                .WithMany()
                .HasForeignKey(af => af.UserId);

            modelBuilder.Entity<AgentFile>()
                .HasOne(af => af.ChatSession)
                .WithMany()
                .HasForeignKey(af => af.ChatSessionId);

            modelBuilder.Entity<AIModelChatMode>()
                .HasKey(am => new { am.AIModelId, am.ChatModeId });

            modelBuilder.Entity<AIModelChatMode>()
                .HasOne(am => am.AIModel)
                .WithMany(m => m.ChatModes)
                .HasForeignKey(am => am.AIModelId);

            modelBuilder.Entity<AIModelChatMode>()
                .HasOne(am => am.ChatMode)
                .WithMany() // Remove navigation property to AIModels
                .HasForeignKey(am => am.ChatModeId);

            modelBuilder.Entity<ChatMode>().HasData(
                new ChatMode { Id = 1, Mode = "chat", Name = "Chat" },
                new ChatMode { Id = 2, Mode = "tools", Name = "Tools" },
                new ChatMode { Id = 3, Mode = "agent", Name = "Agent" },
                new ChatMode { Id = 4, Mode = "planner", Name = "Agent with Planning" },
                new ChatMode { Id = 5, Mode = "rag", Name = "Knowledge-Based (RAG)" }
            );

            // Seed AIModels
            modelBuilder.Entity<AIModel>().HasData(
                new AIModel { Id = 6, ModelName = "google/gemma-3-27b-it:free", Name = "Gemma 3 27B", Company = "Google", LogoUrl = "assets/images/model-icons/gemma.png", Description = "Gemma 3 introduces multimodality, supporting vision-language input and text outputs. It handles context windows up to 128k tokens, understands over 140 languages, and offers improved math, reasoning, and chat capabilities, including structured outputs and function calling. Gemma 3 27B is Google's latest open source model, successor to Gemma 2.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/google/gemma-3-27b-it:free" },
                new AIModel { Id = 7, ModelName = "google/gemini-2.0-flash-exp:free", Name = "Gemini Flash 2.0 - Limited", Company = "Google", LogoUrl = "assets/images/model-icons/gemini.png", Description = "Gemini Flash 2.0 offers a significantly faster time to first token (TTFT) compared to Gemini Flash 1.5, while maintaining quality on par with larger models like Gemini Pro 1.5. It introduces notable enhancements in multimodal understanding, coding capabilities, complex instruction following, and function calling. These advancements come together to deliver more seamless and robust agentic experiences.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/google/gemini-2.0-flash-exp:free" },
                new AIModel { Id = 10, ModelName = "llama-3.1-8b-instant", Name = "LLaMA 3.1 8B (Groq)", Company = "Meta", LogoUrl = "assets/images/model-icons/llama.png", Description = "Llama 3.1 8B is a high-performance, lightweight model optimized for low latency and high throughput, hosted on Groq.", ReferralSource = "Groq", ReferenceLink = "https://console.groq.com/docs/models" },
                new AIModel { Id = 11, ModelName = "mistralai/devstral-2512:free", Name = "Mistral Devstral", Company = "Mistral AI", LogoUrl = "assets/images/model-icons/mistralai.png", Description = "Devstral is a state-of-the-art model from Mistral AI, optimized for development tasks and now available for free via OpenRouter.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/mistralai/devstral-2512:free" },
                new AIModel { Id = 12, ModelName = "deepseek/deepseek-r1-0528:free", Name = "DeepSeek R1", Company = "DeepSeek", LogoUrl = "assets/images/model-icons/deepseek.png", Description = "DeepSeek R1 is a powerful and efficient model from DeepSeek, optimized for speed and high-quality responses, now available for free via OpenRouter.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/deepseek/deepseek-r1-0528:free" }
            );

            // Seed AIModelChatModes (model-mode relationships)
            modelBuilder.Entity<AIModelChatMode>().HasData(
                // Gemma 3 27B
                new AIModelChatMode { AIModelId = 6, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 6, ChatModeId = 2 },
                // Gemini Flash 2.0 - Limited
                new AIModelChatMode { AIModelId = 7, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 7, ChatModeId = 2 },
                // LLaMA 3.1 8B (Groq)
                new AIModelChatMode { AIModelId = 10, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 10, ChatModeId = 2 },
                // Mistral Devstral
                new AIModelChatMode { AIModelId = 11, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 11, ChatModeId = 2 },
                // DeepSeek R1
                new AIModelChatMode { AIModelId = 12, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 12, ChatModeId = 2 },
                new AIModelChatMode { AIModelId = 12, ChatModeId = 5 }
            );
        }
    }
}
