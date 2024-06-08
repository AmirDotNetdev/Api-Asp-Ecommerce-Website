using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data.CustomModels;
using TestApi.DTOs.BasePorductDto.CustomModels;
using TestApi.DTOs.BasePorductDto.Request;
using TestApi.Models.ProductModels;
using TestApi.Repositories.BaseProductRepository;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseProductController : ControllerBase
    {
        private readonly IBaseProductRepository _repo;
        public BaseProductController(IBaseProductRepository productRepository)
        {
            _repo = productRepository;
        }

        [HttpGet]
        [Route("GettAll")]
        public async Task<ActionResult<IEnumerable<BaseProduct>>> GetAll()
        {
            var baseProduct = await _repo.GetAllAsync();
            return Ok(baseProduct);
        }

        [HttpGet]
        [Route("GetAllWithFullInfo")]
        public async Task<ActionResult<IEnumerable<Model_BaseProductCustom>>> GetAllWithFullInfo()
        {
            return Ok(await _repo.GetAllWithFullInfoAsync());
        }

        [HttpGet]
        [Route("GetAllPages/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<Model_BaseProductCustom>>> GetAllPages([FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            return Ok(await _repo.GetAllWithFullInfoByPages(pageNumber, pageSize));
        }

        [HttpGet]
        [Route("GetByIdWithNoInfo/{id}")]
        public async Task<ActionResult<IEnumerable<Model_BaseProductCustom>>> GetByIdWithNoInfo([FromRoute] int id)
        {
            var baseProduct = await _repo.GetByIdWithNoInfo(id);
            if (baseProduct.isSuccess == false)
            {
                return NotFound("No product Found with this given Id");
            }
            return Ok(baseProduct);
        }

        [HttpGet]
        [Route("GetByIdFullInfo/{id}")]
        public async Task<ActionResult<IEnumerable<Model_BaseProductCustom>>> GetByIdFullInfo([FromRoute] int id)
        {
            var baseProduct = await _repo.GetByIdWithFullInfo(id);
            if (baseProduct.isSuccess == false)
            {
                return NotFound("No Product Found with this Id ");
            }
            return Ok(baseProduct);
        }

        [HttpPost]
        [Route("AddBaseProduct")]
        public async Task<ActionResult<Model_BaseProductCustom>> AddBaseProduct([FromBody] Request_BaseProduct request)
        {
            var baseProduct = await _repo.AddBaseProduct(request);
            if (baseProduct.isSuccess == false)
            {
                return BadRequest("Cant Add Base Product");
            }
            return CreatedAtAction(nameof(GetByIdWithNoInfo), new { id = baseProduct.baseProducts[0].Id }, baseProduct);
        }

        [HttpPut]
        [Route("UpdateBaseProduct/{id}")]
        public async Task<ActionResult<Model_BaseProductCustom>> UpdateBaseProduct([FromRoute] int id, [FromBody] Request_BaseProduct request)
        {
            var baseProduct = await _repo.UpdateBaseProduct(id, request);
            if (baseProduct.isSuccess == false)
            {
                return BadRequest("Cant Update product");
            }
            return Ok(baseProduct);
        }

        [HttpGet]
        [Route("GetProductSearchSuggestions/{searchText}")]
        public async Task<ActionResult<List<string>>> GetProductSearchSuggestions([FromRoute] string searchText)
        {
            return Ok(await _repo.GetProductSearchSuggestions(searchText));
        }
    }
}
