using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
<<<<<<< HEAD
=======
<<<<<<< HEAD
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string lastname { get; set; }

        [Display(Name = "Contact Number")]
        public string? contactnumber { get; set; }

        public string? address { get; set; }

        [Display(Name = "Profile Image")]
        public string? Img { get; set; }
    }
}
=======
    public enum RoleEnum
    {
        Admin = 0,
        EndUser = 1,
        Interiordesigner = 2
    }
>>>>>>> e53dd0cc56666c3a2212b456684e1f62817019bd
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

<<<<<<< HEAD
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
=======
        [Required]
        public RoleEnum Role { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }

}
>>>>>>> origin/Sami
>>>>>>> e53dd0cc56666c3a2212b456684e1f62817019bd
