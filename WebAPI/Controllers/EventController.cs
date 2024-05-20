using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Repositories.Commons.Payload.Requests;
using Repositories.DTO;
using Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>A list of events</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /events
        ///
        /// </remarks>
        /// <response code="200">Returns list of events</response>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsAsync([FromQuery] EventParams eventParams,
               OriganizationStatusEnums? origanizationStatusEnums,
               EventStatusEnums? eventStatusEnums
            )
        {
            try
            {
                var data = await _eventService.GetEventsAsync();
                if (data == null)
                {
                    return BadRequest("Get List Failed!");
                }
                return Ok(ApiResult<List<EventModel>>.Succeed(data, "Get Event Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Event With Id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /events/1
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks> 
        /// <response code="200">Returns a event</response>
        /// <response code="404">Event Not Found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventByIdAsync(int id)
        {
            try
            {
                var data = await _eventService.GetEventByIdAsync(id);
                if (data == null)
                {
                    return NotFound(ApiResult<EventModel>.Error(null, "Event Not Found!"));
                }
                return Ok(ApiResult<EventModel>.Succeed(data,
                    "Get Event Details Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a event
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A New Event</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /events
        ///     {
        ///        "id": 1,
        ///        "name": "Coding Battle",
        ///        "description": "Coding Battle for student",
        ///        "isDonation": true,
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks> 
        /// <response code="200">Returns a event</response>
        /// <response code="400">Cai j do bi null</response>
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
