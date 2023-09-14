using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CesarBmx.Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Mappings
{
    public static class PhoneMessageMapping
    {
        public static void Map(this EntityTypeBuilder<PhoneMessage> entityBuilder)
        {
            entityBuilder.Property(t => t.PhoneNumber)
                .HasColumnType("nvarchar(25)")
                .HasMaxLength(50)
                .IsRequired();            
          
        }
    }
}
