using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models.ProductModels;

namespace TestApi.DTOs.ProductDtos.Response
{
    public class Response_MainCategory
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<MainCategory> MainCategory { get; set; } = new List<MainCategory>();
    }
}