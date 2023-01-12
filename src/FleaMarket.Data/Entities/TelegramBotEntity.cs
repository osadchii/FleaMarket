using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;
using FleaMarket.Data.Enums;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.TelegramBot, Schema = DatabaseName.DefaultSchema)]
public class TelegramBotEntity : BaseEntity
{
    [Required]
    [MaxLength(DatabaseLimit.TelegramBotToken)]
    public string Token { get; set; }

    [Required] 
    public TelegramBotType Type { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid? OwnerId { get; set; }

    public TelegramUserEntity Owner { get; set; }
}