using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class EventBoardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEventBoardService _eventBoardService;
        private readonly IEventBoardColumnService _eventBoardColumnService;
        private readonly IEventBoardLabelService _eventBoardLabelService;
        private readonly IEventBoardTaskLabelService _eventBoardTaskLabelService;

        public EventBoardController(IMapper mapper,
                IEventBoardService eventBoardService,
                IEventBoardColumnService eventBoardColumnService,
                IEventBoardLabelService eventBoardLabelService,
                IEventBoardTaskLabelService eventBoardTaskLabelService
            )
        {
            _mapper = mapper;
            _eventBoardService = eventBoardService;
            _eventBoardColumnService = eventBoardColumnService;
            _eventBoardLabelService = eventBoardLabelService;
            _eventBoardTaskLabelService = eventBoardTaskLabelService;
        }

        /**
         * Event Board CRUD
         **/

        [HttpGet("events/{eventId}/event-boards")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoard>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoards(Guid eventId)
        {
            try
            {
                var data = await _eventBoardService.GetBoardsByEventId(eventId);
                return Ok(ApiResult<List<EventBoardResponseDTO>>.Succeed(data, "Get EventBoards Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpGet("event-boards/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoard>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardDetails(Guid id)
        {
            try
            {
                var data = await _eventBoardService.GetBoardById(id);
                if (data == null)
                {
                    return NotFound(ApiResult<object>.Error(null, "EventBoard not found!"));
                }
                return Ok(ApiResult<EventBoardResponseDTO>.Succeed(data, "Get EventBoard Details Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-boards")]
        [ProducesResponseType(typeof(ApiResult<EventBoard>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoard([FromBody] EventBoard eventBoard)
        {
            try
            {
                var param = _mapper.Map<EventBoardCreateDTO>(eventBoard);
                var data = await _eventBoardService.CreateBoard(param);
                return CreatedAtAction(nameof(GetEventBoardDetails), new { id = data.Id }, ApiResult<EventBoardResponseDTO>.Succeed(data, "Create EventBoard Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-boards/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoard>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoard(Guid id, [FromBody] EventBoard eventBoard)
        {
            try
            {
                var param = _mapper.Map<EventBoardUpdateDTO>(eventBoard);
                param.Id = id;
                var data = await _eventBoardService.UpdateBoard(id, param);
                return Ok(ApiResult<EventBoardResponseDTO>.Succeed(data, "Update EventBoard Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpDelete("event-boards/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoard(Guid id)
        {
            try
            {
                await _eventBoardService.DeleteBoard(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoard Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /**
         * Event Board Label CRUD
         **/

        [HttpGet("event-board-labels/{eventId}")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoardLabelDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardLabels(Guid eventId)
        {
            try
            {
                var data = await _eventBoardLabelService.GetLabelsByEventId(eventId);
                return Ok(ApiResult<List<EventBoardLabelDTO>>.Succeed(data, "Get EventBoardLabels Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-board-labels")]
        [ProducesResponseType(typeof(ApiResult<EventBoardLabelDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardLabel([FromBody] EventBoardLabelCreateDTO eventBoardLabelDto)
        {
            try
            {
                var data = await _eventBoardLabelService.CreateLabel(eventBoardLabelDto);
                return CreatedAtAction(nameof(GetEventBoardLabels), new { eventId = eventBoardLabelDto.EventId }, ApiResult<EventBoardLabelDTO>.Succeed(data, "Create EventBoardLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-board-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardLabelDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardLabel(Guid id, [FromBody] EventBoardLabelUpdateDTO eventBoardLabelDto)
        {
            try
            {
                var data = await _eventBoardLabelService.UpdateLabel(id, eventBoardLabelDto);
                return Ok(ApiResult<EventBoardLabelDTO>.Succeed(data, "Update EventBoardLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpDelete("event-board-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoardLabel(Guid id)
        {
            try
            {
                await _eventBoardLabelService.DeleteLabel(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /**
         * Event Board Task Label CRUD
         **/

        [HttpGet("event-board-task-labels/{eventBoardId}")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoardTaskLabelDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardTaskLabels(Guid eventBoardId)
        {
            try
            {
                var data = await _eventBoardTaskLabelService.GetLabelsByEventBoardId(eventBoardId);
                return Ok(ApiResult<List<EventBoardTaskLabelDTO>>.Succeed(data, "Get EventBoardTaskLabels Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-board-task-labels")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskLabelDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardTaskLabel([FromBody] EventBoardTaskLabelCreateDTO eventBoardTaskLabelDto)
        {
            try
            {
                var data = await _eventBoardTaskLabelService.CreateLabel(eventBoardTaskLabelDto);
                return CreatedAtAction(nameof(GetEventBoardTaskLabels), new { eventBoardId = eventBoardTaskLabelDto.EventBoardId }, ApiResult<EventBoardTaskLabelDTO>.Succeed(data, "Create EventBoardTaskLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-board-task-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskLabelDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardTaskLabel(Guid id, [FromBody] EventBoardTaskLabelUpdateDTO eventBoardTaskLabelDto)
        {
            try
            {
                var data = await _eventBoardTaskLabelService.UpdateLabel(id, eventBoardTaskLabelDto);
                return Ok(ApiResult<EventBoardTaskLabelDTO>.Succeed(data, "Update EventBoardTaskLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpDelete("event-board-task-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoardTaskLabel(Guid id)
        {
            try
            {
                await _eventBoardTaskLabelService.DeleteLabel(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardTaskLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

    }
}
