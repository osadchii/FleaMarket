using FleaMarket.Data.Entities;

namespace FleaMarket.Tests;

public partial class TestContext : IClassFixture<TestWebApplicationFactory>
{
    public async Task<TelegramBotEntity> CreateTelegramBot(Guid ownerId)
    {
        var entity = new TelegramBotEntity
        {
            OwnerId = ownerId,
            Token = UniqueText,
            IsActive = true
        };

        await DatabaseContext.AddAsync(entity);
        await DatabaseContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TelegramUserEntity> CreateTelegramUser()
    {
        var entity = new TelegramUserEntity
        {
            ChatId = new Random(DateTime.Now.Millisecond).NextInt64()
        };
        
        await DatabaseContext.AddAsync(entity);
        await DatabaseContext.SaveChangesAsync();

        return entity;
    }
}