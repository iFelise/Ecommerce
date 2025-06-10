using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBRARY.Shared.DTO.CategoryDTO
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProdCategoriesNumber { get; set; }
        public List<ProdCategoryResponseDto> ProdCategories { get; set; }
    }
}
