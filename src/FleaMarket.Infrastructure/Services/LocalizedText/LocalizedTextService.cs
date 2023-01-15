using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Services.LocalizedText.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FleaMarket.Infrastructure.Services.LocalizedText;

public class LocalizedTextService : ILocalizedTextService
{
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly IMemoryCache _memoryCache;
    private const int ExpirationInMinutes = 5;

    public LocalizedTextService(FleaMarketDatabaseContext databaseContext, IMemoryCache memoryCache)
    {
        _databaseContext = databaseContext;
        _memoryCache = memoryCache;
    }

    private const string CacheKeyPrefix = nameof(LocalizedTextService);

    public async Task<string> GetText(LocalizedTextId textId, Language language)
    {
        var cacheKey = GetCacheKey(textId, language);

        if (_memoryCache.TryGetValue(cacheKey, out string value))
        {
            return value;
        }

        value = await _databaseContext.LocalizedTexts
            .AsNoTracking()
            .Where(x => x.LocalizedTextId == textId)
            .Where(x => x.Language == language)
            .Select(x => x.LocalizedText)
            .SingleOrDefaultAsync();

        if (value == default)
        {
            throw new LocalizedTextNotFoundException();
        }

        _memoryCache.Set(cacheKey, value, TimeSpan.FromMinutes(ExpirationInMinutes));

        return value;
    }

    private static string GetCacheKey(LocalizedTextId textId, Language language)
    {
        return $"{CacheKeyPrefix}_{textId.ToString()}_{language.ToString()}";
    }
}