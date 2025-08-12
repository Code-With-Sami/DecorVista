using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Decor_Vista.Models
{
    public class ApplicationContext : DbContext 
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)  {  }

        public DbSet<User> Users { get; set; }
    }
}
