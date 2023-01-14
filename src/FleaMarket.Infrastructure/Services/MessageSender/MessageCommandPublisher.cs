using FleaMarket.Infrastructure.Extensions;
using FleaMarket.Infrastructure.Services.MessageSender.Models;
using MassTransit;

namespace FleaMarket.Infrastructure.Services.MessageSender;

public interface IMessageCommandPublisher
{
    Task SendTextMessage(string token, long chatId, string text);
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
        var command = new MessageCommand
        {
            Token = token
        };

        var content = new TextMessageContent
        {
            ChatId = chatId,
            Text = text
        };

        var commandItem = new MessageCommandItem
        {
            Type = MessageCommandItemType.Text,
            Content = content.ToJson()
        };
        
        command.Items.Add(commandItem);

        return _publishEndpoint.Publish(command);
    }
}