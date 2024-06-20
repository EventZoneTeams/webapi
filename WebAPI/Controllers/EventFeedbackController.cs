using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventFeedbackModel;
using Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/v1/event-feedbacks")]
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

        /// <summary>
        /// Creates a new feedback for an event.
        /// </summary>
        /// <param name="feedbackOption">The type of feedback (from FeedbackTypeEnums).</param>
        /// <param name="input">The feedback data.</param>
        /// <returns>A result object indicating success or failure, with the created feedback data if successful.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /event-feedbacks
        ///     {
        ///         "event-id":1,
        ///         "content": "Minh là Tiến mình làm 1 gaming vì đam mê này mih lớp 9 ........ mình rất thik làm 1 gamer vì nó rất thú vị và sau 1 năm nghiên cứu mih đã ra dc và từ nay mih sẽ làm gamer :D"
        ///
        ///      }
        ///      feedbackOption:
        ///         ISFEEDBACK,
        ///         APPROVE,
        ///         REJECT
        /// </remarks>
        /// <response code="200">Returns the created feedback data.</response>
        /// <response code="400">Returns an error message if the feedback creation fails.</response>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery, Required] FeedbackTypeEnums feedbackOption, [FromBody] CreateFeedbackModel input)
        {
            try
            {
                var result = await _eventFeedbackService.CreateFeedBackForEvent(input, feedbackOption);

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

        /// <summary>
        /// Gets  event feedbacks.
        /// </summary>
        /// <returns>A list event feedbacks.</returns>
        /// /// <remarks>
        ///
        /// </remarks>
        /// <response code="200">Returns the list of event feedbacks.</response>
        /// <response code="400">Returns an error message if the retrieval fails.</response>
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

        /// <summary>
        /// Gets event feedbacks for a specific event.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <returns>A list of event feedbacks for the specified event.</returns>
        /// <response code="200">Returns the list of event feedbacks for the specified event.</response>
        /// <response code="400">Returns an error message if the retrieval fails.</response>
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

        /// <summary>
        /// Deletes a list of feedbacks by their IDs.
        /// </summary>
        /// <param name="feedbackIds">A list of feedback IDs to delete.</param>
        /// <returns>A result object indicating success or failure, with the list of deleted feedback IDs if successful.</returns>
        /// <response code="200">Returns a list of deleted feedback IDs.</response>
        /// <response code="404">If none of the specified feedbacks are found.</response>
        /// <response code="400">Returns an error message if the deletion fails.</response>
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