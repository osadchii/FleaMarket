using FleaMarket.Data;
using FleaMarket.Data.Constants;
using FleaMarket.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

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
    private readonly HttpClient _httpClient;
    private readonly FleaMarketDatabaseContext _context;
    private readonly ILogger<WebhookService> _logger;
    private readonly IOptions<ApplicationConfiguration> _options;

    public WebhookService(HttpClient httpClient, FleaMarketDatabaseContext context, ILogger<WebhookService> logger,
        IOptions<ApplicationConfiguration> options)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
        _options = options;
    }

    public async Task SetWebhook(string token)
    {
        var telegramBot = new TelegramBotClient(token, _httpClient);
        var webhookAddress = @$"{_options.Value.Host}/{ApplicationConstant.TelegramController}/{token}";
        await telegramBot.SetWebhookAsync(webhookAddress);
        _logger.LogInformation("Webhook for telegram bot with token '{Token}' has been set", token);
    }

    public async Task DeleteWebhook(string token)
    {
        var telegramBot = new TelegramBotClient(token, _httpClient);
        await telegramBot.DeleteWebhookAsync();
        _logger.LogInformation("Webhook for telegram bot with token '{Token}' has been deleted", token);
    }

    public async Task SetWebhooks()
    {
        await SetWebhook(_options.Value.ManagementBot.Token);
        
        var telegramBots = await _context.TelegramBots
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
            .Where(x => x.IsActive)
            .Select(x => x.Token)
            .ToListAsync();

        foreach (var token in telegramBots)
        {
            await DeleteWebhook(token);
        }
    }
}