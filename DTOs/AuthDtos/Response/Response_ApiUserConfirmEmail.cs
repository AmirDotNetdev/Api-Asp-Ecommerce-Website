using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTOs.Response
{
    public class Response_ApiUserConfirmEmail
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}