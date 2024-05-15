using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestApi.Data.CustomModels;

namespace TestApi.DTOs.BasePorductDto.Response
{
    public class Response_BaseProductWithFullInfo
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public Model_BaseProductCustom baseProduct { get; set; }
    }
}