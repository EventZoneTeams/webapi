using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
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
        public async Task<ActionResult> CreateEventBookedTicket(BookedTicketRequestDTO bookedTicketDTO)
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
        /// Get list booked ticket
        /// </summary>
        /// <response code="200">Returns a list of ticket</response>
        /// <response code="400">Error during reading data</response>
        /// <response code="404">Event Id is not exist</response>
        [HttpGet("event-attendees")]
        public async Task<ActionResult> GetAllEventBookedTickets()
        {
            try
            {
                var tickets = await _attendeeService.GetAllBookedTickets();

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get list booked ticket by eventId
        /// </summary>
        /// <response code="200">Returns a list of ticket</response>
        /// <response code="400">Error during reading data</response>
        /// <response code="404">Event Id is not exist</response>
        [HttpGet("event-orders/{id}/event-attendees")]
        public async Task<ActionResult> GetEventBookedTicketsByOrder(Guid id)
        {
            try
            {
                var tickets = await _attendeeService.GetAllBookedTicketByOrderID(id);

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// update an booked ticket by its id
        /// </summary>
        /// <response code="200">Returns a booked ticket</response>
        [HttpPut("booked-tickets/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] BookedTicketUpdateDTO model)
        {
            try
            {
                var result = await _attendeeService.UpdateBookedAsync(id, model);
                if (result == null)
                {
                    return NotFound(ApiResult<BookedTicketDetailDTO>.Error(null, "There are no existed booked id: " + id));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// update an booked ticket by its id
        /// </summary>
        /// <response code="200">Returns a booked ticket</response>
        [HttpPut("check-in-tickets/{id}")]
        public async Task<IActionResult> CheckinAsync([FromRoute] Guid id)
        {
            try
            {
                var result = await _attendeeService.CheckinBookedAsync(id);
                if (result == null)
                {
                    return NotFound(ApiResult<BookedTicketDetailDTO>.Error(null, "There are no existed booked id: " + id));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}