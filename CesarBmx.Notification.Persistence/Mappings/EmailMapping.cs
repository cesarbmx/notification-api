using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class EmailMapping
    {
        public static void Map(this EntityTypeBuilder<Email> entityBuilder)
        {
            entityBuilder.Property(t => t.EmailAddress)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .IsRequired();            
          
        }
    }
}
