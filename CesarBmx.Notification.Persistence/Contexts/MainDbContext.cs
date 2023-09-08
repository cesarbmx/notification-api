using System.Diagnostics.CodeAnalysis;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Persistence.Mappings;
using CesarBmx.Shared.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Notification.Persistence.Contexts
{
    public class MainDbContext : DbContext
    {
        public DbSet<Domain.Models.Notification> Notifications { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }

        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Models.Notification>().Map();

            base.OnModelCreating(modelBuilder);

            modelBuilder.UseMasstransit();
        }
    }
}
