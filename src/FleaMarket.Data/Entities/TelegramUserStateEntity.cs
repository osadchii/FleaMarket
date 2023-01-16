using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.TelegramUserState, Schema = DatabaseName.DefaultSchema)]
public class TelegramUserStateEntity : BaseEntity
{
    [ForeignKey(nameof(TelegramBot))]
    public Guid? TelegramBotId { get; set; }

    public TelegramBotEntity TelegramBot { get; set; }

    [Required]
    [ForeignKey(nameof(TelegramUser))]
    public Guid TelegramUserId { get; set; }

    public TelegramUserEntity TelegramUser { get; set; }

    [Required]
    public string State { get; set; }
}