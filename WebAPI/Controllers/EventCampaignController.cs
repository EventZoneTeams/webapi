using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Commons;
using Repositories.Models.EventCampaignModels;
using Repositories.Models.ProductModels;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.EventProductsModel;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventCampaignController : ControllerBase
    {
        private readonly IEventCampaignService _eventCampaignService;

        public EventCampaignController(IEventCampaignService eventCampaignService)
        {
            _eventCampaignService = eventCampaignService;
        }

        [HttpGet("campaigns/{id}")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            try
            {
                var result = await _eventCampaignService.GetACampaignsByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get list existing products
        /// </summary>
        /// <response code="200">Returns a list of products</response>
        [HttpGet("campaigns")]
        public async Task<IActionResult> getCampaignByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] CampaignFilterModel campaignFilterModel)
        {
            try
            {
                var result = await _eventCampaignService.GetCampaignsByFiltersAsync(paginationParameter, campaignFilterModel);
                if (result == null)
                {
                    return NotFound("No accounts found with the specified filters.");
                }
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public async Task<IActionResult> GetAllAsync()
        //{
        //    try
        //    {
        //        var data = await _eventProductService.GetAllProductsAsync();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// Get list existing products of an event
        /// </summary>
        /// <response code="200">Returns a list of products</response>
        [HttpGet("event/{eventid}/campaigns")]
        public async Task<IActionResult> GetAllCampaignByEventAsync(int eventid)
        {
            try
            {
                var data = await _eventCampaignService.GetAllCampaignsByEventAsync(eventid);
                if (data == null)
                {
                    return BadRequest(new { status = false, msg = "Event is not existed" });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create an product and can add many images including
        /// </summary>
        /// <returns>the added event product</returns>
        ///    /// <remarks>
        /// Sample request:
        ///
        ///     POST /event-products
        ///     {
        ///         "EventId":1,
        ///         "Name": First package,
        ///         "Description": "Nice package for student with free purchase",
        ///         "Price":1000,
        ///         "QuantityInStock":10
        ///         "fileImages":[{"input1"}, {"input2"}]
        ///      }
        /// </remarks>
        /// <response code="200">Returns a list of products</response>

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("campaigns")]
        public async Task<IActionResult> CreateAsync([FromForm] EventCampaignDTO model)
        {
            try
            {
                var result = await _eventCampaignService.CreateEventCampaignAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAsync([FromBody] List<EventProductCreateModel> models)
        //{
        //    try
        //    {
        //        var result = await _eventProductService.CreateEventProductAsync(models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// update an event product by its id
        /// </summary>
        /// <response code="200">Returns a product</response>
        [HttpPut("campaigns/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] EventCampaignDTO model)
        {
            try
            {
                var result = await _eventCampaignService.UpdateEventCampaignAsync(id, model);
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

        /// <summary>
        /// delete a list of event product by their IDs
        /// </summary>
        /// <response code="200">Returns list of remove products</response>
        [HttpDelete("campaigns")]
        public async Task<IActionResult> DeleteAsync([FromBody] List<int> campaignIds)
        {
            try
            {
                var result = await _eventCampaignService.DeleteEventCampaignAsync(campaignIds);
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