using DecorVista.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Decor_Vista.Models
{
    public class ApplicationContext : DbContext 
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)  {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Designer>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);
        }


        public DbSet<User> Users { get; set; }

        public DbSet<Designer> Designers { get; set; }

    }
}
