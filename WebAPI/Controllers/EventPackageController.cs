using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventPackageModels;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventPackageController : ControllerBase
    {
        private readonly IEventPackageService _eventPackageService;
        private readonly IImageService _imageService;

        public EventPackageController(IEventPackageService eventPackageService, IImageService imageService)
        {
            _eventPackageService = eventPackageService;
            _imageService = imageService;
        }

        /// <summary>
        /// Create an package with event product included
        /// </summary>
        /// <returns>the added event package</returns>
        ///    /// <remarks>
        /// Sample request:
        ///
        ///     POST /event-packages
        ///     {
        ///         "eventId":1,
        ///         "description": "Nice package for student with free purchase",
        ///         "products":[{"productid": 1,"quantity": 10}, {"productid": 1,"quantity": 10}],
        ///         "thumbnailUrl": "any input"
        ///      }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("event-packages")]
        public async Task<IActionResult> CreateAsync([FromQuery] int eventId, [FromForm] CreatePackageRequest package)
        {
            try
            {
                string url = "";
                if (package.Thumbnail != null)
                {
                    url = await _imageService.UploadImageAsync(package.Thumbnail, "package-thumbnails");
                }
                var result = await _eventPackageService.CreatePackageWithProducts(eventId, url, package);

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

        /// <summary>
        /// Get list products with event packages included
        /// </summary>
        /// <returns>A list of products</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("products/event-packages")]
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

        /// <summary>
        /// Get list all event packages
        /// </summary>
        /// <returns>A list of event packages</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("event-packages")]
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

        /// <summary>
        /// Get list all event packages of an event
        /// </summary>
        /// <returns>A list of event packages</returns>
        [HttpGet("{eventid}/event-packages")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPackagesInEventAsync(int eventid)
        {
            try
            {
                var data = await _eventPackageService.GetAllPackageOfEvent(eventid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove a list of event packages by their id
        /// </summary>
        /// <returns>A list of event packages removed</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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