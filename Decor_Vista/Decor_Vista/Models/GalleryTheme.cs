using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class GalleryTheme
    {
        [Key]
        public int ThemeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ThemeName { get; set; }
    }
}
