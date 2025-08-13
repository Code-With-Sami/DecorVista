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

        [Required(ErrorMessage = "Please select a category.")]
        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        [Display(Name = "Sub-Category (Optional)")]
        public string? SubCategory { get; set; }

       
        [StringLength(255)]
        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}