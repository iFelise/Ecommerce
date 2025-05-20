using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "URL de la imagen")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ImageUrl { get; set; } = null!;

        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
