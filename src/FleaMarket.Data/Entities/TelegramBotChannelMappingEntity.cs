using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.TelegramBotChannelMapping, Schema = DatabaseName.DefaultSchema)]
public class TelegramBotChannelMappingEntity : BaseEntity
{
    [Required]
    [ForeignKey(nameof(TelegramBotEntity))]
    public Guid TelegramBotId { get; set; }

    public TelegramBotEntity TelegramBot { get; set; }

    [Required]
    [ForeignKey(nameof(TelegramChannelEntity))]
    public Guid TelegramChannelId { get; set; }

    public TelegramChannelEntity TelegramChannel { get; set; }

    public bool PostAnnouncements { get; set; }
}