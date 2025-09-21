using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decor_Vista.Models
{
    public class PortfolioImage
    {
        [Key]
        public int PortfolioImageId { get; set; }

        [Required]
        public int PortfolioId { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }

        [ForeignKey("PortfolioId")]
        public Portfolio Portfolio { get; set; }
    }
}
