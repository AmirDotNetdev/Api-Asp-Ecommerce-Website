using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTOs.BasePorductDto.Request
{
    public class Request_BaseProductDiscount
    {
        [Range(0,99)]
        public int Discount { get; set; }
    }
}