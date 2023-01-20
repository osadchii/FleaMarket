using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FleaMarket.Infrastructure.Services.UpdateHandlers;

public interface IUpdateHandleService
{
    Task Handle(string token, Update update);
}

public class UpdateHandleService : IUpdateHandleService
{
    private readonly ITextMessageHandler _textMessageHandler;
    private readonly IMyChatMemberHandler _myChatMemberHandler;

    public UpdateHandleService(ITextMessageHandler textMessageHandler, IMyChatMemberHandler myChatMemberHandler)
    {
        _textMessageHandler = textMessageHandler;
        _myChatMemberHandler = myChatMemberHandler;
    }

    public Task Handle(string token, Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => _textMessageHandler.Handle(token, update.Message),
            UpdateType.MyChatMember => _myChatMemberHandler.Handle(token, update.MyChatMember),
            _ => Task.CompletedTask
        };
    }
}