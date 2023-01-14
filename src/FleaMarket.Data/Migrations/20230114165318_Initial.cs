using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "flea_market");

            migrationBuilder.CreateTable(
                name: "localized_text",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    LocalizedTextId = table.Column<string>(type: "text", nullable: false),
                    LocalizedText = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localized_text", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "telegram_user",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "telegram_bot",
                schema: "flea_market",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_bot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_telegram_bot_telegram_user_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "flea_market",
                        principalTable: "telegram_user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_localized_text_Language_LocalizedTextId",
                schema: "flea_market",
                table: "localized_text",
                columns: new[] { "Language", "LocalizedTextId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telegram_bot_OwnerId",
                schema: "flea_market",
                table: "telegram_bot",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_telegram_bot_Token",
                schema: "flea_market",
                table: "telegram_bot",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telegram_user_ChatId",
                schema: "flea_market",
                table: "telegram_user",
                column: "ChatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "localized_text",
                schema: "flea_market");

            migrationBuilder.DropTable(
                name: "telegram_bot",
                schema: "flea_market");

            migrationBuilder.DropTable(
                name: "telegram_user",
                schema: "flea_market");
        }
    }
}
