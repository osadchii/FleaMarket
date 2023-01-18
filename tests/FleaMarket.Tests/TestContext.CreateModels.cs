using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace FleaMarket.Tests;

public partial class TestContext
{
    protected Task<TelegramBotEntity> CreateTelegramBot(Guid ownerId, Action<TelegramBotEntity>? populate = null)
    {
        return CreateEntity(new TelegramBotEntity
        {
            OwnerId = ownerId,
            Token = UniqueText,
            IsActive = true
        }, populate);
    }

    protected Task<TelegramUserEntity> CreateTelegramUser(Action<TelegramUserEntity>? populate = null)
    {
        return CreateEntity(new TelegramUserEntity
        {
            ChatId = new Random(DateTime.Now.Millisecond).NextInt64()
        }, populate);
    }

    private async Task<TEntity> CreateEntity<TEntity>(TEntity entity, Action<TEntity>? populate = null)
        where TEntity : BaseEntity, new()
    {
        populate?.Invoke(entity);
        await DatabaseContext.AddAsync(entity);
        await DatabaseContext.SaveChangesAsync();

        return entity;
    }
}