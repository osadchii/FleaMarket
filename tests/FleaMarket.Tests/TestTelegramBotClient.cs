using FleaMarket.Infrastructure.Telegram.Client;

namespace FleaMarket.Tests;

public class TestTelegramBotClient : IFleaMarketTelegramBotClient
{
    public readonly List<SetWebHookMessage> SetWebhookMessages = new();
    public readonly List<string> DeleteWebhookMessages = new();
    public readonly List<TextMessage> TextMessages = new();
    
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

    public Task SendTextMessage(string token, long chatId, string text)
    {
        var message = new TextMessage
        {
            Token = token,
            ChatId = chatId,
            Text = text
        };
        TextMessages.Add(message);
        
        return Task.CompletedTask;
    }

    public class SetWebHookMessage
    {
        public string Token { get; set; }
        public string Url { get; set; }
    }
    
    public class TextMessage
    {
        public string Token { get; set; }
        public long ChatId { get; set; }
        public string Text { get; set; }
    }
}