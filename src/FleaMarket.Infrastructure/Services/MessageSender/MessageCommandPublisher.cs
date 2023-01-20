using FleaMarket.Data.Enums;
using FleaMarket.Infrastructure.Services.LocalizedText;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using MassTransit;

namespace FleaMarket.Infrastructure.Services.MessageSender;

public interface IMessageCommandPublisher
{
    Task SendTextMessage(string token, long chatId, LocalizedTextId textId, Language language);
    Task SendTextMessage(string token, long chatId, string text);
    Task SendKeyboard(string token, long chatId, string text, IEnumerable<IEnumerable<string>> buttons);
    Task SendKeyboard(string token, long chatId, LocalizedTextId textId, Language language, IEnumerable<IEnumerable<string>> buttons);
}

public class MessageCommandPublisher : IMessageCommandPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILocalizedTextService _localizedTextService;

    public MessageCommandPublisher(IPublishEndpoint publishEndpoint, ILocalizedTextService localizedTextService)
    {
        _publishEndpoint = publishEndpoint;
        _localizedTextService = localizedTextService;
    }

    public async Task SendTextMessage(string token, long chatId, LocalizedTextId textId, Language language)
    {
        var text = await _localizedTextService.GetText(textId, language);
        await SendTextMessage(token, chatId, text);
    }

    public Task SendTextMessage(string token, long chatId, string text)
    {
        var command = new MessageCommand(token);
        var content = new TextMessageContent(text);
        command.AddItem(MessageCommandItemType.Text, chatId, content);

        return _publishEndpoint.Publish(command);
    }

    public Task SendKeyboard(string token, long chatId, string text, IEnumerable<IEnumerable<string>> buttons)
    {
        var command = new MessageCommand(token);
        var content = new KeyboardMessageContent(text, buttons);
        command.AddItem(MessageCommandItemType.Keyboard, chatId, content);

        return _publishEndpoint.Publish(command);
    }

    public async Task SendKeyboard(string token, long chatId, LocalizedTextId textId, Language language, IEnumerable<IEnumerable<string>> buttons)
    {
        var text = await _localizedTextService.GetText(textId, language);
        await SendKeyboard(token, chatId, text, buttons);
    }
}