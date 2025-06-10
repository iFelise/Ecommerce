using LIBRARY.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBRARY.Shared.DTO.CategoryDTO
{
    public class ProdCategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsNumber { get; set; }
        public List<ProductResponseDto> Products { get; set; }
    }
}
