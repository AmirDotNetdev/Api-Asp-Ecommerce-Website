using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.ProductModels
{
    public class BaseProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MainCategoryId { get; set; }
        public virtual MainCategory MainCategory { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public virtual ICollection<ProductVariant> productVariants { get; set; }
        public virtual ICollection<ImageBase> imageBases { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int Discount { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalPrice { get; set; }

    }
}