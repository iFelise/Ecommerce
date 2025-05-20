using Ecommerce.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
