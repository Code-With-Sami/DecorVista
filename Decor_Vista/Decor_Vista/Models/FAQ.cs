using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        [Display(Name = "Question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Answer is required")]
        [StringLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters")]
        [Display(Name = "Answer")]
        public string Answer { get; set; } = string.Empty;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
