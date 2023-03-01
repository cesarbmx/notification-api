﻿using CesarBmx.Shared.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class OrderMapping
    {
        public static void Map(this EntityTypeBuilder<Order> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.OrderId);

            // Relationships
            entityBuilder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .HasOne<Currency>()
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Properties
            entityBuilder.Property(t => t.OrderId)
                .HasColumnType("int")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entityBuilder.Property(t => t.WatcherId)
                .HasColumnType("int")
                .IsRequired();

            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.CurrencyId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            entityBuilder.Property(t => t.Quantity)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            entityBuilder.Property(t => t.OrderType)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasStringToEnumConversion()
                .IsRequired();

            entityBuilder.Property(t => t.OrderStatus)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .HasStringToEnumConversion()
                .IsRequired();

            entityBuilder.Property(t => t.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            entityBuilder.Property(t => t.ClosedAt)
                .HasColumnType("datetime2");

            entityBuilder.Property(t => t.NotifiedAt)
                    .HasColumnType("datetime2");
        }
    }
}
