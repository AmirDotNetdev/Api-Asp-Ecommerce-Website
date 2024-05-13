using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.ProductModels
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductVariant> ProductVariants { get; set; }
    }
}