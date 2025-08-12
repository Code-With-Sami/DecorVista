using System;
using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
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
        [Phone]
        [StringLength(15)]
        public string? ContactNumber { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
