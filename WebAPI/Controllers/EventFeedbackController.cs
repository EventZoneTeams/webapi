using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventFeedbackModel;
using Services.DTO.EventPackageModels;
using Services.Interface;
using Services.Services;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/event-feedbacks")]
    [ApiController]
    public class EventFeedbackController : ControllerBase
    {
        private readonly IEventFeedbackService _eventFeedbackService;
        private readonly IImageService _imageService;

        public EventFeedbackController(IEventFeedbackService eventFeedbackService, IImageService imageService)
        {
            _eventFeedbackService = eventFeedbackService;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery, Required] FeedbackTypeEnums type, [FromBody] CreateFeedbackModel input)
        {
            try
            {
                var result = await _eventFeedbackService.CreateFeedBackForEvent(input, type);

                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackageAsync()
        {
            try
            {
                var data = await _eventFeedbackService.GettAllFeedbacksAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetAllPackageAsync(int id)
        {
            try
            {
                var data = await _eventFeedbackService.GettAllFeedbacksByEventIdAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] List<int> feedbackIds)
        {
            try
            {
                var result = await _eventFeedbackService.DeleteFeedbacksAsync(feedbackIds);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}