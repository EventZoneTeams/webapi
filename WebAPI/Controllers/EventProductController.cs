using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventProductsModel;
using Services.DTO.TestModels;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/v1/event-products")]
    [ApiController]
    public class EventProductController : Controller
    {
        private readonly IEventProductService _eventProductService;
        private readonly IImageService _imageService;

        public EventProductController(IEventProductService eventProductService, IImageService imageService)
        {
            _eventProductService = eventProductService;
            _imageService = imageService;
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

        [HttpGet("event/{id}")]
        public async Task<IActionResult> GetAllAsync(int id)
        {
            try
            {
                var data = await _eventProductService.GetAllProductsByEventAsync(id);
                if (data == null)
                {
                    return BadRequest(new {status=false , msg="Event is not existed"});

                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] EventProductCreateModel model)
        {
            try
            {
                if (model.fileImages == null || model.fileImages.Count == 0)
                {
                    return BadRequest("No files were provided.");
                }
                var uploadedFileUrls = await _imageService.UploadMultipleImagesAsync(model.fileImages, "test-image-multiple");

                if (uploadedFileUrls.Count == 0)
                {
                    return BadRequest("Failed to upload any files.");
                }

                var result = await _eventProductService.CreateEventProductAsync(model, uploadedFileUrls);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAsync([FromBody] List<EventProductCreateModel> models)
        //{
        //    try
        //    {
        //        var result = await _eventProductService.CreateEventProductAsync(models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] EventProductUpdateModel model)
        {
            try
            {
                var result = await _eventProductService.UpdateEventProductAsync(id, model);
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