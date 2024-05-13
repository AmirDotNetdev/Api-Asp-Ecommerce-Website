using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestApi.Constants;
using TestApi.Controllers;
using TestApi.Data;
using TestApi.DTOs.Request;
using TestApi.DTOs.Response;
using TestApi.Migrations;
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
        public async Task<Response_LoginDto> Login(Request_LoginApi login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if(user == null)
            {
                return new Response_LoginDto()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Wrong Login Information"
                    }

                };
            }
            if(user.EmailConfirmed == false)
            {
                return new Response_LoginDto()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email not confirimed"
                    }
                };
            }
            var isPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if(!isPassword)
            {
                return new Response_LoginDto()
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Wrong Password"
                    }
                };
            }
            var token = await GenerateToken(user);
            var refreshToken = await GenerateRefreshToken(user, token);
            return new Response_LoginDto()
            {
                RefreshToken = refreshToken,
                Token = token,
                UserId = user.Id,
                Result = true
            };
        }

        public async Task<Response_LoginDto> VerifyAndGenerateTokens(Request_TokenDto tokenDto)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(tokenDto.Token);
            var tokenIssuer = tokenContent.Issuer;
            if(tokenIssuer != _configuration["JwtSettings:Issuer"])
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }
            var tokenAudience = tokenContent.Audiences.ToList();
            if(!tokenAudience.Contains(_configuration["JwtSettings:Audience"]))
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }
            var userName = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null || user.Id != tokenDto.UserId)
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }

            //Refresh token validateion

            var dbRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenDto.RefreshToken);
            if(dbRefreshToken == null)
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }
            if(dbRefreshToken.JwtId != tokenContent.Id)
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }
            if(dbRefreshToken.ExpireDate < DateTime.UtcNow)
            {
                return new Response_LoginDto
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "Token parameters are wrong"
                    }
                };
            }
            var newToken = await GenerateToken(user);
            var newRefreshToken = await GenerateRefreshToken(user , newToken);
            return new Response_LoginDto
            {
                Result = true,
                RefreshToken = newRefreshToken,
                UserId = user.Id,
                Token = newToken,
                Errors = new List<String>()
            };
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

        private async Task<string> GenerateRefreshToken(ApiUser apiUser, string token)
        {
            var existingRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == apiUser.Id);
            if(existingRefreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(existingRefreshToken);
                await _dbContext.SaveChangesAsync();

            }
            var JwtSecurityTokenHandler =new JwtSecurityTokenHandler();
            var tokenContent = JwtSecurityTokenHandler.ReadJwtToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = tokenContent.Id,
                Token = RandomStringGeneration(23),
                AddedDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddMinutes(110),
                UserId = apiUser.Id
            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken.Token;
        }
        private string RandomStringGeneration(int length)
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> LogoutDeleteRefreshToken(string userId)
        {
            var dbRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
            if(dbRefreshToken == null)
            {
                return true;
            }
            _dbContext.RefreshTokens.Remove(dbRefreshToken);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}