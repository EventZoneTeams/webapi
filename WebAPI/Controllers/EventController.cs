using AutoMapper;
using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Extensions;
using EventZone.Repositories.Helper;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IEventPackageService _eventPackageService;

        public EventController(IEventService eventService, IMapper mapper, IEventPackageService eventPackageService)
        {
            _eventService = eventService;
            _mapper = mapper;
            _eventPackageService = eventPackageService;
        }

        /// <summary>
        /// Get list events
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
        public async Task<IActionResult> GetEventsAsync([FromQuery] EventParams eventParams)
        {
            try
            {
                var events = await _eventService.GetEvent(eventParams);

                Response.AddPaginationHeader(events.MetaData);

                var eventReponseDTOs = _mapper.Map<List<EventResponseDTO>>(events);

                return Ok(ApiResult<List<EventResponseDTO>>.Succeed(eventReponseDTOs, "Get list events successfully"));
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
        public async Task<IActionResult> GetEventByIdAsync(Guid id)
        {
            try
            {
                var eventModel = await _eventService.GetEventById(id);
                var data = await _eventPackageService.GetAllPackageOfEvent(id);
                return Ok(ApiResult<EventResponseDTO>.Succeed(eventModel, "Get Event Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
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
        ///         
        ///      }
        ///
        ///  Note:
        ///
        ///     status:
        ///         PENDING,
        ///         REJECTED,
        ///         ISFEEDBACK,
        ///         APRROVED,
        ///         DONATING,
        ///         SUCCESSFUL,
        ///         FAILED
        /// </remarks>
        /// <response code="200">Returns a event</response>
        /// <response code="400">Requied field is null</response>
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventCreateDTO createEventModel)
        {
            try
            {
                var eventModel = await _eventService.CreateEvent(createEventModel);
                return Ok(ApiResult<EventResponseDTO>.Succeed(eventModel, "Create Event Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Update an existing event
        /// </summary>
        /// <param name="id">The ID of the event to update</param>
        /// <param name="updateEventModel">The updated event data</param>
        /// <returns>The updated event</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /events/1
        ///     {
        ///         "name": "Charity Fundraiser for Children's Education Updated",
        ///         "description": "A charity event to raise funds for underprivileged children's education and school supplies Updated.",
        ///         "thumbnailUrl": "https://eventzoneblob.blob.core.windows.net/images/event-thumbnails/419976033_907448214020009_8314703696454832411_n.png",
        ///         "donationStartDate": "2024-05-15T09:00:00.000Z",
        ///         "donationEndDate": "2024-05-30T18:00:00.000Z",
        ///         "eventStartDate": "2024-05-25T10:00:00.000Z",
        ///         "eventEndDate": "2024-05-25T18:00:00.000Z",
        ///         "location": "Central Park, New York City",
        ///         "userId": 1,
        ///         "eventCategoryId": 1,
        ///         "university": "New York University",
        ///         "status": "PENDING",
        ///         "origanizationStatus": "PREPARING",
        ///         "isDonation": true,
        ///         "totalCost": 25000
        ///      }
        ///
        ///  Note:
        ///
        ///     origanizationStatus:
        ///         PREPARING,
        ///         ACCOMPLISHED,
        ///         DELAYED
        ///         CANCELED
        ///
        ///     status:
        ///         PENDING,
        ///         REJECTED,
        ///         ISFEEDBACK,
        ///         APRROVED,
        ///         DONATING,
        ///         SUCCESSFUL,
        ///         FAILED
        /// </remarks>
        /// <response code="200">Returns the updated event</response>
        /// <response code="400">If the event or the update data is invalid</response>
        /// <response code="404">If the event is not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] EventCreateDTO updateEventModel)
        {
            try
            {

                var format = new EventDTO
                {
                    Name = updateEventModel.Name,
                    Description = updateEventModel.Description,
                    ThumbnailUrl = updateEventModel.ThumbnailUrl,
                    EventStartDate = updateEventModel.EventStartDate,
                    EventEndDate = updateEventModel.EventEndDate,
                    Note = updateEventModel.Note,
                    EventCategoryId = updateEventModel.EventCategoryId,
                };
                var updatedEvent = await _eventService.UpdateEvent(id, format);
                return Ok(ApiResult<EventResponseDTO>.Succeed(updatedEvent, "Event updated successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Delete an existing event
        /// </summary>
        /// <param name="id">The ID of the event to delete</param>
        /// <returns>The deleted event</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /events/1
        ///
        /// </remarks>
        /// <response code="200">Returns the deleted event</response>
        /// <response code="400">If the event is invalid</response>
        /// <response code="404">If the event is not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventAsync(Guid id)
        {
            try
            {
                var deletedEvent = await _eventService.DeleteEvent(id);
                return Ok(ApiResult<EventResponseDTO>.Succeed(deletedEvent, "Event deleted successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}