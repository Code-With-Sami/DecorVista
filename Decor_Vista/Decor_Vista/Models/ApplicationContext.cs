using Decor_Vista.Models;
using DecorVista.Models; // Keep both namespaces if needed
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Decor_Vista.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        // Admin & Users
        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<User> Users { get; set; }

        // Gallery & Related
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<GalleryCategory> GalleryCategories { get; set; }
        public DbSet<GalleryTheme> GalleryThemes { get; set; }
        public DbSet<GalleryColorScheme> GalleryColorSchemes { get; set; }
        public DbSet<UserFavorites> UserFavorites { get; set; }

        // Product Catalog
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Cart> Cart { get; set; }

        // Designer
        public DbSet<Designer> Designers { get; set; }
        public DbSet<DesignerService> DesignerServices { get; set; }

        // Portfolio
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioImage> PortfolioImages { get; set; }

        // Site Management
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Gallery Categories (Room Types)
            modelBuilder.Entity<GalleryCategory>().HasData(
                new GalleryCategory { CategoryId = 1, CategoryName = "Living Room" },
                new GalleryCategory { CategoryId = 2, CategoryName = "Bedroom" },
                new GalleryCategory { CategoryId = 3, CategoryName = "Kitchen" },
                new GalleryCategory { CategoryId = 4, CategoryName = "Bathroom" },
                new GalleryCategory { CategoryId = 5, CategoryName = "Office" },
                new GalleryCategory { CategoryId = 6, CategoryName = "Outdoor Space" }
            );

            // Seed Gallery Themes (Styles)
            modelBuilder.Entity<GalleryTheme>().HasData(
                new GalleryTheme { ThemeId = 1, ThemeName = "Modern" },
                new GalleryTheme { ThemeId = 2, ThemeName = "Contemporary" },
                new GalleryTheme { ThemeId = 3, ThemeName = "Traditional" },
                new GalleryTheme { ThemeId = 4, ThemeName = "Minimalist" },
                new GalleryTheme { ThemeId = 5, ThemeName = "Industrial" },
                new GalleryTheme { ThemeId = 6, ThemeName = "Scandinavian" },
                new GalleryTheme { ThemeId = 7, ThemeName = "Bohemian" },
                new GalleryTheme { ThemeId = 8, ThemeName = "Rustic" },
                new GalleryTheme { ThemeId = 9, ThemeName = "Art Deco" },
                new GalleryTheme { ThemeId = 10, ThemeName = "Coastal" }
            );

            // Seed Gallery Color Schemes
            modelBuilder.Entity<GalleryColorScheme>().HasData(
                new GalleryColorScheme { ColorSchemeId = 1, ColorSchemeName = "Neutral" },
                new GalleryColorScheme { ColorSchemeId = 2, ColorSchemeName = "Warm" },
                new GalleryColorScheme { ColorSchemeId = 3, ColorSchemeName = "Cool" },
                new GalleryColorScheme { ColorSchemeId = 4, ColorSchemeName = "Monochromatic" },
                new GalleryColorScheme { ColorSchemeId = 5, ColorSchemeName = "Complementary" },
                new GalleryColorScheme { ColorSchemeId = 6, ColorSchemeName = "Analogous" },
                new GalleryColorScheme { ColorSchemeId = 7, ColorSchemeName = "Bold & Vibrant" },
                new GalleryColorScheme { ColorSchemeId = 8, ColorSchemeName = "Pastel" },
                new GalleryColorScheme { ColorSchemeId = 9, ColorSchemeName = "Earth Tones" },
                new GalleryColorScheme { ColorSchemeId = 10, ColorSchemeName = "Jewel Tones" }
            );

            // Seed Product Categories
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { CategoryId = 1, CategoryName = "Furniture", Description = "Chairs, tables, sofas, beds, and other furniture items", IconClass = "fas fa-couch" },
                new ProductCategory { CategoryId = 2, CategoryName = "Lighting", Description = "Lamps, chandeliers, and lighting fixtures", IconClass = "fas fa-lightbulb" },
                new ProductCategory { CategoryId = 3, CategoryName = "Decor", Description = "Decorative items and accessories", IconClass = "fas fa-star" },
                new ProductCategory { CategoryId = 4, CategoryName = "Rugs and Carpets", Description = "Floor coverings and area rugs", IconClass = "fas fa-square" },
                new ProductCategory { CategoryId = 5, CategoryName = "Wall Art", Description = "Paintings, prints, and wall decorations", IconClass = "fas fa-palette" },
                new ProductCategory { CategoryId = 6, CategoryName = "Curtains and Blinds", Description = "Window treatments and coverings", IconClass = "fas fa-window-maximize" }
            );

            // Configure decimal precision
            modelBuilder.Entity<Designer>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Weight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Rating)
                .HasPrecision(3, 2);
        }
    }
}
