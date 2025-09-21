using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class GalleryColorScheme
    {
        [Key]
        public int ColorSchemeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ColorSchemeName { get; set; }
    }
}
