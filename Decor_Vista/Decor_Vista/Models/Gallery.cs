using System;
using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class Gallery
    {
        [Key]
        public int GalleryId { get; set; }

        [Required(ErrorMessage = "A title is required.")]
        [StringLength(150)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a room type.")]
        [StringLength(50)]
        [Display(Name = "Room Type")]
        public string Category { get; set; }

        [StringLength(50)]
        [Display(Name = "Sub-Category (Optional)")]
        public string? SubCategory { get; set; }

        [Required(ErrorMessage = "Please select a theme/style.")]
        [StringLength(50)]
        [Display(Name = "Theme/Style")]
        public string Theme { get; set; }

        [Required(ErrorMessage = "Please select a color scheme.")]
        [StringLength(50)]
        [Display(Name = "Color Scheme")]
        public string ColorScheme { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}