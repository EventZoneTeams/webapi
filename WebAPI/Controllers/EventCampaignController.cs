using EventZone.Domain.DTOs.EventCampaignDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.EventCampaignModels;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventZone.WebAPI.Controllers
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
        public async Task<IActionResult> UpdateAsync(Guid id)
        {
            try
            {
                var result = await _eventCampaignService.GetACampaignsByIdAsync(id);
                if (result == null)
                {
                    return NotFound(ApiResult<EventCampaignStaticticDTO>.Error(null, "Campaign is not found"));
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

                return Ok(ApiResult<Pagination<EventCampaignDTO>>.Succeed(result, "Get list events successfully"));
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
        public async Task<IActionResult> GetAllCampaignByEventAsync(Guid eventid)
        {
            try
            {
                var data = await _eventCampaignService.GetAllCampaignsByEventAsync(eventid);
                if (data == null)
                {
                    throw new Exception("This campaign is not existed");
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
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
        ///         "StartDate":2024-07-18T03:14:40.143Z
        ///         "QuantityInStock":10
        ///         "fileImages":[{"input1"}, {"input2"}]
        ///      }
        /// </remarks>
        /// <response code="200">Returns a list of products</response>

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("campaigns")]
        public async Task<IActionResult> CreateAsync([FromForm] EventCampaignCreateDTO model)
        {
            try
            {
                var result = await _eventCampaignService.CreateEventCampaignAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
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
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] EventCampaignUpdateDTO model)
        {
            try
            {
                var result = await _eventCampaignService.UpdateEventCampaignAsync(id, model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                throw new Exception("This campaign is not existed");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// delete a campaign  by their ID
        /// </summary>
        /// <response code="200">Returns list of remove products</response>
        [HttpDelete("campaigns/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                var result = await _eventCampaignService.DeleteCampaignByIdAsync(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                throw new Exception("This campaign is not existed");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}