using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TestApi.Models.ProductModels;

namespace TestApi.DTOs.ProductDtos.Response
{
    public class Response_ProductColor
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    }
}