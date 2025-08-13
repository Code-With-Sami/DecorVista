using DecorVista.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Decor_Vista.Models
{
    public class ApplicationContext : DbContext 
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Gallery> Gallery { get; set; } 
        public DbSet<GalleryCategory> GalleryCategories { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<GalleryCategory>().HasData(
                new GalleryCategory { CategoryId = 1, CategoryName = "Living Room" },
                new GalleryCategory { CategoryId = 2, CategoryName = "Bedroom" },
                new GalleryCategory { CategoryId = 3, CategoryName = "Kitchen" },
                new GalleryCategory { CategoryId = 4, CategoryName = "Bathroom" },
                new GalleryCategory { CategoryId = 5, CategoryName = "Office" },
                new GalleryCategory { CategoryId = 6, CategoryName = "Outdoor Space" }
            );
        
            modelBuilder.Entity<Designer>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);
        }
        public DbSet<Designer> Designers { get; set; }

    }
}
