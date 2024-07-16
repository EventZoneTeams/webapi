using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.EventDonationDTOs;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventDonationController : ControllerBase
    {
        private readonly IEventDonationService _eventDonationService;

        public EventDonationController(IEventDonationService eventCampaignService)
        {
            _eventDonationService = eventCampaignService;
        }

        [HttpPost("event-donations")]
        public async Task<IActionResult> CreateAsync(EventDonationCreateDTO model)
        {
            try
            {
                var result = await _eventDonationService.AddDonationToCampaign(model);
                if (result.Status == false)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("campaigns/{id}/event-donations")]
        public async Task<IActionResult> CreateAsync(int id)
        {
            try
            {
                var result = await _eventDonationService.GetAllDonationOfCampaign(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}