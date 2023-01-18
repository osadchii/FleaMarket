﻿// <auto-generated />
using System;
using FleaMarket.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FleaMarket.Data.Migrations
{
    [DbContext(typeof(FleaMarketDatabaseContext))]
    [Migration("20230118174136_InvalidTokenText")]
    partial class InvalidTokenText
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FleaMarket.Data.Entities.LocalizedTextEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LocalizedText")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("LocalizedTextId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Language", "LocalizedTextId")
                        .IsUnique();

                    b.ToTable("localized_text", "flea_market");
                });

            modelBuilder.Entity("FleaMarket.Data.Entities.TelegramBotEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("telegram_bot", "flea_market");
                });

            modelBuilder.Entity("FleaMarket.Data.Entities.TelegramUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId")
                        .IsUnique();

                    b.ToTable("telegram_user", "flea_market");
                });

            modelBuilder.Entity("FleaMarket.Data.Entities.TelegramUserStateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TelegramBotId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TelegramUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TelegramBotId");

                    b.HasIndex("TelegramUserId", "TelegramBotId")
                        .IsUnique();

                    b.ToTable("telegram_user_state", "flea_market");
                });

            modelBuilder.Entity("FleaMarket.Data.Entities.TelegramBotEntity", b =>
                {
                    b.HasOne("FleaMarket.Data.Entities.TelegramUserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FleaMarket.Data.Entities.TelegramUserStateEntity", b =>
                {
                    b.HasOne("FleaMarket.Data.Entities.TelegramBotEntity", "TelegramBot")
                        .WithMany()
                        .HasForeignKey("TelegramBotId");

                    b.HasOne("FleaMarket.Data.Entities.TelegramUserEntity", "TelegramUser")
                        .WithMany()
                        .HasForeignKey("TelegramUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TelegramBot");

                    b.Navigation("TelegramUser");
                });
#pragma warning restore 612, 618
        }
    }
}
