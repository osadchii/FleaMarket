using FleaMarket.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FleaMarket.Tests.Services.WebhookService;

public class WhenDeleteWebhook : TestContext
{
    private readonly IWebhookService _webhookService;

    public WhenDeleteWebhook(TestWebApplicationFactory factory) : base(factory)
    {
        _webhookService = Services.GetRequiredService<IWebhookService>();
    }

    [Fact]
    public async Task ShouldBeDeleted()
    {
        // Arrange

        var token = UniqueText;

        // Act

        await _webhookService.DeleteWebhook(token);

        // Assert

        TelegramBotClient.DeleteWebhookMessages
            .ShouldContain(x => x == token);
    }
}