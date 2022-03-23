using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal Total { get; set; }
        public string Product { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
