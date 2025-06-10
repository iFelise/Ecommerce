using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIBRARY.Shared.Entity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Producto")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo es Obligatorio")]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public float Stock { get; set; }

        public ICollection<ProdCategory>? ProdCategories { get; set; }

        [Display(Name = "Categorías")]
        public int ProductCategoriesNumber => ProdCategories == null ? 0 : ProdCategories.Count;

        public ICollection<ProductImage>? ProductImages { get; set; }

        [Display(Name = "Imágenes")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        [Display(Name = "Imagén")]
        public string MainImage => ProductImages == null ? string.Empty : ProductImages.FirstOrDefault()!.ImageId;
    }
}