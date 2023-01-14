using FleaMarket.Data.Constants;
using FleaMarket.Infrastructure.Services;
using FleaMarket.Tests.Constants;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.WebhookService;

public class WhenSetWebhook : TestContext
{
    private readonly IWebhookService _webhookService;

    public WhenSetWebhook(TestWebApplicationFactory factory) : base(factory)
    {
        _webhookService = Services.GetRequiredService<IWebhookService>();
    }

    [Fact]
    public async Task ShouldBeSet()
    {
        // Arrange

        var token = UniqueText;
        var url = $"{TestConstant.Host}/{ApplicationConstant.TelegramController}/{token}";

        // Act

        await _webhookService.SetWebhook(token);

        // Assert

        TelegramBotClient.SetWebhookMessages
            .ShouldContain(x => x.Token == token && x.Url == url);
    }
}