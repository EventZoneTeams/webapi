using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.BusinessModels.EventProductsModel;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/eventpackages")]
    [ApiController]
    public class EventPackageController : ControllerBase
    {
        private readonly IEventPackageService _eventPackageService;

        public EventPackageController(IEventPackageService eventPackageService)
        {
            _eventPackageService = eventPackageService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery]int eventId, List<int> productIds)
        {
            try
            {
                var result = await _eventPackageService.CreatePackageWithProducts(eventId, productIds);
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

        [HttpGet]
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
        [HttpGet("special")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var data = await _eventPackageService.GetAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
