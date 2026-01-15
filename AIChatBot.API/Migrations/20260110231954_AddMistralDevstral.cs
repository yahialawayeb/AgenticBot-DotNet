using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMistralDevstral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModels",
                columns: new[] { "Id", "Company", "Description", "LogoUrl", "ModelName", "Name", "ReferenceLink", "ReferralSource" },
                values: new object[] { 11, "Mistral AI", "Devstral is a state-of-the-art model from Mistral AI, optimized for development tasks and now available for free via OpenRouter.", "assets/images/model-icons/mistralai.png", "mistralai/devstral-2512:free", "Mistral Devstral", "https://openrouter.ai/mistralai/devstral-2512:free", "OpenRouter" });

            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 11, 1 },
                    { 11, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
