using System;
using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [StringLength(100)]
        [Display(Name = "Brand")]
        public string? Brand { get; set; }

        [StringLength(500)]
        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Detailed Description")]
        public string? DetailedDescription { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sale price must be greater than 0.")]
        [Display(Name = "Sale Price (Optional)")]
        public decimal? SalePrice { get; set; }

        [Display(Name = "Dimensions")]
        public string? Dimensions { get; set; }

        [Display(Name = "Materials")]
        public string? Materials { get; set; }

        [Display(Name = "Color")]
        public string? Color { get; set; }

        [Display(Name = "Style")]
        public string? Style { get; set; }

        [Display(Name = "Weight (kg)")]
        public decimal? Weight { get; set; }

        [Display(Name = "SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; } = 0;

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Main Image")]
        public string? MainImagePath { get; set; }

        [Display(Name = "Additional Images")]
        public string? AdditionalImagesPath { get; set; }

        [Display(Name = "Purchase Link")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? PurchaseLink { get; set; }

        [Display(Name = "Partner Website")]
        public string? PartnerWebsite { get; set; }

        [Display(Name = "Tags")]
        public string? Tags { get; set; }

        [Display(Name = "Rating")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal? Rating { get; set; }

        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
