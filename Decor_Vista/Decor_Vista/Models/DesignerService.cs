using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecorVista.Models
{
    public enum PricingType
    {
        Hourly = 1,
        Fixed = 2,
        StartingFrom = 3
    }

    public class DesignerService
    {
        [Key]
        public int Id { get; set; }

        // Ownership
        [Required]
        public int DesignerId { get; set; }

        // Core
        [Required, StringLength(150)]
        [Display(Name = "Service Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Category { get; set; }

        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Pricing Type")]
        public PricingType PricingType { get; set; } = PricingType.StartingFrom;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }  // required when PricingType != StartingFrom? (we validate in controller)

        [StringLength(100)]
        [Display(Name = "Typical Duration")]
        public string? DurationText { get; set; } // e.g. "2 weeks", "1 month"

        // Optional media
        [Display(Name = "Cover Image")]
        public string? CoverImagePath { get; set; }

        // Flags
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Featured")]
        public bool IsFeatured { get; set; } = false;

        // Audit
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
