using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;

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

    protected Task<LocalizedTextEntity> CreateLocalizedText(Action<LocalizedTextEntity>? populate = null)
    {
        return CreateEntity(new LocalizedTextEntity
        {
            LocalizedText = UniqueText,
            Language = Language.English,
            LocalizedTextId = LocalizedTextId.SelectLanguage
        }, populate);
    }

    private async Task<TEntity> CreateEntity<TEntity>(TEntity entity, Action<TEntity>? populate = null) where TEntity : BaseEntity, new()
    {
        populate?.Invoke(entity);
        await DatabaseContext.AddAsync(entity);
        await DatabaseContext.SaveChangesAsync();

        return entity;
    }
}