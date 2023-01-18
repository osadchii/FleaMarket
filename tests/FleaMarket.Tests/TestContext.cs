using FleaMarket.Data;
using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Telegram.Client;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FleaMarket.Tests;

public abstract partial class TestContext : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider Services;
    protected readonly TestTelegramBotClient TelegramBotClient;
    protected readonly FleaMarketDatabaseContext DatabaseContext;
    protected ITestHarness Harness { get; }

    protected TestContext(TestWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        Services = factory.Server.Services;
        TelegramBotClient = (Services.GetRequiredService<IFleaMarketTelegramBotClient>() as TestTelegramBotClient)!;
        DatabaseContext = Services.GetRequiredService<FleaMarketDatabaseContext>();
        Harness = Services.GetRequiredService<ITestHarness>();
        EnsureLocalizedTexts();
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

    // TODO: FIX
    private void EnsureLocalizedTexts()
    {
        foreach (var textId in Enum.GetValues<LocalizedTextId>())
        {
            var text = Guid.NewGuid().ToString();
            foreach (var language in Enum.GetValues<Language>())
            {
                var exists = DatabaseContext.LocalizedTexts
                    .Where(x => x.Language == language)
                    .Any(x => x.LocalizedTextId == textId);

                if (exists)
                {
                    continue;
                }
                var entity = new LocalizedTextEntity
                {
                    LocalizedText = text,
                    Language = language,
                    LocalizedTextId = textId
                };

                DatabaseContext.LocalizedTexts.Add(entity);
                DatabaseContext.SaveChanges();
            }
        }
    }
}