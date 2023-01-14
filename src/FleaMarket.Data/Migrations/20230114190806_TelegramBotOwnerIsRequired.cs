﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramBotOwnerIsRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_telegram_bot_telegram_user_OwnerId",
                schema: "flea_market",
                table: "telegram_bot");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "flea_market",
                table: "telegram_bot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_telegram_bot_telegram_user_OwnerId",
                schema: "flea_market",
                table: "telegram_bot",
                column: "OwnerId",
                principalSchema: "flea_market",
                principalTable: "telegram_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_telegram_bot_telegram_user_OwnerId",
                schema: "flea_market",
                table: "telegram_bot");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "flea_market",
                table: "telegram_bot",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_telegram_bot_telegram_user_OwnerId",
                schema: "flea_market",
                table: "telegram_bot",
                column: "OwnerId",
                principalSchema: "flea_market",
                principalTable: "telegram_user",
                principalColumn: "Id");
        }
    }
}
