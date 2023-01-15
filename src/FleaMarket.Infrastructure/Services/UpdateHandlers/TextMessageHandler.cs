using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Services.LocalizedText;
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
    private readonly ILocalizedTextService _localizedTextService;
    private readonly ITelegramUserService _telegramUserService;

    public TextMessageHandler(ILogger<TextMessageHandler> logger, IMessageCommandPublisher publisher, ILocalizedTextService localizedTextService, ITelegramUserService telegramUserService)
    {
        _logger = logger;
        _publisher = publisher;
        _localizedTextService = localizedTextService;
        _telegramUserService = telegramUserService;
    }

    public async Task Handle(string token, Message message)
    {
        var text = message.Text;
        var chatId = message.Chat.Id;

        var user = await _telegramUserService.GetOrCreateTelegramUser(chatId);
        
        _logger.LogInformation("Received message with text '{Text}' from Chat with Id {Id}", text, chatId);

        var toSend = await _localizedTextService.GetText(LocalizedTextId.SelectLanguage, Language.English);

        await _publisher.SendTextMessage(token, chatId, toSend);
    }
}