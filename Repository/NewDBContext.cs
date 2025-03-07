using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVC_Project.Models;

namespace MVC_Project.Repository
{
    public class NewDBContext : DbContext
    {
        public NewDBContext(DbContextOptions<NewDBContext> options) : base(options) 
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship: Category has many Products
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products) 
                .WithOne(p => p.Category) 
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);  
        }
    }
}
