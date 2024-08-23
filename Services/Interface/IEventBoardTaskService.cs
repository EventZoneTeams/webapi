using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventBoardTaskService
    {
        Task<EventBoardTaskResponseDTO> GetTaskById(Guid id);
        Task<List<EventBoardTaskResponseDTO>> GetTasksByColumnId(Guid columnId);
        Task<EventBoardTaskResponseDTO> CreateTask(EventBoardTaskCreateDTO taskCreateDto);
        Task<EventBoardTaskResponseDTO> UpdateTask(Guid taskId, EventBoardTaskUpdateDTO taskUpdateDto);
        Task DeleteTask(Guid id);
    }
}
