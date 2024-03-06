using ABQStask.Models;
using Microsoft.EntityFrameworkCore;

namespace ABQStask.Data
{
    public class AbqsContex : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public AbqsContex(DbContextOptions<AbqsContex> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<User>().HasData(
                    new User { UserId = Guid.NewGuid(), Name = "Admin1", Email = "admin999@gmail.com", Phone = "9874563210", Password = "Admin1", isAdmin = true, isDeleted = false }
                    );
        }
    }
}
