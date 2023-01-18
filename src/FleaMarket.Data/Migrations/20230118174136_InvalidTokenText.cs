using FleaMarket.Data.Enums;
using FleaMarket.Data.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class InvalidTokenText : Migration
    {
        private readonly (LocalizedTextId TextId, Language Language, string value)[] Values = new[]
        {
            (LocalizedTextId.InvalidToken, Language.English, "Token is not valid"),
            (LocalizedTextId.InvalidToken, Language.Russian, "Неверный токен"),
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
