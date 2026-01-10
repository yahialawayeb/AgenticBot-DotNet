using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGroqModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModels",
                columns: new[] { "Id", "Company", "Description", "LogoUrl", "ModelName", "Name", "ReferenceLink", "ReferralSource" },
                values: new object[] { 10, "Meta", "Llama 3.1 8B is a high-performance, lightweight model optimized for low latency and high throughput, hosted on Groq.", "assets/images/model-icons/llama.png", "llama-3.1-8b-instant", "LLaMA 3.1 8B (Groq)", "https://console.groq.com/docs/models", "Groq" });

            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 10, 1 },
                    { 10, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
