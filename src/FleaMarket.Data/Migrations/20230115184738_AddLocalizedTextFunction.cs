using FleaMarket.Data.Enums;
using FleaMarket.Data.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizedTextFunction : Migration
    {
        private readonly (LocalizedTextId TextId, Language Language, string value)[] Values = new[]
        {
            (LocalizedTextId.SelectLanguage, Language.English, "Please select an interface language" +
                                                               $"{Environment.NewLine}Пожалуйста, выберите язык интерфейса"),
            (LocalizedTextId.SelectLanguage, Language.Russian, "Please select an interface language" +
                                                               $"{Environment.NewLine}Пожалуйста, выберите язык интерфейса")
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
