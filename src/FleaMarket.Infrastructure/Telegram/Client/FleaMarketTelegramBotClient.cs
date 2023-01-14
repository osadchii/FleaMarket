using Telegram.Bot;
using Telegram.Bot.Types.Enums;

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

    public async Task SendTextMessage(string token, long chatId, string text)
    {
        var telegramBotClient = new TelegramBotClient(token, _httpClient);
        await telegramBotClient.SendChatActionAsync(chatId, ChatAction.Typing);
        await Task.Delay(1000);
        await telegramBotClient.SendTextMessageAsync(chatId, text, ParseMode.Html);
    }
}