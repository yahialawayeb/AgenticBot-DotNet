using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeepSeekR1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModels",
                columns: new[] { "Id", "Company", "Description", "LogoUrl", "ModelName", "Name", "ReferenceLink", "ReferralSource" },
                values: new object[] { 12, "DeepSeek", "DeepSeek R1 is a powerful and efficient model from DeepSeek, optimized for speed and high-quality responses, now available for free via OpenRouter.", "assets/images/model-icons/deepseek.png", "deepseek/deepseek-r1-0528:free", "DeepSeek R1", "https://openrouter.ai/deepseek/deepseek-r1-0528:free", "OpenRouter" });

            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 12, 1 },
                    { 12, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
