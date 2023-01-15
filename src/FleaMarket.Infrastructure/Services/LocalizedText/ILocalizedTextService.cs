using FleaMarket.Data.Enums;

namespace FleaMarket.Infrastructure.Services.LocalizedText;

public interface ILocalizedTextService
{
    Task<string> GetText(LocalizedTextId textId, Language language);
}