using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
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
   
