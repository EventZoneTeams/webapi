using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Extensions;
using EventZone.Repositories.Helper;
using EventZone.Services.Interface;
using EventZone.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult> GetEventTickets(Guid id)
        {
            try
            {
                var tickets = await _eventTicketService.GetAllTicketsByEventIdAsync(id);
                if (tickets == null)
                {
                    return NotFound(ApiResult<EventTicketDetailDTO>.Error(null, "There are no existed event id: " + id));
                }
                return Ok(ApiResult<List<EventTicketDetailDTO>>.Succeed(tickets, "get list successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// delete a ticket by their IDs
        /// </summary>
        /// <response code="200">Returns the removed ticket </response>
        [HttpDelete("event-tickets/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _eventTicketService.DeleteEventTicketByIdAsync(id);
                if (result == null)
                {
                    return NotFound(ApiResult<EventTicketDetailDTO>.Error(null, "There are no existed product id: " + id));
                }
                return Ok(ApiResult<EventTicketDetailDTO>.Succeed(result.Data, "ticket deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get a ticket by id
        /// </summary>
        /// <response code="200">Returns a ticket</response>
        [HttpGet("event-tickets/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _eventTicketService.GetTicketById(id);
                if (result != null)
                {
                    return Ok(ApiResult<EventTicketDetailDTO>.Succeed(result.Data, "get ticket successfully"));
                }
                return NotFound(ApiResult<EventTicketDetailDTO>.Error(null, "There are no existed product id: " + id));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// update an event ticket by its id
        /// </summary>
        /// <response code="200">Returns the updated ticket</response>
        [HttpPut("event-tickets/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] EventTicketUpdateDTO model)
        {
            try
            {
                var result = await _eventTicketService.UpdateEventTicketAsync(id, model);
                if (result == null)
                {
                    return NotFound(ApiResult<EventTicketDetailDTO>.Error(null, "There are no existed product id: " + id));
                }

                return Ok(ApiResult<EventTicketDetailDTO>.Succeed(result.Data, "update ticket successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}