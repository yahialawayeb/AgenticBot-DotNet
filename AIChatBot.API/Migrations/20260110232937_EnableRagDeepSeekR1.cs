using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class EnableRagDeepSeekR1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[] { 12, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 12, 5 });
        }
    }
}
