using Telegram.Bot;

namespace FleaMarket.Infrastructure.Telegram.Client;

public class FleaMarketTelegramBotClient : IFleaMarketTelegramBotClient
{
    private readonly HttpClient _httpClient;

    public FleaMarketTelegramBotClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task SetWebhook(string token, string url)
    {
        var telegramBotClient = new TelegramBotClient(token, _httpClient);
        return telegramBotClient.SetWebhookAsync(url);
    }

    public Task DeleteWebhook(string token)
    {
        var telegramBotClient = new TelegramBotClient(token, _httpClient);
        return telegramBotClient.DeleteWebhookAsync();
    }
}