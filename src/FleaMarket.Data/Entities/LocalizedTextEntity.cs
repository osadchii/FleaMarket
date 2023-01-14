using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleaMarket.Data.Constants;
using FleaMarket.Data.Enums;

namespace FleaMarket.Data.Entities;

[Table(DatabaseName.LocalizedText, Schema = DatabaseName.DefaultSchema)]
public class LocalizedTextEntity : BaseEntity
{
    [Required]
    public Language Language { get; set; }
    
    [Required]
    public LocalizedTextId LocalizedTextId { get; set; }
    
    [Required]
    [MaxLength(DatabaseLimit.LocalizedText)]
    public string LocalizedText { get; set; }
}