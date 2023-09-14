using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CesarBmx.Notification.Domain.Types;
using CesarBmx.Shared.Persistence.Extensions;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class NotificationMapping
    {
        public static void Map(this EntityTypeBuilder<Domain.Models.Notification> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.NotificationId);

            // Discriminator
            entityBuilder.HasDiscriminator<NotificationType>(nameof(NotificationType))                
                .HasValue<PhoneMessage>(NotificationType.PHONE_MESSAGE)                
                .HasValue<Email>((NotificationType.EMAIL));

            // Properties
            entityBuilder.Property(t => t.NotificationId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.NotificationType)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .HasStringToEnumConversion()
              .IsRequired();

            entityBuilder.Property(t => t.Text)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(200)
                .IsRequired();

            entityBuilder.Property(t => t.ScheduledFor)
              .HasColumnType("datetime2");

            entityBuilder.Property(t => t.SentAt)
                .HasColumnType("datetime2");
        }
    }
}
