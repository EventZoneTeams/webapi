﻿using AutoMapper;
using EventZone.Domain.DTOs.WalletDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WithdrawnRequestController : ControllerBase
    {
        private readonly ILogger<WithdrawnRequestController> _logger;
        private readonly IWithdrawnRequestService _withdrawnRequestService;
        private readonly IMapper _mapper;

        public WithdrawnRequestController(ILogger<WithdrawnRequestController> logger, IWithdrawnRequestService withdrawnRequestService, IMapper mapper)
        {
            _logger = logger;
            _withdrawnRequestService = withdrawnRequestService;
            _mapper = mapper;
        }

        [HttpGet("GetListByUserId")]
        public async Task<IActionResult> GetListWalletByUserId()
        {
            try
            {
                var result = await _withdrawnRequestService.GetRequestByUserId();
                return Ok(ApiResult<List<WithdrawnRequest>>.Succeed(result, "Request list retrieved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving withdrawal requests for user.");
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateWithdrawnRequest([FromBody] WithdrawnRequestDTO request)
        {
            try
            {
                var entity = _mapper.Map<WithdrawnRequest>(request);
                var result = await _withdrawnRequestService.CreateARequest(entity);
                return Ok(ApiResult<WithdrawnRequest>.Succeed(result, "Request created successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal request.");
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("ApproveRequest/{id}")]
        public async Task<IActionResult> ApproveWithdrawnRequest(Guid id, [FromBody] string imageUrl)
        {
            try
            {
                var result = await _withdrawnRequestService.ApproveRequest(id, imageUrl);
                return Ok(ApiResult<WithdrawnRequest>.Succeed(result, "Request approved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving withdrawal request with ID {id}.");
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("RejectRequest/{id}")]
        public async Task<IActionResult> RejectWithdrawnRequest(Guid id)
        {
            try
            {
                var result = await _withdrawnRequestService.RejectRequest(id);
                return Ok(ApiResult<WithdrawnRequest>.Succeed(result, "Request rejected successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting withdrawal request with ID {id}.");
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}
