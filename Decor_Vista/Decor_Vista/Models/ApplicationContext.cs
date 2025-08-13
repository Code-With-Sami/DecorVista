<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
=======
﻿using DecorVista.Models;
using Microsoft.EntityFrameworkCore;
>>>>>>> origin/Sami
using Microsoft.Identity.Client;

namespace Decor_Vista.Models
{
    public class ApplicationContext : DbContext 
    {
<<<<<<< HEAD
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Gallery> Gallery { get; set; } 
        public DbSet<GalleryCategory> GalleryCategories { get; set; } 
=======
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)  {  }
>>>>>>> origin/Sami

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

<<<<<<< HEAD
        
            modelBuilder.Entity<GalleryCategory>().HasData(
                new GalleryCategory { CategoryId = 1, CategoryName = "Living Room" },
                new GalleryCategory { CategoryId = 2, CategoryName = "Bedroom" },
                new GalleryCategory { CategoryId = 3, CategoryName = "Kitchen" },
                new GalleryCategory { CategoryId = 4, CategoryName = "Bathroom" },
                new GalleryCategory { CategoryId = 5, CategoryName = "Office" },
                new GalleryCategory { CategoryId = 6, CategoryName = "Outdoor Space" }
            );
        }
=======
            modelBuilder.Entity<Designer>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);
        }


        public DbSet<User> Users { get; set; }

        public DbSet<Designer> Designers { get; set; }

>>>>>>> origin/Sami
    }
}
