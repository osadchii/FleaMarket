using FleaMarket.Data.Enums;
using FleaMarket.Data.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class MainMenuManagementTexts : Migration
    {
        private readonly (LocalizedTextId TextId, Language Language, string value)[] Values = new[]
        {
            (LocalizedTextId.MainMenu, Language.English, "Select a menu item"),
            (LocalizedTextId.MainMenu, Language.Russian, "Выберите пункт меню"),
            
            (LocalizedTextId.AddBotButton, Language.English, "Add new bot"),
            (LocalizedTextId.AddBotButton, Language.Russian, "Добавить нового бота"),
            
            (LocalizedTextId.MyBotsButton, Language.English, "My bots"),
            (LocalizedTextId.MyBotsButton, Language.Russian, "Мои боты"),
            
            (LocalizedTextId.ChangeLanguageButton, Language.English, "Change language \\ Изменить язык"),
            (LocalizedTextId.ChangeLanguageButton, Language.Russian, "Change language \\ Изменить язык")
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
