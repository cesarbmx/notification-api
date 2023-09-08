using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class NotificationMapping
    {
        public static void Map(this EntityTypeBuilder<Domain.Models.Notification> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.NotificationId);

            // Properties
            entityBuilder.Property(t => t.NotificationId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            entityBuilder.Property(t => t.UserId)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(t => t.PhoneNumber)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();
            
            entityBuilder.Property(t => t.Text)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(200)
                .IsRequired();

            entityBuilder.Property(t => t.SentAt)
                .HasColumnType("datetime2");
        }
    }
}
