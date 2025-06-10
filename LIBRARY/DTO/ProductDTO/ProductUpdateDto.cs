using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBRARY.Shared.DTO.ProductDTO
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public int ProdCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public IFormFile? ImageFile { get; set; } // opcional para PUT
    }
}
