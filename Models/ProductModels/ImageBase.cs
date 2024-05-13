using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.ProductModels
{
    public class ImageBase
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime AddedOn { get; set; }
        public int BaseProductId { get; set; }
        public BaseProduct BaseProduct { get; set; }
        public string StaticPath { get; set; }
    }
}