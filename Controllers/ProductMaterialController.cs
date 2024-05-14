using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTOs.ProductDtos.Request;
using TestApi.DTOs.ProductDtos.Response;
using TestApi.Repositories.MaterialRepository;
using TestApi.Repositories.ValidationsRepository;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductMaterialController : ControllerBase
    {
        private readonly IValidationsRepository _validation;
        private readonly IMaterialRepository _materialRepo;
        public ProductMaterialController(IMaterialRepository materialRepository, IValidationsRepository validations)
        {
            _validation = validations;
            _materialRepo = materialRepository;
        }
        [HttpGet]
        [Route("ProductMaterials")]
        public async Task<ActionResult<Response_ProductMaterial>> GetAllProudctMaterials ()
        {
            var productMaterial = await _materialRepo.GetAllProductMaterial();
            if(productMaterial.isSuccess ==false)
            {
                return BadRequest(productMaterial);
            }
            return Ok(productMaterial);
        }
        [HttpGet]
        [Route("ProductMaterial/{materialId}")]
        public async Task<ActionResult<Response_ProductMaterial>> GetProductMaterialById([FromRoute] int materialId)
        {
            var productMaterialResponse = await _materialRepo.GetProductMaterialById(materialId);
            if (productMaterialResponse.isSuccess == false)
            return BadRequest(productMaterialResponse);

            return Ok(productMaterialResponse);
        }

        [HttpPost]
        [Route("AddProductMaterial")]
        public async Task<ActionResult<Response_ProductMaterial>> AddProductMaterial([FromBody] Request_ProductMaterial request)
        {
            var result = await CheckProdcutMaterialName(request);
            if(result)
            {
                return BadRequest("This material is already exist");
            }
            var productAdded = await _materialRepo.AddProductMaterial(request);
            if(productAdded == null)
            {
                return new Response_ProductMaterial
                {
                    Message = "No product added"
                };
            }
            
               

            if(!productAdded.isSuccess)
            {
                return BadRequest(productAdded);
            }
             return CreatedAtAction(nameof(GetProductMaterialById), new { materialId = productAdded.Materials[0].Id }, productAdded);

           
        }

        [HttpPut]
        [Route("UpdateMaterial/{materialId}")]
        public async Task<ActionResult<Response_ProductMaterial>> UpdateProductMaterial([FromRoute] int materialId, [FromBody] Request_ProductMaterial request)
        {
            var resutl = await CheckProdcutMaterialName(request);
            if(resutl == true)
            {
                return BadRequest("This material is already exist");
            }
            var updatedProductMaterial = await _materialRepo.UpdateProductMaterial(materialId, request);
            if(updatedProductMaterial.isSuccess == false)
            {
                return BadRequest(updatedProductMaterial);
            }
            return Ok(updatedProductMaterial);

        }   

        [HttpDelete]
        [Route("DeleteProductMaterial/{materialId}")]
        public async Task<ActionResult<Response_ProductMaterial>> DeleteProductMaterial([FromRoute] int materialId)
        {
            
            var response = await _materialRepo.DeleteProductMaterial(materialId);
            if(response.isSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //private functions

        private async Task<bool> CheckProdcutMaterialName(Request_ProductMaterial request)
        {
            var response = await _validation.ValidateMaterial(request.Name);
            return response;
        }
    }
}