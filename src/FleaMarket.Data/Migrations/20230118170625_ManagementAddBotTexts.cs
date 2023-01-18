using FleaMarket.Data.Enums;
using FleaMarket.Data.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class ManagementAddBotTexts : Migration
    {
        private readonly (LocalizedTextId TextId, Language Language, string value)[] Values = new[]
        {
            (LocalizedTextId.SendToken, Language.English, "Send your telegram bot token" +
                                                          $"{Environment.NewLine}You can create a new bot or view the token of an existing bot in the @BotFather official bot"),
            (LocalizedTextId.SendToken, Language.Russian, "Пришлите токен вашего телеграм бота." +
                                                          $"{Environment.NewLine}Вы можете создать нового бота или посмотреть токен существующего бота в официальном боте @BotFather"),
            
            (LocalizedTextId.Cancel, Language.English, "Cancel"),
            (LocalizedTextId.Cancel, Language.Russian, "Отмена"),
            
            (LocalizedTextId.Add, Language.English, "Add"),
            (LocalizedTextId.Add, Language.Russian, "Добавить"),
            
            (LocalizedTextId.AddBotTokenConfirmation, Language.English, "Add bot with this token?"),
            (LocalizedTextId.AddBotTokenConfirmation, Language.Russian, "Добавить бота с этим токеном?"),
            
            (LocalizedTextId.BotAlreadyExists, Language.English, "Bot with this token already exists"),
            (LocalizedTextId.BotAlreadyExists, Language.Russian, "Бот с этим токеном уже добавлен")
        };
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var value in Values)
            {
                migrationBuilder.Sql(LocalizedTextFunction.AddLocalizedText(value.TextId, value.Language, value.value));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var value in Values)
            {
                migrationBuilder.Sql(LocalizedTextFunction.DeleteLocalizedText(value.TextId, value.Language));
            }
        }
    }
}
