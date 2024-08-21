using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Extensions;
using EventZone.Repositories.Helper;
using EventZone.Services.Interface;
using EventZone.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventTicketController : ControllerBase
    {
        private readonly IEventTicketService _eventTicketService;

        public EventTicketController(IEventTicketService eventTicketService)
        {
            _eventTicketService = eventTicketService;
        }

        [HttpPost("event-tickets")]
        /// <summary>
        /// Create a ticket
        /// </summary>
        /// <response code="200">Returns a new ticket detail</response>
        public async Task<ActionResult> CreateEventTicket(EventTicketDTO ticket)
        {
            try
            {
                var result = await _eventTicketService.CreateNewTicketAsync(ticket);
                return Ok(ApiResult<EventTicketDetailDTO>.Succeed(result, "Create ticket successfully"));
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
        [HttpGet("events/{id}/event-tickets")]
        public async Task<ActionResult<List<EventOrderReponseDTO>>> GetEventTicketss(Guid id)
        {
            try
            {
                var tickets = await _eventTicketService.GetAllTicketsByEventIdAsync(id);
                if (tickets == null)
                {
                    return NotFound("This event is not existing in data");
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