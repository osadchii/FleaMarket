using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUserLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                schema: "flea_market",
                table: "telegram_user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                schema: "flea_market",
                table: "telegram_user");
        }
    }
}
