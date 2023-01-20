using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
        await telegramBotClient.SendTextMessageAsync(chatId, text, ParseMode.Html);
    }

    public async Task SendKeyboard(string token, long chatId, string text, IEnumerable<IEnumerable<string>> buttons)
    {
        var telegramBotClient = new TelegramBotClient(token, _httpClient);
        var keyboardButtons = buttons.Select(x =>
            x.Select(y => new KeyboardButton(y)));

        var replyMarkup = new ReplyKeyboardMarkup(keyboardButtons)
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
            InputFieldPlaceholder = text
        };
        await telegramBotClient.SendTextMessageAsync(chatId, text, ParseMode.Html, replyMarkup: replyMarkup);
    }

    public Task<bool> TestToken(string token)
    {
        var telegramBotClient = new TelegramBotClient(token, _httpClient);
        return telegramBotClient.TestApiAsync();
    }
}