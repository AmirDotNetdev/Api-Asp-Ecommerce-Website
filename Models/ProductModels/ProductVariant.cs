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
        public virtual BaseProduct BaseProduct { get; set; }
        public int ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }    
        public int ProductSizeId { get; set; }
        public virtual ProductSize ProductSize { get; set; }
        public int Quantity { get; set; }
        
    }
}