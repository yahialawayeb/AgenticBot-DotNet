using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDeepSeekV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModels",
                columns: new[] { "Id", "Company", "Description", "LogoUrl", "ModelName", "Name", "ReferenceLink", "ReferralSource" },
                values: new object[] { 5, "DeepSeek", "DeepSeek V3, a 685B-parameter, mixture-of-experts model, is the latest iteration of the flagship chat model family from the DeepSeek team.\nIt succeeds the DeepSeek V3 model and performs really well on a variety of tasks.", "assets/images/model-icons/deepseek.png", "deepseek/deepseek-chat-v3-0324:free", "DeepSeek v3", "https://openrouter.ai/deepseek/deepseek-chat-v3-0324:free", "OpenRouter" });

            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 5, 1 },
                    { 5, 2 }
                });
        }
    }
}
