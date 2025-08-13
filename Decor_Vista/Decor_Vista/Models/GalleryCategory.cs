using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class GalleryCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}