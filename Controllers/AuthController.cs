using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTOs.Request;
using TestApi.DTOs.Response;
using TestApi.Models.AuthModels;
using TestApi.Repositories.AuthRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly UserManager<ApiUser> _userManager;
        public AuthController(IAuthRepository authRepository, UserManager<ApiUser> userManager)
        {
            _authRepo = authRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Response_ApiUserRegisterDtos>> Register ([FromBody] Request_ApiUserRegisterDto request_ApiUserRegisterDto)
        {
            var userDto = await _authRepo.Register(request_ApiUserRegisterDto);
            if(userDto.isSuccess == false)
            {
                return BadRequest(new Response_ApiUserRegisterDtos()
                {
                    isSuccess = false,
                    Message = userDto.Message
                });
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userDto.apiUser);
            var callUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new {userId = userDto.apiUser.Id, code = code});

        }
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<ActionResult<Response_ApiUserConfirmEmail>> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest(new Response_ApiUserConfirmEmail()
                {
                    isSuccess = false,
                    Message = "wrong email link address"
                });
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new Response_ApiUserConfirmEmail()
                {
                    isSuccess = false,
                    Message = "wrong user id provided"
                });
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status = result.Succeeded ? "Thanks for your Email confirimation" : "Email not confirmed";

            return Ok(new Response_ApiUserConfirmEmail()
            {
                isSuccess = true,
                Message = status
            });
        }
    }
}