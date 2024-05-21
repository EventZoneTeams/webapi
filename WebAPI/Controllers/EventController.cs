using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Repositories.Commons.Payload.Requests;
using Repositories.DTO;
using Services.Interface;
using Services.ViewModels.EventModels;

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
        /// <returns>A New Event</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /events
        ///     {
        ///         "name": "Charity Fundraiser for Children's Education",
        ///         "description": "A charity event to raise funds for underprivileged children's education and school supplies.",
        ///         "donationStartDate": "2024-05-15T09:00:00.000Z",
        ///         "donationEndDate": "2024-05-30T18:00:00.000Z",
        ///         "eventStartDate": "2024-05-25T10:00:00.000Z",
        ///         "eventEndDate": "2024-05-25T18:00:00.000Z",
        ///         "location": "Central Park, New York City",
        ///         "userId": 1,
        ///         "university": "New York University",
        ///         "status": "Upcoming",
        ///         "origanizationStatus": "Approved",
        ///         "isDonation": true,
        ///         "totalCost": 25000
        ///      }
        ///
        ///  Note:
        /// 
        ///     status: 
        ///         PREPARING,
        ///         ACCOMPLISHED,
        ///         DELAYED
        ///         CANCELED
        ///         
        ///     origanizationStatus:
        ///         PENDING,
        ///         REJECTED,
        ///         ISFEEDBACK,
        ///         APRROVED,
        ///         DONATING,
        ///         SUCCESSFUL,
        ///         FAILED
        /// </remarks> 
        /// <response code="200">Returns a event</response>
        /// <response code="400">Cai j do bi null</response>
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync(CreateEventModel createEventModel)
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
