using FleaMarket.Data;
using FleaMarket.Infrastructure.Configurations;
using FleaMarket.Infrastructure.Services.MessageSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FleaMarket.Infrastructure.Services.UpdateHandlers;

public interface IMyChatMemberHandler
{
    Task Handle(string token, ChatMemberUpdated chatMemberUpdated);
}

public class MyChatMemberHandler : IMyChatMemberHandler
{
    private readonly IMessageCommandPublisher _messageCommandPublisher;
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ILogger<MyChatMemberHandler> _logger;

    public MyChatMemberHandler(IMessageCommandPublisher messageCommandPublisher,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> options,
        ILogger<MyChatMemberHandler> logger)
    {
        _messageCommandPublisher = messageCommandPublisher;
        _databaseContext = databaseContext;
        _logger = logger;
        _applicationConfiguration = options.Value;
    }

    public async Task Handle(string token, ChatMemberUpdated chatMemberUpdated)
    {
        if (chatMemberUpdated.Chat.Type != ChatType.Channel)
        {
            return;
        }

        var ownerId = await _databaseContext.TelegramBots
            .Where(x => x.Token == token)
            .Include(x => x.Owner)
            .Select(x => x.Owner.ChatId)
            .FirstOrDefaultAsync();

        if (ownerId == default)
        {
            _logger.LogWarning("Owner's chat id for telegram bot not found");
            return;
        }
        
        
    }
}