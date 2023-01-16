using FleaMarket.Data;
using FleaMarket.Data.Entities;
using FleaMarket.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FleaMarket.Infrastructure.Services.TelegramUserStateService;

public interface ITelegramUserStateService
{
    Task<TelegramUserState> GetState(Guid userId, Guid? botId);
    Task SetState(Guid userId, Guid? botId, TelegramUserState state);
}

public class TelegramUserStateService : ITelegramUserStateService
{
    private readonly FleaMarketDatabaseContext _databaseContext;

    public TelegramUserStateService(FleaMarketDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<TelegramUserState> GetState(Guid userId, Guid? botId)
    {
        var state = await _databaseContext.TelegramUserStates
            .AsNoTracking()
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.TelegramBotId == botId)
            .Select(x => x.State)
            .FirstOrDefaultAsync();

        var deserializedState = state?.FromJson<TelegramUserState>();
        return deserializedState;
    }

    public async Task SetState(Guid userId, Guid? botId, TelegramUserState state)
    {
        var serializedState = state.ToJson();
        
        var userStateEntry = await _databaseContext.TelegramUserStates
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.TelegramBotId == botId)
            .FirstOrDefaultAsync();

        if (userStateEntry is null)
        {
            userStateEntry = new TelegramUserStateEntity
            {
                TelegramBotId = botId,
                TelegramUserId = userId,
                State = serializedState
            };

            await _databaseContext.AddAsync(userStateEntry);
        }
        else
        {
            userStateEntry.State = serializedState;
        }

        await _databaseContext.SaveChangesAsync();
    }
}