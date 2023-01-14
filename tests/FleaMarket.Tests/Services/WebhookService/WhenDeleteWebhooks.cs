using FleaMarket.Data.Entities;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Tests.Constants;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.WebhookService;

public class WhenDeleteWebhooks : TestContext
{
    private readonly IWebhookService _webhookService;
    
    public WhenDeleteWebhooks(TestWebApplicationFactory factory) : base(factory)
    {
        _webhookService = Services.GetRequiredService<IWebhookService>();
    }

    [Fact]
    public async Task ShouldBeDeleted()
    {
        // Arrange

        const int botCount = 1000;
        
        var owner = await CreateTelegramUser();

        var telegramBots = new List<TelegramBotEntity>();

        for (var i = 0; i < botCount; i++)
        {
            var telegramBot = await CreateTelegramBot(owner.Id);
            telegramBots.Add(telegramBot);
        }

        var expectedTokens = telegramBots
            .Select(x => x.Token)
            .ToArray();

        // Act

        await _webhookService.DeleteWebhooks();

        // Assert

        TelegramBotClient.DeleteWebhookMessages
            .ShouldContain(x =>
                x == TestConstant.ManagementBotToken);

        TelegramBotClient.DeleteWebhookMessages
            .Intersect(expectedTokens)
            .Count()
            .ShouldBe(botCount);
    }
}