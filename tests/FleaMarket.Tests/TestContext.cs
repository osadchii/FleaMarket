using FleaMarket.Data;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Telegram.Client;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Tests;

public abstract partial class TestContext : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    protected readonly IServiceProvider Services;
    protected readonly TestTelegramBotClient TelegramBotClient;
    protected readonly FleaMarketDatabaseContext DatabaseContext;
    protected ITestHarness Harness { get; }

    protected TestContext(TestWebApplicationFactory factory)
    {
        Services = factory.Server.Services;
        TelegramBotClient = (Services.GetRequiredService<IFleaMarketTelegramBotClient>() as TestTelegramBotClient)!;
        DatabaseContext = Services.GetRequiredService<FleaMarketDatabaseContext>();
        Harness = Services.GetRequiredService<ITestHarness>();
    }

    public Task<string> GetLocalizedText(LocalizedTextId textId, Language language)
    {
        return DatabaseContext.LocalizedTexts
            .Where(x => x.Language == language)
            .Where(x => x.LocalizedTextId == textId)
            .Select(x => x.LocalizedText)
            .FirstAsync();
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
        TelegramBotClient.KeyboardMessages.Clear();
    }
}