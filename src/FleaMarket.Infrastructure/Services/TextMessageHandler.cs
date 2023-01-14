using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Services;

public interface ITextMessageHandler
{
    Task Handle(string token, Message message);
}

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ILogger<TextMessageHandler> _logger;

    public TextMessageHandler(ILogger<TextMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(string token, Message message)
    {
        var text = message.Text;
        var chatId = message.Chat.Id;
        
        _logger.LogInformation("Received message with text '{Text}' from Chat with Id {Id}", text, chatId);
        
        return Task.CompletedTask;
    }
}