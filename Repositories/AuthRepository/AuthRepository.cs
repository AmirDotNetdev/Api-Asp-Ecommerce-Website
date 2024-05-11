using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestApi.Constants;
using TestApi.Data;
using TestApi.DTOs.Request;
using TestApi.DTOs.Response;
using TestApi.Models.AuthModels;

namespace TestApi.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApiUser> _userManager;
        public AuthRepository(IConfiguration config, ApplicationDbContext dbContext, UserManager<ApiUser> userManager)
        {
            _configuration = config;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Response_ApiUserRegisterDtos> Register(Request_ApiUserRegisterDto userDto)
        {
            var user = new ApiUser()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,

            };
            user.UserName = userDto.Email;
            user.EmailConfirmed = false;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);
                return new Response_ApiUserRegisterDtos()
                {
                    isSuccess = true,
                    apiUser = user,
                };
            }
            List <string> errors = new List<string>();
            foreach(var error in result.Errors)
            {
                errors.Add(error.Description.ToString());
            }
            return new Response_ApiUserRegisterDtos()
            {
                isSuccess = false,
                Message = errors
            };
        }

        public async Task<Response_ApiUserRegisterDtos> RegisterAdmin(Request_ApiUserRegisterDto userDto, int secretKey)
        {
            if(secretKey != 12345)
            {
                return new Response_ApiUserRegisterDtos()
                {
                    isSuccess = false,
                    Message = new List<string>()
                    {"wrong secret key"}
                };
            }
             var user = new ApiUser()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,

            };
            user.UserName = userDto.Email;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Administrator);
                return new Response_ApiUserRegisterDtos()
                {
                    isSuccess = true,
                    apiUser = user,
                };
            }
            List <string> errors = new List<string>();
            foreach(var error in result.Errors)
            {
                errors.Add(error.Description.ToString());
            }
            return new Response_ApiUserRegisterDtos()
            {
                isSuccess = false,
                Message = errors
            };
        }
    }
}