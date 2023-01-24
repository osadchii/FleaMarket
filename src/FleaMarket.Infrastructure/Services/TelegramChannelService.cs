using FleaMarket.Data;
using FleaMarket.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FleaMarket.Infrastructure.Services;

public interface ITelegramChannelService
{
    Task<TelegramChannelEntity> GetOrCreateTelegramChannel(long chatId, string title);
    Task CreateBotChannelMappingIfNotExist(Guid telegramBotId, Guid telegramChannelId);
    Task SetMappingPostAnnouncementValue(Guid telegramBotId, Guid telegramChannelId, bool value);
}

public class TelegramChannelService : ITelegramChannelService
{
    private readonly FleaMarketDatabaseContext _databaseContext;
    private readonly ILogger<TelegramChannelService> _logger;

    public TelegramChannelService(FleaMarketDatabaseContext databaseContext, ILogger<TelegramChannelService> logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
    }

    public async Task<TelegramChannelEntity> GetOrCreateTelegramChannel(long chatId, string title)
    {
        var channel = await _databaseContext.TelegramChannels
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync();

        if (channel is not null)
        {
            if (channel.Title != title)
            {
                channel.Title = title;
                await _databaseContext.SaveChangesAsync();
                _logger.LogInformation("Updated title for channel with Chat Id {ChatId} to '{Title}'", chatId, title);
            }

            return channel;
        }

        channel = new TelegramChannelEntity
        {
            ChatId = chatId,
            Title = title
        };

        await _databaseContext.AddAsync(channel);
        await _databaseContext.SaveChangesAsync();

        _logger.LogInformation("Created new channel with Chat Id {ChatId}", chatId);

        return channel;
    }

    public async Task CreateBotChannelMappingIfNotExist(Guid telegramBotId, Guid telegramChannelId)
    {
        var exists = await _databaseContext.TelegramBotChannelMappings
            .Where(x => x.TelegramBotId == telegramBotId)
            .AnyAsync(x => x.TelegramChannelId == telegramChannelId);

        if (exists)
        {
            return;
        }

        var entity = new TelegramBotChannelMappingEntity
        {
            TelegramBotId = telegramBotId,
            TelegramChannelId = telegramChannelId
        };

        await _databaseContext.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();

        _logger.LogInformation("Created bot with Id {TelegramBotId} and channel with Id {TelegramChannelId} mapping",
            telegramBotId, telegramChannelId);
    }

    public async Task SetMappingPostAnnouncementValue(Guid telegramBotId, Guid telegramChannelId, bool value)
    {
        var entry = await _databaseContext.TelegramBotChannelMappings
            .Where(x => x.TelegramBotId == telegramBotId)
            .Where(x => x.TelegramChannelId == telegramChannelId)
            .SingleOrDefaultAsync();

        if (entry is null)
        {
            entry = new TelegramBotChannelMappingEntity
            {
                TelegramBotId = telegramBotId,
                TelegramChannelId = telegramChannelId,
                PostAnnouncements = value
            };

            await _databaseContext.AddAsync(entry);
            await _databaseContext.SaveChangesAsync();

            _logger.LogInformation(
                "Created bot with Id {TelegramBotId} and channel with Id {TelegramChannelId} mapping with PostAnnouncements value {PostAnnouncements}",
                telegramBotId, telegramChannelId, value);

            return;
        }

        entry.PostAnnouncements = value;
        await _databaseContext.SaveChangesAsync();

        _logger.LogInformation(
            "Updated bot with Id {TelegramBotId} and channel with Id {TelegramChannelId} mapping PostAnnouncements value to {PostAnnouncements}",
            telegramBotId, telegramChannelId, value);
    }
}