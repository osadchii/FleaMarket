namespace FleaMarket.Infrastructure.Telegram.Client;

public interface IFleaMarketTelegramBotClient
{
    Task SetWebhook(string token, string url);
    Task DeleteWebhook(string token);
    Task SendTextMessage(string token, long chatId, string text);
    Task SendKeyboard(string token, long chatId, string text, IEnumerable<IEnumerable<string>> buttons);
}