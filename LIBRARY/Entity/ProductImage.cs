using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIBRARY.Shared.Entity
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ImageId { get; set; }

        // Relación 1:1 con Product
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}