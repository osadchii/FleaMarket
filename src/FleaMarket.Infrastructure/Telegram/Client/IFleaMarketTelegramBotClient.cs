namespace FleaMarket.Infrastructure.Telegram.Client;

public interface IFleaMarketTelegramBotClient
{
    Task SetWebhook(string token, string url);
    Task DeleteWebhook(string token);
}