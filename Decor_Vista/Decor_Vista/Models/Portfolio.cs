using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decor_Vista.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; }

        [Required]
        public int DesignerId { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; } // FK from GalleryCategories

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? MainImagePath { get; set; }

        [StringLength(255)]
        public string? ProjectLink { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("CategoryId")]
        public GalleryCategory Category { get; set; }

        public List<PortfolioImage> Images { get; set; }
    }
}
