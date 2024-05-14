using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;
using TestApi.Repositories.ProductColorRepository;
using TestApi.Repositories.ValidationsRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductColorController : ControllerBase
    {
        private readonly IProductColorRepository _productColorRepo;
        private readonly IValidationsRepository _validationsRepo;
        public ProductColorController(IProductColorRepository productColorRepository, IValidationsRepository validationsRepository)
        {
            _productColorRepo = productColorRepository;
            _validationsRepo = validationsRepository;
        }

        [HttpGet]
        [Route("ProductColors")]
        public async Task<ActionResult<Response_ProductColor>> GetAllProductColor()
        {
            var response = await _productColorRepo.GetAllProductColors();
            if(response.isSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("ProductColor/{id}")]
        public async Task<ActionResult<Response_ProductColor>> GetProductColorById([FromRoute] int id)
        {
            var response = await _productColorRepo.GetProductColorById(id);
            if(!response.isSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("AddProductColor")]
        public async Task<ActionResult<Response_ProductColor>> AddProductColor(Request_ProductColor request)
        {
            var isExist = await CheckProductColor(request);
            if(isExist)
            {
                return BadRequest("this product color is already exist");
            }
            var response = await _productColorRepo.AddProductColor(request);
            if(response.isSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
            
        }

        [HttpPut]
        [Route("UpdateProductColor/{id}")]
        public async Task<ActionResult<Response_ProductColor>> UpdateProductColor([FromRoute] int id, [FromBody] Request_ProductColor request)
        {
            var isExist = await CheckProductColor(request);
            if(isExist)
            {
                return BadRequest("this product color is already exist");
            }
            var response = await _productColorRepo.UpdateProductColor(id, request);
            if(response.isSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteProductColor/{id}")]
        public async Task<ActionResult<Response_ProductColor>> DeleteProductColor([FromRoute] int id)
        {
            
            var response = await _productColorRepo.DeleteProductColor(id);
            if(response.isSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //private functions

        private Task<bool> CheckProductColor(Request_ProductColor request)
        {
            var result = _validationsRepo.ValidateProductColor(request.Name);
            return result;
        }
    }
}