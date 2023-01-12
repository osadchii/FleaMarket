using System.ComponentModel.DataAnnotations;

namespace FleaMarket.Data.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreatedOn { get; set; }
    
    [Required]
    public DateTime ChangedOn { get; set; }
}