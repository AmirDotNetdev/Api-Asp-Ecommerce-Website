using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models.ProductModels;

namespace TestApi.Data.CustomModels
{
    public class Model_BaseProductCustom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MainCategory MainCategory { get; set; }
        public Material Material { get; set; }
        public ICollection<Model_BaseImageCustom> ImageBase { get; set; }
        public ICollection<Model_ProductVariantCustom> ProductVariant { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int Discount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
    }
}