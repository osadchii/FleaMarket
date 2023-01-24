using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telegram_channel",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "telegram_bot_channel_mapping",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramBotId = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostAnnouncements = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_bot_channel_mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_telegram_bot_channel_mapping_telegram_bot_TelegramBotId",
                        column: x => x.TelegramBotId,
                        principalSchema: "flea_market",
                        principalTable: "telegram_bot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_telegram_bot_channel_mapping_telegram_channel_TelegramChann~",
                        column: x => x.TelegramChannelId,
                        principalSchema: "flea_market",
                        principalTable: "telegram_channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_telegram_bot_channel_mapping_TelegramBotId_TelegramChannelId",
                schema: "flea_market",
                table: "telegram_bot_channel_mapping",
                columns: new[] { "TelegramBotId", "TelegramChannelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telegram_bot_channel_mapping_TelegramChannelId",
                schema: "flea_market",
                table: "telegram_bot_channel_mapping",
                column: "TelegramChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_telegram_channel_ChatId",
                schema: "flea_market",
                table: "telegram_channel",
                column: "ChatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_bot_channel_mapping",
                schema: "flea_market");

            migrationBuilder.DropTable(
                name: "telegram_channel",
                schema: "flea_market");
        }
    }
}
