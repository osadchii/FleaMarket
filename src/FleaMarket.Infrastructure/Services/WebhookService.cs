using FleaMarket.Data;
using FleaMarket.Data.Constants;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Telegram.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FleaMarket.Infrastructure.Services;

public interface IWebhookService
{
    Task SetWebhook(string token);
    Task DeleteWebhook(string token);
    Task SetWebhooks();
    Task DeleteWebhooks();
}

public class WebhookService : IWebhookService
{
    private readonly IFleaMarketTelegramBotClient _telegramBotClient;
    private readonly FleaMarketDatabaseContext _context;
    private readonly ILogger<WebhookService> _logger;
    private readonly IOptions<ApplicationConfiguration> _options;

    public WebhookService(FleaMarketDatabaseContext context, ILogger<WebhookService> logger,
        IOptions<ApplicationConfiguration> options, IFleaMarketTelegramBotClient telegramBotClient)
    {
        _context = context;
        _logger = logger;
        _options = options;
        _telegramBotClient = telegramBotClient;
    }

    public async Task SetWebhook(string token)
    {
        var webhookAddress = @$"{_options.Value.Host}/{ApplicationConstant.TelegramController}/{token}";
        await _telegramBotClient.SetWebhook(token, webhookAddress);
        _logger.LogInformation("Webhook for telegram bot with token '{Token}' has been set", token);
    }

    public async Task DeleteWebhook(string token)
    {
        await _telegramBotClient.DeleteWebhook(token);
        _logger.LogInformation("Webhook for telegram bot with token '{Token}' has been deleted", token);
    }

    public async Task SetWebhooks()
    {
        await SetWebhook(_options.Value.ManagementBot.Token);
        
        var telegramBots = await _context.TelegramBots
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => x.Token)
            .ToListAsync();

        foreach (var token in telegramBots)
        {
            await SetWebhook(token);
        }
    }

    public async Task DeleteWebhooks()
    {
        await DeleteWebhook(_options.Value.ManagementBot.Token);
        
        var telegramBots = await _context.TelegramBots
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => x.Token)
            .ToListAsync();

        foreach (var token in telegramBots)
        {
            await DeleteWebhook(token);
        }
    }
}