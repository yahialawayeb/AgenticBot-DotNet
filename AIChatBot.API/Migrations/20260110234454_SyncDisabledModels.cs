using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class SyncDisabledModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModels",
                columns: new[] { "Id", "Company", "Description", "LogoUrl", "ModelName", "Name", "ReferenceLink", "ReferralSource" },
                values: new object[,]
                {
                    { 1, "Microsoft", "Phi-3 is a family of lightweight 3B (Mini) and 14B (Medium) state-of-the-art open models by Microsoft.", "assets/images/model-icons/phi.png", "phi3", "Phi-3 Mini", "https://ollama.com/library/phi3:latest", "Ollama" },
                    { 2, "Meta", "Meta Llama 3: The most capable openly available LLM to date.", "assets/images/model-icons/llama.png", "llama3", "LLaMA 3", "https://ollama.com/library/llama3:latest", "Ollama" },
                    { 3, "Mistral AI", "The 7B model released by Mistral AI, updated to version 0.3.", "assets/images/model-icons/mistralai.png", "mistral", "Mistral 7B", "https://ollama.com/library/mistral:latest", "Ollama" },
                    { 4, "Google", "Gemma is a family of lightweight, state-of-the-art open models built by Google DeepMind. Updated to version 1.1.", "assets/images/model-icons/gemma.png", "gemma:2b", "Gemma", "https://ollama.com/library/gemma:2b", "Ollama" },
                    { 8, "OpenAI", "GPT-3.5 Turbo is OpenAI's fastest model. It can understand and generate natural language or code, and is optimized for chat and traditional completion tasks.", "assets/images/model-icons/chatgpt.png", "openai/gpt-3.5-turbo-0613", "GPT-3.5 Turbo", "https://openrouter.ai/openai/gpt-3.5-turbo-0613", "OpenRouter" },
                    { 9, "Google", "Gemini Flash 2.0 offers a significantly faster time to first token (TTFT) compared to Gemini Flash 1.5, while maintaining quality on par with larger models like Gemini Pro 1.5. It introduces notable enhancements in multimodal understanding, coding capabilities, complex instruction following, and function calling. These advancements come together to deliver more seamless and robust agentic experiences.", "assets/images/model-icons/gemini.png", "google/gemini-2.0-flash-001", "Gemini Flash 2.0 - Unlimited", "https://openrouter.ai/google/gemini-2.0-flash-001", "OpenRouter" }
                });

            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 2 },
                    { 4, 1 },
                    { 4, 2 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 3 },
                    { 8, 4 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 3 },
                    { 9, 4 }
                });
        }
    }
}
