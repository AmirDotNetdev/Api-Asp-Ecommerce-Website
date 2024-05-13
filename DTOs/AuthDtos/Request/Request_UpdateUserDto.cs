using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTOs.Request
{
    public class Request_UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}