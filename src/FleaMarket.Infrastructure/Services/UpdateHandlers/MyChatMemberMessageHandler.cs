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
    private readonly ITelegramChannelService _telegramChannelService;

    public MyChatMemberHandler(IMessageCommandPublisher messageCommandPublisher,
        FleaMarketDatabaseContext databaseContext, IOptions<ApplicationConfiguration> options,
        ILogger<MyChatMemberHandler> logger, ITelegramChannelService telegramChannelService)
    {
        _messageCommandPublisher = messageCommandPublisher;
        _databaseContext = databaseContext;
        _logger = logger;
        _telegramChannelService = telegramChannelService;
        _applicationConfiguration = options.Value;
    }

    public async Task Handle(string token, ChatMemberUpdated chatMemberUpdated)
    {
        var chat = chatMemberUpdated.Chat;

        if (chat.Type != ChatType.Channel)
        {
            return;
        }

        var chatId = chat.Id;
        var title = chat.Title;

        var ownerData = await _databaseContext.TelegramBots
            .AsNoTracking()
            .Where(x => x.Token == token)
            .Include(x => x.Owner)
            .Select(x => new
            {
                ChatId = x.Owner.ChatId,
                OwnerId = x.Owner.Id,
                BotId = x.Id
            })
            .FirstOrDefaultAsync();

        if (ownerData is null)
        {
            _logger.LogWarning("Owner's chat id for telegram bot not found");
            return;
        }

        var telegramChannel = await _telegramChannelService.GetOrCreateTelegramChannel(chatId, title);
        await _telegramChannelService.CreateBotChannelMappingIfNotExist(ownerData.BotId, telegramChannel.Id);
    }
}