using AutoMapper;
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
                return Ok(ApiResult<List<EventBoard>>.Succeed(data, "Get EventBoards Successfully!"));
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
                return Ok(ApiResult<EventBoard>.Succeed(data, "Get EventBoard Details Successfully!"));
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
                var data = await _eventBoardService.CreateBoard(eventBoard);
                return CreatedAtAction(nameof(GetEventBoardDetails), new { id = data.Id }, ApiResult<EventBoard>.Succeed(data, "Create EventBoard Successfully!"));
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
                eventBoard.Id = id;
                var data = await _eventBoardService.UpdateBoard(eventBoard);
                return Ok(ApiResult<EventBoard>.Succeed(data, "Update EventBoard Successfully!"));
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
                await _eventBoardService.SoftDeleteBoard(id);
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
        [ProducesResponseType(typeof(ApiResult<List<EventBoardLabel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardLabels(Guid eventId)
        {
            try
            {
                var data = await _eventBoardLabelService.GetLabelsByEventId(eventId);
                return Ok(ApiResult<List<EventBoardLabel>>.Succeed(data, "Get EventBoardLabels Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-board-labels")]
        [ProducesResponseType(typeof(ApiResult<EventBoardLabel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardLabel([FromBody] EventBoardLabel eventBoardLabel)
        {
            try
            {
                var data = await _eventBoardLabelService.CreateLabel(eventBoardLabel);
                return CreatedAtAction(nameof(GetEventBoardLabels), new { eventId = eventBoardLabel.EventId }, ApiResult<EventBoardLabel>.Succeed(data, "Create EventBoardLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-board-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardLabel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardLabel(Guid id, [FromBody] EventBoardLabel eventBoardLabel)
        {
            try
            {
                eventBoardLabel.Id = id;
                var data = await _eventBoardLabelService.UpdateLabel(eventBoardLabel);
                return Ok(ApiResult<EventBoardLabel>.Succeed(data, "Update EventBoardLabel Successfully!"));
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
                await _eventBoardLabelService.SoftDeleteLabel(id);
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
        [ProducesResponseType(typeof(ApiResult<List<EventBoardTaskLabel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardTaskLabels(Guid eventBoardId)
        {
            try
            {
                var data = await _eventBoardTaskLabelService.GetLabelsByEventBoardId(eventBoardId);
                return Ok(ApiResult<List<EventBoardTaskLabel>>.Succeed(data, "Get EventBoardTaskLabels Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-board-task-labels")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskLabel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardTaskLabel([FromBody] EventBoardTaskLabel eventBoardTaskLabel)
        {
            try
            {
                var data = await _eventBoardTaskLabelService.CreateLabel(eventBoardTaskLabel);
                return CreatedAtAction(nameof(GetEventBoardTaskLabels), new { eventBoardId = eventBoardTaskLabel.EventBoardId }, ApiResult<EventBoardTaskLabel>.Succeed(data, "Create EventBoardTaskLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-board-task-labels/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskLabel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardTaskLabel(Guid id, [FromBody] EventBoardTaskLabel eventBoardTaskLabel)
        {
            try
            {
                eventBoardTaskLabel.Id = id;
                var data = await _eventBoardTaskLabelService.UpdateLabel(eventBoardTaskLabel);
                return Ok(ApiResult<EventBoardTaskLabel>.Succeed(data, "Update EventBoardTaskLabel Successfully!"));
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
                await _eventBoardTaskLabelService.SoftDeleteLabel(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardTaskLabel Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /**
         * Event Board Column CRUD
         **/

        [HttpGet("event-board-columns/{eventBoardId}")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoardColumn>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardColumns(Guid eventBoardId)
        {
            try
            {
                var data = await _eventBoardColumnService.GetColumnsByEventBoardId(eventBoardId);
                return Ok(ApiResult<List<EventBoardColumn>>.Succeed(data, "Get EventBoardColumns Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("event-board-columns")]
        [ProducesResponseType(typeof(ApiResult<EventBoardColumn>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardColumn([FromBody] EventBoardColumn eventBoardColumn)
        {
            try
            {
                var data = await _eventBoardColumnService.CreateColumn(eventBoardColumn);
                return CreatedAtAction(nameof(GetEventBoardColumns), new { eventBoardId = eventBoardColumn.EventBoardId }, ApiResult<EventBoardColumn>.Succeed(data, "Create EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-board-columns/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardColumn>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardColumn(Guid id, [FromBody] EventBoardColumn eventBoardColumn)
        {
            try
            {
                eventBoardColumn.Id = id;
                var data = await _eventBoardColumnService.UpdateColumn(eventBoardColumn);
                return Ok(ApiResult<EventBoardColumn>.Succeed(data, "Update EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpDelete("event-board-columns/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoardColumn(Guid id)
        {
            try
            {
                await _eventBoardColumnService.SoftDeleteColumn(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}
