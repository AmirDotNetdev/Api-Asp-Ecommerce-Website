using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
