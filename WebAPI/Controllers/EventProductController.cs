using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;
using Services.BusinessModels.EventProductsModel;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/eventproducts")]
    [ApiController]
    public class EventProductController : Controller
    {

        private readonly IEventProductService _eventProductService;

        public EventProductController(IEventProductService eventProductService)
        {
            _eventProductService = eventProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var data = await _eventProductService.GetAllProductsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EventProductCreateModel model)
        {
            try
            {
                var result = await _eventProductService.CreateEventProductAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int productId, [FromBody] EventProductDetailModel model)
        {
            try
            {
                var result = await _eventProductService.UpdateEventProductAsync( productId,model);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] List<int> productIds)
        {
            try
            {
                var result = await _eventProductService.DeleteEventProductAsync(productIds);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
