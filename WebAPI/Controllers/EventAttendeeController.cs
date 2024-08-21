using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Repositories.Commons;
using EventZone.Services.Interface;
using EventZone.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventAttendeeController : ControllerBase
    {
        private readonly IAttendeeService _attendeeService;

        public EventAttendeeController(IAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }

        [HttpPost("event-attendees")]
        public async Task<ActionResult> CreateEventBookedTicket(BookedTicketDTO bookedTicketDTO)
        {
            try
            {
                var result = await _attendeeService.BookANewTicketForEvent(bookedTicketDTO);
                return Ok(ApiResult<List<BookedTicketDetailDTO>>.Succeed(result, "Create ticket successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get list ticket by eventId
        /// </summary>
        /// <response code="200">Returns a list of ticket</response>
        /// <response code="400">Error during reading data</response>
        /// <response code="404">Event Id is not exist</response>
        [HttpGet("event-attendees")]
        public async Task<ActionResult> GetAllEventBookedTickets()
        {
            try
            {
                var tickets = await _attendeeService.GetAllBookedTicket();
                if (tickets == null)
                {
                    return NotFound(ApiResult<EventTicketDetailDTO>.Error(null, "There are no existed event i"));
                }
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}