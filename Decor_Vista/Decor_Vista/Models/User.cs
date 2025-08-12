using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public enum RoleEnum
    {
        Admin = 0,
        EndUser = 1,
        Interiordesigner = 2
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public RoleEnum Role { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }

}
