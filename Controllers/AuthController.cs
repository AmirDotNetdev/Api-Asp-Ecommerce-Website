using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestApi.DTOs.Request;
using TestApi.DTOs.Response;
using TestApi.Models.AuthModels;
using TestApi.Repositories.AuthRepository;
using TestApi.Templates;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository authRepository, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _authRepo = authRepository;
            _userManager = userManager;
            _configuration = configuration;
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
            var callbackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new {userId = userDto.apiUser.Id, code = code});
            var body = ConfirmEmailTemplate.EmailLinkTemplate(callbackUrl);
            return Ok (new Response_ApiUserConfirmEmail()
            {
                isSuccess = true,
                Message = "Compelete"
            });
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
        [HttpPost]
        [Route("RegisterAdmin/{secretKey}")]
        public async Task<ActionResult<Response_ApiUserRegisterDtos>> RegisterAdmin([FromRoute] int secretKey, [FromBody] Request_ApiUserRegisterDto userDto)
        {
            var user = await _authRepo.RegisterAdmin(userDto, secretKey);
            if(user.isSuccess)
            {
                return Ok(new Response_ApiUserRegisterDtos()
                {
                    isSuccess = true,
                    Message = new List<string> ()
                    {
                        "User Successfuly registerd",
                        "Admin no need to verify email"
                    }
                });
            }
            return BadRequest(new Response_ApiUserRegisterDtos()
            {
                isSuccess = false,
                Message = user.Message
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Response_LoginDto>> Login([FromBody] Request_LoginApi login)
        {
            var authResult = await _authRepo.Login(login);
            if(authResult.Result == false)
            {
                return BadRequest(authResult);
            }
            return Ok(authResult);
        }

        // private function

        private async Task<bool> SendEmail (string body, string email , string subject = "EmailConfirimation")
        {
            return true;
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),

            }.Union(userClaims).Union(roleClaims);
            //Generate Token
            var token = new JwtSecurityToken(
                issuer : _configuration["JwtSettings:Issuer"],
                audience : _configuration["JwtSettings:Audience"],
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials : credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
            
                
            
        }
    }
}