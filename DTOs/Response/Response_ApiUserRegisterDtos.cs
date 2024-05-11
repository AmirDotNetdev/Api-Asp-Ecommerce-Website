using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models.AuthModels;

namespace TestApi.DTOs.Response
{
    public class Response_ApiUserRegisterDtos
    {
        public bool isSuccess { get; set; }
        public  ApiUser apiUser { get; set; }
        public List<String> Message { get; set; } = new List<String>();
    }
}