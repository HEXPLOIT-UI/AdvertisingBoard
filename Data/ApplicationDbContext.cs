using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(e => e.Login).IsUnique();
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(e => e.Name).IsUnique();
        }
    }
}
