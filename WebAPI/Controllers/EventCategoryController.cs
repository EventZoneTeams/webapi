using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Repositories.Commons.Payload.RequestModels;
using Services.BusinessModels.EventCategoryModels;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/event-categories")]
    [ApiController]
    public class EventCategoryController : Controller
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesOfEventAsync([FromQuery] EventParams eventParams,
               OriganizationStatusEnums? origanizationStatusEnums,
               EventStatusEnums? eventStatusEnums
            )
        {
            try
            {
                var data = await _eventCategoryService.GetEventCategories();
                if (data == null)
                {
                    return BadRequest("Get Categories Of Event Failed!");
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
        /// <param name="id"></param>
        /// <returns>A Category With Id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /event-categories/1
        ///     return
        ///     {
        ///        "id": 1,
        ///        "title": "Âm Nhạc",
        ///        "imageUrk": "google.com"
        ///     }
        ///
        /// </remarks> 
        /// <response code="200">Returns a category</response>
        /// <response code="404">Category Not Found</response>
        [HttpGet("{id}")]
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
                return BadRequest(ex.Message);
            }
        }

    }
}
