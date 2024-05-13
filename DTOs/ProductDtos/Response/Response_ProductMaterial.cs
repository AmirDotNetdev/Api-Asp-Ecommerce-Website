using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models.ProductModels;

namespace TestApi.DTOs.ProductDtos.Response
{
    public class Response_ProductMaterial
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<Material> Materials { get; set; } = new List<Material>();
    }
}