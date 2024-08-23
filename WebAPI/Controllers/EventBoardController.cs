using AutoMapper;
using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;
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
        private readonly IEventBoardTaskService _eventBoardTaskService;

        public EventBoardController(IMapper mapper,
                IEventBoardService eventBoardService,
                IEventBoardColumnService eventBoardColumnService,
                IEventBoardLabelService eventBoardLabelService,
                IEventBoardTaskLabelService eventBoardTaskLabelService,
                IEventBoardTaskService eventBoardTaskService
            )
        {
            _mapper = mapper;
            _eventBoardService = eventBoardService;
            _eventBoardColumnService = eventBoardColumnService;
            _eventBoardLabelService = eventBoardLabelService;
            _eventBoardTaskLabelService = eventBoardTaskLabelService;
            _eventBoardTaskService = eventBoardTaskService;
        }

        /**
         * Event Board CRUD
         **/

        [HttpGet("events/{eventId}/event-boards")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoard([FromBody] EventBoardCreateDTO eventBoardCreateDTO)
        {
            try
            {
                var data = await _eventBoardService.CreateBoard(eventBoardCreateDTO);
                return CreatedAtAction(nameof(GetEventBoardDetails), new { id = data.Id }, ApiResult<EventBoardResponseDTO>.Succeed(data, "Create EventBoard Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPut("event-boards/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoard(Guid id, EventBoardUpdateDTO eventBoardUpdateDTO)
        {
            try
            {
                var data = await _eventBoardService.UpdateBoard(id, eventBoardUpdateDTO);
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

        [HttpGet("events/{eventId}/event-board-labels/")]
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

        [HttpGet("event-boards/{eventBoardId}/event-board-task-labels")]
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

        // Get all columns for an event board
        [HttpGet("event-boards/{eventBoardId}/columns")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoardColumnDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardColumns(Guid eventBoardId)
        {
            try
            {
                var data = await _eventBoardColumnService.GetColumnsByEventBoardId(eventBoardId);
                return Ok(ApiResult<List<EventBoardColumnDTO>>.Succeed(data, "Get EventBoardColumns Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Get a specific column by ID
        [HttpGet("event-board-columns/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardColumnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardColumnById(Guid id)
        {
            try
            {
                var data = await _eventBoardColumnService.GetColumnById(id);
                if (data == null)
                {
                    return NotFound(ApiResult<object>.Error(null, "EventBoardColumn not found!"));
                }
                return Ok(ApiResult<EventBoardColumnDTO>.Succeed(data, "Get EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Create a new column
        [HttpPost("event-board-columns")]
        [ProducesResponseType(typeof(ApiResult<EventBoardColumnDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardColumn([FromBody] EventBoardColumnCreateDTO eventBoardColumnCreateDTO)
        {
            try
            {
                var data = await _eventBoardColumnService.CreateColumn(eventBoardColumnCreateDTO);
                return CreatedAtAction(nameof(GetEventBoardColumnById), new { id = data.Id }, ApiResult<EventBoardColumnDTO>.Succeed(data, "Create EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Update an existing column
        [HttpPut("event-board-columns/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardColumnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardColumn(Guid id, [FromBody] EventBoardColumnUpdateDTO updateDTO)
        {
            try
            {
                var data = await _eventBoardColumnService.UpdateColumn(id, updateDTO);
                return Ok(ApiResult<EventBoardColumnDTO>.Succeed(data, "Update EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Delete a column
        [HttpDelete("event-board-columns/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoardColumn(Guid id)
        {
            try
            {
                await _eventBoardColumnService.DeleteColumn(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardColumn Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Get all tasks for a specific column
        [HttpGet("event-board-columns/{eventBoardColumnId}/tasks")]
        [ProducesResponseType(typeof(ApiResult<List<EventBoardTaskResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardTasks(Guid eventBoardColumnId)
        {
            try
            {
                var data = await _eventBoardTaskService.GetTasksByColumnId(eventBoardColumnId);
                return Ok(ApiResult<List<EventBoardTaskResponseDTO>>.Succeed(data, "Get EventBoardTasks Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Get a specific task by ID
        [HttpGet("event-board-tasks/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventBoardTaskById(Guid id)
        {
            try
            {
                var data = await _eventBoardTaskService.GetTaskById(id);
                if (data == null)
                {
                    return NotFound(ApiResult<object>.Error(null, "EventBoardTask not found!"));
                }
                return Ok(ApiResult<EventBoardTaskResponseDTO>.Succeed(data, "Get EventBoardTask Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Create a new task
        [HttpPost("event-board-tasks")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEventBoardTask([FromBody] EventBoardTaskCreateDTO eventBoardTaskCreateDTO)
        {
            try
            {
                var data = await _eventBoardTaskService.CreateTask(eventBoardTaskCreateDTO);
                return CreatedAtAction(nameof(GetEventBoardTaskById), new { id = data.Id }, ApiResult<EventBoardTaskResponseDTO>.Succeed(data, "Create EventBoardTask Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Update an existing task
        [HttpPut("event-board-tasks/{id}")]
        [ProducesResponseType(typeof(ApiResult<EventBoardTaskResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEventBoardTask(Guid id, [FromBody] EventBoardTaskUpdateDTO eventBoardTaskUpdateDTO)
        {
            try
            {
                var data = await _eventBoardTaskService.UpdateTask(id, eventBoardTaskUpdateDTO);
                return Ok(ApiResult<EventBoardTaskResponseDTO>.Succeed(data, "Update EventBoardTask Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        // Delete a task
        [HttpDelete("event-board-tasks/{id}")]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEventBoardTask(Guid id)
        {
            try
            {
                await _eventBoardTaskService.DeleteTask(id);
                return Ok(ApiResult<object>.Succeed(null, "Delete EventBoardTask Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}
