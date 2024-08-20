using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.ProductModels;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
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

        /// <summary>
        /// Get list existing products
        /// </summary>
        /// <response code="200">Returns a list of products</response>
        [HttpGet("event-products")]
        public async Task<IActionResult> GetAccountByFilters([FromQuery] PaginationParameter paginationParameter, [FromQuery] ProductFilterModel productFilterModel)
        {
            try
            {
                var result = await _eventProductService.GetProductsByFiltersAsync(paginationParameter, productFilterModel);
                if (result == null)
                {
                    return NotFound("No accounts found with the specified filters.");
                }
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(ApiResult<Pagination<EventProductDetailDTO>>.Succeed(result, "Get list products successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <response code="200">Returns a product</response>
        [HttpGet("event-products/{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            try
            {
                var result = await _eventProductService.GetProductById(id);
                if (result == null)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get list existing products of an event
        /// </summary>
        /// <response code="200">Returns a list of products</response>
        [HttpGet("{eventid}/event-products")]
        public async Task<IActionResult> GetAllAsync(Guid eventid)
        {
            try
            {
                var data = await _eventProductService.GetAllProductsByEventAsync(eventid);
                if (data == null)
                {
                    return BadRequest(new { status = false, msg = "Event is not existed" });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Create an product and can add many images including
        /// </summary>
        /// <returns>the added event product</returns>
        ///    /// <remarks>
        /// Sample request:
        ///
        ///     POST /event-products
        ///     {
        ///         "EventId":1,
        ///         "Name": First package,
        ///         "Description": "Nice package for student with free purchase",
        ///         "Price":1000,
        ///         "QuantityInStock":10
        ///         "fileImages":[{"input1"}, {"input2"}]
        ///      }
        /// </remarks>
        /// <response code="200">Returns a list of products</response>

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("event-products")]
        public async Task<IActionResult> CreateAsync([FromForm] EventProductCreateDTO model)
        {
            try
            {
                if (model.fileImages == null || model.fileImages.Count == 0)
                {
                    return BadRequest("No files were provided.");
                }
                else
                {
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
                return BadRequest(ApiResult<object>.Fail(ex));
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

        /// <summary>
        /// update an event product by its id
        /// </summary>
        /// <response code="200">Returns a product</response>
        [HttpPut("event-products/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] EventProductUpdateDTO model)
        {
            try
            {
                var result = await _eventProductService.UpdateEventProductAsync(id, model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// delete a list of event product by their IDs
        /// </summary>
        /// <response code="200">Returns list of remove products</response>
        [HttpDelete("event-products/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _eventProductService.DeleteEventProductByIdAsync(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}