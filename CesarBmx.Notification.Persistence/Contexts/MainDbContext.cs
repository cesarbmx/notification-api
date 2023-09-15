using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Persistence.Mappings;
using CesarBmx.Shared.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Contexts
{
    public class MainDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().Map();

            base.OnModelCreating(modelBuilder);

            // Masstransit outbox
            modelBuilder.UseMasstransitOutbox();
        }
    }
}
