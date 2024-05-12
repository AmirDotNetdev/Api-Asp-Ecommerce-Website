using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.DTOs.Request;
using TestApi.DTOs.Response;

namespace TestApi.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<Response_ApiUserRegisterDtos> Register (Request_ApiUserRegisterDto userDto);
        Task<Response_ApiUserRegisterDtos> RegisterAdmin(Request_ApiUserRegisterDto userDto, int secretKey);
        Task<Response_LoginDto> Login(Request_LoginApi loginApi);
        Task<Response_LoginDto> VerifyAndGenerateTokens(Request_TokenDto tokenDto);
    }
}