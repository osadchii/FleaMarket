using FleaMarket.Data.Constants;
using FleaMarket.Data.Entities;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Tests.Constants;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.WebhookService;

public class WhenSetWebhooks : TestContext
{
    private readonly IWebhookService _webhookService;

    public WhenSetWebhooks(TestWebApplicationFactory factory) : base(factory)
    {
        _webhookService = Services.GetRequiredService<IWebhookService>();
    }

    [Fact]
    public async Task ShouldBeSet()
    {
        // Arrange

        const int botCount = 1000;
        const string urlFormat = $"{TestConstant.Host}/{ApplicationConstant.TelegramController}/{{0}}";

        var owner = await CreateTelegramUser();

        var telegramBots = new List<TelegramBotEntity>();

        for (var i = 0; i < botCount; i++)
        {
            var telegramBot = await CreateTelegramBot(owner.Id);
            telegramBots.Add(telegramBot);
        }

        var expectedUrls = telegramBots
            .Select(x => string.Format(urlFormat, x.Token))
            .ToArray();

        // Act

        await _webhookService.SetWebhooks();

        // Assert

        TelegramBotClient.SetWebhookMessages
            .ShouldContain(x =>
                x.Token == TestConstant.ManagementBotToken &&
                x.Url == string.Format(urlFormat, TestConstant.ManagementBotToken));

        TelegramBotClient.SetWebhookMessages
            .IntersectBy(expectedUrls, x => x.Url)
            .Count()
            .ShouldBe(botCount);
    }
}