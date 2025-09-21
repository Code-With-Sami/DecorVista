using System;
using System.ComponentModel.DataAnnotations;

namespace Decor_Vista.Models
{
    public class UserFavorites
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GalleryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}
