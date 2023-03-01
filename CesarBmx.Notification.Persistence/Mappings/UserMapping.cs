﻿using System;
using CesarBmx.Shared.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class UserMapping
    {
        public static void Map(this EntityTypeBuilder<User> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.UserId);

            // Properties
            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.PhoneNumber)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            // Data seeding
            var time = DateTime.UtcNow.StripSeconds();
            entityBuilder.HasData(
                new User("master", "+34 666868686", time)
            );
        }
    }
}
