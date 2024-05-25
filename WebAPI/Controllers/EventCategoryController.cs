using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Repositories.Extensions;
using Services.BusinessModels.EventCategoryModels;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/event-categories")]
    [ApiController]
    public class EventCategoryController : Controller
    {
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IImageService _imageService;

        public EventCategoryController(IEventCategoryService eventCategoryService, IImageService imageService)
        {
            _eventCategoryService = eventCategoryService;
            _imageService = imageService;
        }



        /// <summary>
        /// Get all event categories
        /// </summary>
        /// <returns>A list of event categories</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /event-categories
        ///
        /// </remarks>
        /// <response code="200">Returns list of event categories</response>
        /// <response code="400">If the item is null</response>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesOfEventAsync([FromQuery] string? SearchTerm, [FromQuery] EventCategoryOrderBy eventCategoryOrderBy)
        {
            try
            {
                var data = await _eventCategoryService.GetEventCategories(new CategoryParam
                {
                    SearchTerm = SearchTerm,
                    OrderBy = eventCategoryOrderBy,
                });

                if (data == null)
                {
                    return BadRequest(ApiResult<object>.Error(null, "Get Categories Of Event Failed!"));
                }
                return Ok(ApiResult<List<EventCategoryModel>>.Succeed(data, "Get Categories Of Event Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get a category of event by id
        /// </summary>
        /// <param name="id">The id of the event category</param>
        /// <returns>The event category with the specified id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /event-categories/1
        ///     
        /// Sample response:
        ///     {
        ///        "id": 1,
        ///        "title": "Âm Nhạc",
        ///        "imageUrl": "google.com"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the event category</response>
        /// <response code="404">If the event category is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryEventByIdAsync(int id)
        {
            try
            {
                var data = await _eventCategoryService.GetEventCategoryById(id);
                if (data == null)
                {
                    return NotFound(ApiResult<EventCategoryModel>.Error(null, "Event Not Found!"));
                }
                return Ok(ApiResult<EventCategoryModel>.Succeed(data,
                    "Get Event Details Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Create a new event category
        /// </summary>
        /// <param name="data">The data for the new event category, including title and image</param>
        /// <returns>The created event category</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /event-categories
        ///     {
        ///         "title": "Âm Nhạc",
        ///         "image": [binary image data]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the created event category</response>
        /// <response code="400">If the model state is invalid or an error occurs during creation</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventCategory([FromForm] CreateEventCategoryModel data)
        {
            try
            {
                string imageUrl = null;
                if (data.Image != null)
                {
                    imageUrl = await _imageService.UploadImageAsync(data.Image, "event-category");
                }

                var eventCategory = new EventCategoryModel
                {
                    Title = data.Title,
                    ImageUrl = imageUrl
                };

                var result = await _eventCategoryService.CreateEventCategory(eventCategory);

                return Ok(ApiResult<EventCategoryModel>.Succeed(result,
                        "Create Event Category Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Update an existing event category
        /// </summary>
        /// <param name="id">The id of the event category to update</param>
        /// <param name="data">The updated data for the event category, including title and image</param>
        /// <returns>The updated event category</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /event-categories/1
        ///     {
        ///         "title": "Updated Âm Nhạc",
        ///         "image": [binary image data]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated event category</response>
        /// <response code="404">If the event category is not found</response>
        /// <response code="400">If the model state is invalid or an error occurs during update</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventCategory(int id, [FromForm] UpdateEventCategoryModel data)
        {
            try
            {
                string imageUrl = null;
                if (data.Image != null)
                {
                    imageUrl = await _imageService.UploadImageAsync(data.Image, "event-category");
                }

                var eventCategory = new EventCategoryModel
                {
                    Id = id,
                    Title = data.Title,
                    ImageUrl = imageUrl
                };

                var result = await _eventCategoryService.UpdateEventCategory(id, eventCategory);

                if (result == null)
                {
                    return NotFound(ApiResult<EventCategoryModel>.Error(null, "Event Category Not Found!"));
                }

                return Ok(ApiResult<EventCategoryModel>.Succeed(result, "Update Event Category Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Delete an event category
        /// </summary>
        /// <param name="id">The id of the event category to delete</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /event-categories/1
        ///
        /// </remarks>
        /// <response code="204">If the event category was successfully deleted</response>
        /// <response code="404">If the event category is not found</response>
        /// <response code="400">If an error occurs during deletion</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventCategory(int id)
        {
            try
            {
                var result = await _eventCategoryService.DeleteEventCategory(id);

                if (result == null)
                {
                    return NotFound(ApiResult<object>.Error(null, "Event Category Not Found!"));
                }

                return Ok(ApiResult<EventCategoryModel>.Succeed(result, "Delete Event Category Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

    }
}
