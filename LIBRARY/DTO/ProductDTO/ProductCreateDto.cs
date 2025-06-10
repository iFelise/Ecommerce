using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LIBRARY.Shared.DTO.ProductDTO
{
    public class ProductCreateDto
    {
        public int Id { get; set; }
        public int ProdCategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public IFormFile ImageFile { get; set; }
    }

}
