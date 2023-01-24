using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.TelegramChannel, Schema = DatabaseName.DefaultSchema)]
public class TelegramChannelEntity : BaseEntity
{
    [Required]
    public long ChatId { get; set; }

    [MaxLength(DatabaseLimit.TelegramChannelTitle)]
    public string Title { get; set; }
}