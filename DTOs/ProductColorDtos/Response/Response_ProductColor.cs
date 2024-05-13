using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.XMP.Options;
using TestApi.Models.ProductModels;

namespace TestApi.DTOs.ProductColorDtos.Response
{
    public class Response_ProductColor
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}