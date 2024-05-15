using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.ProductModels
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public BaseProduct BaseProduct { get; set; }
        public int ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; }    
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
        public int Quantity { get; set; }
        
    }
}