using FleaMarket.Data.Enums;

namespace FleaMarket.Data.Utils;

public static class LocalizedTextFunction
{
    private const string AddLocalizedTextTemplate = @"
INSERT INTO
flea_market.localized_text
(""Id"", ""Language"", ""LocalizedTextId"", ""LocalizedText"", ""CreatedOn"", ""ChangedOn"")
VALUES
('{0}', '{1}', '{2}', '{3}', timezone('utc', now()), timezone('utc', now()))";

    private const string DeleteLocalizedTextTemplate = @"
DELETE FROM
flea_market.localized_text
WHERE
""Language"" = '{0}'
AND ""LocalizedTextId"" = '{1}'";

    public static string AddLocalizedText(LocalizedTextId textId, Language language, string text)
    {
        var sql = string.Format(AddLocalizedTextTemplate, Guid.NewGuid(), language.ToString(), textId.ToString(), text);
        return sql;
    }

    public static string DeleteLocalizedText(LocalizedTextId textId, Language language)
    {
        var sql = string.Format(DeleteLocalizedTextTemplate, language.ToString(), textId.ToString());
        return sql;
    }
}