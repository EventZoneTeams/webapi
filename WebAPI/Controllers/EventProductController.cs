using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/v1/eventproducts")]
    [ApiController]
    public class EventProductController : Controller
    {

        private readonly ProductRepository _productRepository;

        public EventProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var data = await _productRepository.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
