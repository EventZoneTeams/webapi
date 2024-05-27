using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.BusinessModels.EventPackageModels;
using Services.BusinessModels.EventProductsModel;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/event-packages")]
    [ApiController]
    public class EventPackageController : ControllerBase
    {
        private readonly IEventPackageService _eventPackageService;

        public EventPackageController(IEventPackageService eventPackageService)
        {
            _eventPackageService = eventPackageService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery]int eventId, CreatePackageRequest package)
        {
            try
            {
                var result = await _eventPackageService.CreatePackageWithProducts(eventId, package.Description ,package.Products);
                if (result.Status)
                {
                return Ok(result);

                }

                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductsInPackagesWithProduct_Package()
        {
            try
            {
                var data = await _eventPackageService.GetProductsInPackagesWithProduct_Package();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPackageAsync()
        {
            try
            {
                var data = await _eventPackageService.GetAllWithProducts();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetAllPackagesInEventAsync(int id)
        {
            try
            {
                var data = await _eventPackageService.GetAllPackageOfEvent(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] List<int> packageIds)
        {
            try
            {
                var result = await _eventPackageService.DeleteEventPackagesAsync(packageIds);
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
