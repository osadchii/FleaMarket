using FleaMarket.Infrastructure.Telegram.Client;

namespace FleaMarket.Tests;

public class TestTelegramBotClient : IFleaMarketTelegramBotClient
{
    public readonly List<SetWebHookMessage> SetWebhookMessages = new();
    public List<string> DeleteWebhookMessages = new();
    public Task SetWebhook(string token, string url)
    {
        var message = new SetWebHookMessage
        {
            Token = token,
            Url = url
        };
        SetWebhookMessages.Add(message);
        return Task.CompletedTask;
    }

    public Task DeleteWebhook(string token)
    {
        DeleteWebhookMessages.Add(token);
        return Task.CompletedTask;
    }
    
    public class SetWebHookMessage
    {
        public string Token { get; set; }
        public string Url { get; set; }
    }
}