using FleaMarket.Data;
using FleaMarket.Infrastructure.Telegram.Client;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Tests;

public abstract partial class TestContext : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider Services;
    protected readonly TestTelegramBotClient TelegramBotClient;
    protected readonly FleaMarketDatabaseContext DatabaseContext;

    protected TestContext(TestWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        Services = factory.Server.Services;
        TelegramBotClient = (Services.GetRequiredService<IFleaMarketTelegramBotClient>() as TestTelegramBotClient)!;
        DatabaseContext = Services.GetRequiredService<FleaMarketDatabaseContext>();
    }

    protected static string UniqueText =>
        Guid.NewGuid().ToString().Replace("-", "") +
        Guid.NewGuid().ToString().Replace("-", "") +
        Guid.NewGuid().ToString().Replace("-", "");

    public void Dispose()
    {
        TelegramBotClient.TextMessages.Clear();
        TelegramBotClient.DeleteWebhookMessages.Clear();
        TelegramBotClient.SetWebhookMessages.Clear();
    }
}