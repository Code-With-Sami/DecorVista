using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [StringLength(200, ErrorMessage = "Image path cannot exceed 200 characters")]
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        [Display(Name = "Author")]
        public string? Author { get; set; }

        [StringLength(200, ErrorMessage = "Tags cannot exceed 200 characters")]
        [Display(Name = "Tags")]
        public string? Tags { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; } = false;

        [Display(Name = "Published Date")]
        public DateTime? PublishedDate { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
