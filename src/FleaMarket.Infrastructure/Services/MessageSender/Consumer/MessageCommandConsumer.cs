using FleaMarket.Infrastructure.Services.MessageSender.Models;
using MassTransit;

namespace FleaMarket.Infrastructure.Services.MessageSender.Consumer;

public class MessageCommandConsumer : IConsumer<MessageCommand>
{
    private readonly IMessageCommandHandler _messageCommandHandler;

    public MessageCommandConsumer(IMessageCommandHandler messageCommandHandler)
    {
        _messageCommandHandler = messageCommandHandler;
    }

    public Task Consume(ConsumeContext<MessageCommand> context)
    {
        var message = context.Message;
        return _messageCommandHandler.Handle(message);
    }
}