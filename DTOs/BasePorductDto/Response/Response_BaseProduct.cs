using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Data.CustomModels;
using TestApi.DTOs.BasePorductDto.CustomModels;

namespace TestApi.DTOs.BasePorductDto.Response
{
    public class Response_BaseProduct
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<Model_BaseProductWithNoExtraInfo> baseProducts { get; set; } = new List<Model_BaseProductWithNoExtraInfo>();
    }
}