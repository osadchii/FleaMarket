using FleaMarket.Data;
using FleaMarket.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleaMarket.Infrastructure.Services;

public interface ITelegramUserService
{
    Task<TelegramUserEntity> GetOrCreateTelegramUser(long chatId);
}

public class TelegramUserService : ITelegramUserService
{
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly ILogger<TelegramUserService> _logger;

    public TelegramUserService(FleaMarketDatabaseContext databaseContext, ILogger<TelegramUserService> logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
    }

    public async Task<TelegramUserEntity> GetOrCreateTelegramUser(long chatId)
    {
        var user = await _databaseContext.TelegramUsers
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync();

        if (user is not null)
        {
            return user;
        }
        
        user = new TelegramUserEntity
        {
            ChatId = chatId
        };

        await _databaseContext.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
            
        _logger.LogInformation("Created new user with Chat Id {ChatId}", chatId);

        return user;
    }
}