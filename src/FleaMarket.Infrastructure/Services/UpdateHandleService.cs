using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FleaMarket.Infrastructure.Services;

public interface IUpdateHandleService
{
    Task Handle(string token, Update update);
}

public class UpdateHandleService : IUpdateHandleService
{
    private readonly ILogger<UpdateHandleService> _logger;
    private readonly ITextMessageHandler _textMessageHandler;

    public UpdateHandleService(ILogger<UpdateHandleService> logger, ITextMessageHandler textMessageHandler)
    {
        _logger = logger;
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