using System.ComponentModel.DataAnnotations;

namespace LIBRARY.Shared.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo es Obligatorio")]
        public string Name { get; set; }
        public ICollection<ProdCategory> ProdCategories { get; set; }

        public int ProdCategoriesNumber => ProdCategories == null ? 0 : ProdCategories.Count;

    }
}