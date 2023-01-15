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

    public UpdateHandleService(ITextMessageHandler textMessageHandler)
    {
        _textMessageHandler = textMessageHandler;
    }

    public Task Handle(string token, Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => _textMessageHandler.Handle(token, update.Message),
            _ => Task.CompletedTask
        };
    }
}