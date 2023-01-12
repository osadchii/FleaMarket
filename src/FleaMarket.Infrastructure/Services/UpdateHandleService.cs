using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace FleaMarket.Infrastructure.Services;

public interface IUpdateHandleService
{
    Task Handle(string token, Update update);
}

public class UpdateHandleService : IUpdateHandleService
{
    private readonly ILogger<UpdateHandleService> _logger;

    public UpdateHandleService(ILogger<UpdateHandleService> logger)
    {
        _logger = logger;
    }

    public Task Handle(string token, Update update)
    {
        _logger.LogInformation("Received message from bot with token {Token}", token);
        return Task.CompletedTask;
    }
}