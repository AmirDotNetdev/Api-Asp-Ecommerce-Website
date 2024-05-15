using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Data.CustomModels;

namespace TestApi.DTOs.BasePorductDto.Response
{
    public class Response_BaseProductWithPaging
    {
        public List<Model_BaseProductCustom> BaseProduct { get; set; } = new List<Model_BaseProductCustom>();
        public int TotalPages { get; set; }
        

    }
}