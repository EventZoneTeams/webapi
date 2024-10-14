using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.DTOs.UserDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventStaffController : Controller
    {
        private readonly IEventStaffService _eventStaffService;

        public EventStaffController(IEventStaffService eventStaffService)
        {
            _eventStaffService = eventStaffService;
        }

        [HttpGet("get-staff-events")]
        public async Task<IActionResult> GetEventByCurrentStaff()
        {
            try
            {
                var events = await _eventStaffService.GetEventByCurrentStaff();
                if (events == null || events.Count == 0)
                {
                    return NotFound(ApiResult<List<EventResponseDTO>>.Error([], "No event found for current staff"));
                }

                return Ok(ApiResult<List<EventResponseDTO>>.Succeed(events, "Events retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("get-user-list-order-ticket")]
        public async Task<IActionResult> GetUserListOrderAndTicket([FromQuery] Guid eventId)
        {
            try
            {
                var result = await _eventStaffService.GetUserListOrderAndTicket(eventId);
                return Ok(ApiResult<object>.Succeed(result, "User list order and ticket retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventStaff(Guid eventId)
        {
            try
            {
                var eventStaff = await _eventStaffService.GetEventStaffAsync(eventId);
                if (eventStaff == null || eventStaff.Count == 0)
                {
                    return NotFound(ApiResult<List<UserDetailsModel>>.Error([], $"No staff found for event ID: {eventId}"));
                }

                return Ok(ApiResult<List<UserDetailsModel>>.Succeed(eventStaff, "Event staff retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("{eventId}")]
        public async Task<IActionResult> AddStaffToEvent(Guid eventId, [FromBody] AddStaffRequest addStaffRequest)
        {
            try
            {
                var eventStaff = await _eventStaffService.AddStaffIntoEvent(eventId, addStaffRequest.UserId, addStaffRequest.Note);
                return Ok(ApiResult<EventStaff>.Succeed(eventStaff, "Staff added to event successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> RemoveStaffFromEvent(Guid eventId, [FromQuery] Guid userId)
        {
            try
            {
                var eventStaff = await _eventStaffService.RemoveStaffFromEvent(eventId, userId);
                return Ok(ApiResult<EventStaff>.Succeed(eventStaff, "Staff removed from event successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }

    public class AddStaffRequest
    {
        public Guid UserId { get; set; }
        public string Note { get; set; }
    }
}
