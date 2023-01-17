﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;
using FleaMarket.Data.Enums;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.TelegramUser, Schema = DatabaseName.DefaultSchema)]
public class TelegramUserEntity : BaseEntity
{
    [Required]
    public long ChatId { get; set; }

    public Language Language { get; set; }
}