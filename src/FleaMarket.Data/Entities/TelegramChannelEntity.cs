using System.ComponentModel.DataAnnotations;

namespace FleaMarket.Data.Entities;

public class TelegramChannelEntity : BaseEntity
{
    [Required]
    public long ChatId { get; set; }
}