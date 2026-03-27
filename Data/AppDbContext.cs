using Microsoft.EntityFrameworkCore;
using WebApplication8.Entities;

namespace WebApplication8.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.Email).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
