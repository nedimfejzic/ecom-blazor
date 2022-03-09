using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Featured { get; set; } = false;

        public string ImageUrl { get; set; } = string.Empty;

        public Category ? Category { get; set; }
        public int CategoryId { get; set; }

        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    }
}
