using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class RevertDeepSeekModelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ModelName", "ReferenceLink" },
                values: new object[] { "deepseek/deepseek-r1-0528:free", "https://openrouter.ai/deepseek/deepseek-r1-0528:free" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ModelName", "ReferenceLink" },
                values: new object[] { "deepseek/deepseek-r1:free", "https://openrouter.ai/deepseek/deepseek-r1:free" });
        }
    }
}
