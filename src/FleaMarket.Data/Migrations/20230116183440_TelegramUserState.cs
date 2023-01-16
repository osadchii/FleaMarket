using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUserState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telegram_user_state",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramBotId = table.Column<Guid>(type: "uuid", nullable: true),
                    TelegramUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_user_state", x => x.Id);
                    table.ForeignKey(
                        name: "FK_telegram_user_state_telegram_bot_TelegramBotId",
                        column: x => x.TelegramBotId,
                        principalSchema: "flea_market",
                        principalTable: "telegram_bot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_telegram_user_state_telegram_user_TelegramUserId",
                        column: x => x.TelegramUserId,
                        principalSchema: "flea_market",
                        principalTable: "telegram_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_telegram_user_state_TelegramBotId",
                schema: "flea_market",
                table: "telegram_user_state",
                column: "TelegramBotId");

            migrationBuilder.CreateIndex(
                name: "IX_telegram_user_state_TelegramUserId_TelegramBotId",
                schema: "flea_market",
                table: "telegram_user_state",
                columns: new[] { "TelegramUserId", "TelegramBotId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_user_state",
                schema: "flea_market");
        }
    }
}
