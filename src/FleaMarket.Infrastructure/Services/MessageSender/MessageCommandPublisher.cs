using FleaMarket.Infrastructure.Services.MessageSender.Models;
using MassTransit;

namespace FleaMarket.Infrastructure.Services.MessageSender;

public interface IMessageCommandPublisher
{
    Task SendTextMessage(string token, long chatId, string text);
    Task SendKeyboard(string token, long chatId, string text, IEnumerable<IEnumerable<string>> buttons);
}

public class MessageCommandPublisher : IMessageCommandPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageCommandPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
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
}