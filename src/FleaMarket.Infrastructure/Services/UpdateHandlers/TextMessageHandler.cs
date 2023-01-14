using FleaMarket.Infrastructure.Services.MessageSender;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Services.UpdateHandlers;

public interface ITextMessageHandler
{
    Task Handle(string token, Message message);
}

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ILogger<TextMessageHandler> _logger;
    private readonly IMessageCommandPublisher _publisher;

    public TextMessageHandler(ILogger<TextMessageHandler> logger, IMessageCommandPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(string token, Message message)
    {
        var text = message.Text;
        var chatId = message.Chat.Id;
        
        _logger.LogInformation("Received message with text '{Text}' from Chat with Id {Id}", text, chatId);

        await _publisher.SendTextMessage(token, chatId, text);
    }
}