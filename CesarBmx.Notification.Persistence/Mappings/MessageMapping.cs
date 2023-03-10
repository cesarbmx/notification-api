using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class MessageMapping
    {
        public static void Map(this EntityTypeBuilder<Message> entityBuilder)
        {
            // Key
            entityBuilder.HasKey(t => t.MessageId);

            // Properties
            entityBuilder.Property(t => t.MessageId)
                .HasColumnType("uniqueidentifier")
                .IsRequired()
                .ValueGeneratedOnAdd();

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

            entityBuilder.Property(t => t.SentTime)
                .HasColumnType("datetime2");

            entityBuilder.Property(t => t.Time)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }
}
