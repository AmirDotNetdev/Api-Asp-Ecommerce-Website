using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;
using TestApi.DTOs.Request;
using TestApi.Models.ProductModels;
using TestApi.Repositories.MainCategoryRepository;
using TestApi.Repositories.ValidationsRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainCategoryController : ControllerBase
    {
  
        private readonly IMainCategoryRepository _mainCategoryRepo;
        private readonly IValidationsRepository _validationsRepository;
        public MainCategoryController(IMainCategoryRepository mainCategoryRepository, IValidationsRepository validationsRepository)
        {
            _mainCategoryRepo = mainCategoryRepository;
            _validationsRepository = validationsRepository;

        }

        [HttpGet]
        [Route("GetAllMainCategory")]
        public async Task<ActionResult<Response_MainCategory>> GetAllMainCategory ()
        {
            var mainCategories = await _mainCategoryRepo.GetMainCategory();
            if(!mainCategories.isSuccess)
            {
                return BadRequest(mainCategories);
            }
            return Ok(mainCategories);

        }
        [HttpGet]
        [Route("GetMainCategoryById/{mainCategoryId}")]
        public async Task<ActionResult<Response_MainCategory>> GetMainCategoryById ([FromRoute] int mainCategoryId)
        {
            var mainCategory = await _mainCategoryRepo.GetMainCategoryById(mainCategoryId);
            if(!mainCategory.isSuccess)
            {
                return BadRequest(mainCategory);
            }
            return Ok(mainCategory);
        }

        [HttpPost]
        [Route("AddMainCategory")]
        public async Task<ActionResult<Response_MainCategory>> AddMainCategory([FromBody] Request_MainCategory request)
        {
            var isExist = await CheckMainCategory(request);
            if(isExist)
            {
                return BadRequest("The MainCategory is already exist");
            }
            
            var response = await _mainCategoryRepo.AddMainCategory(request);
            if(!response.isSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
            
        }

        [HttpPut]
        [Route("UpdateMainCategory/{id}")]
        public async Task<ActionResult<Response_MainCategory>> UpdateMainCategory([FromRoute] int id, [FromBody] Request_MainCategory request)
        {
            var isExixt = await CheckMainCategory(request);
            if(isExixt)
            {
                return BadRequest("This MainCategory is already exist");
            }
            var mainCategoryUpdated = await _mainCategoryRepo.UpdateMainCategory(id, request);
            if(!mainCategoryUpdated.isSuccess)
            {
                return BadRequest(mainCategoryUpdated);
            }
            return Ok(mainCategoryUpdated);
            
        }

        [HttpDelete]
        [Route("DeleteMainCategory/{id}")]
        public async Task<ActionResult<Response_MainCategory>> DeleteMainCategory([FromRoute] int id)
        {
            var response = await _mainCategoryRepo.DeleteMainCategory(id);
            if(!response.isSuccess)
            {
                return BadRequest(response);
            }
            return response;
        }

        //private validations
        private async Task<bool> CheckMainCategory(Request_MainCategory request)
        {
            var isValid = await _validationsRepository.ValidateMainCategory(request.Name);
            return isValid;
        }
    }
}