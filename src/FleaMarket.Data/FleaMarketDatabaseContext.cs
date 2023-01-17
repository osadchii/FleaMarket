using FleaMarket.Data.Entities;
using FleaMarket.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FleaMarket.Data;

public sealed class FleaMarketDatabaseContext : DbContext
{
    public DbSet<TelegramUserEntity> TelegramUsers { get; set; }
    public DbSet<TelegramBotEntity> TelegramBots { get; set; }
    public DbSet<LocalizedTextEntity> LocalizedTexts { get; set; }
    public DbSet<TelegramUserStateEntity> TelegramUserStates { get; set; }

    public FleaMarketDatabaseContext(DbContextOptions<FleaMarketDatabaseContext> options) : base(options)
    {
        ChangeTracker.StateChanged += UpdateAuditFields;
        ChangeTracker.Tracked += UpdateAuditFields;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUserEntity>(entity =>
        {
            entity
                .HasIndex(x => x.ChatId)
                .IsUnique();
            
            entity
                .Property(x => x.Language)
                .HasConversion(new EnumToStringConverter<Language>());
        });
        
        modelBuilder.Entity<TelegramBotEntity>(entity =>
        {
            entity
                .HasIndex(x => x.Token)
                .IsUnique();
            
            entity
                .HasIndex(x => x.OwnerId);
        });

        modelBuilder.Entity<LocalizedTextEntity>(entity =>
        {
            entity
                .HasIndex(x => new { x.Language, x.LocalizedTextId })
                .IsUnique();
            
            entity
                .Property(x => x.Language)
                .HasConversion(new EnumToStringConverter<Language>());
            
            entity
                .Property(x => x.LocalizedTextId)
                .HasConversion(new EnumToStringConverter<LocalizedTextId>());
        });

        modelBuilder.Entity<TelegramUserStateEntity>(entity =>
        {
            entity
                .HasIndex(x => new { x.TelegramUserId, x.TelegramBotId })
                .IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }

    private static void UpdateAuditFields(object sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is not BaseEntity entity)
        {
            return;
        }

        var now = DateTime.UtcNow;

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (e.Entry.State)
        {
            case EntityState.Modified:
                entity.ChangedOn = now;
                e.Entry.Properties.First(x => x.Metadata.Name == nameof(BaseEntity.ChangedOn))
                    .IsModified = true;
                break;
            case EntityState.Added:
                entity.ChangedOn = now;
                entity.CreatedOn = now;
                break;
        }
    }
}