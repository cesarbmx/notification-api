using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CesarBmx.Shared.Persistence.Extensions;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class MessageMapping
    {
        public static void Map(this EntityTypeBuilder<Message> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.MessageId);

            // Indexes
            entityBuilder.HasIndex(t => t.UserId);

            // Properties
            entityBuilder.Property(t => t.MessageId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.Telegram)
                .HasColumnType("bit")
                .IsRequired();

            entityBuilder.Property(t => t.Whatsapp)
                .HasColumnType("bit")
                .IsRequired();

            entityBuilder.Property(t => t.Email)
                .HasColumnType("bit")
                .IsRequired();

            entityBuilder.Property(t => t.DeliveryType)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .HasStringToEnumConversion()
                .IsRequired();

            entityBuilder.Property(t => t.PhoneNumber)
                .HasColumnType("nvarchar(25)")
                .HasMaxLength(25);

            entityBuilder.Property(t => t.EmailAddress)
               .HasColumnType("nvarchar(50)")
               .HasMaxLength(50);

            entityBuilder.Property(t => t.Text)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(200)
                .IsRequired();

            entityBuilder.Property(t => t.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired();

            entityBuilder.Property(t => t.ScheduledFor)
                .HasColumnType("datetime2");

            entityBuilder.Property(t => t.SentAt)
                .HasColumnType("datetime2");
        }
    }
}
