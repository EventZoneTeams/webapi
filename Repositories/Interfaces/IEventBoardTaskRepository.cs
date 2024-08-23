using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;
using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardTaskRepository : IGenericRepository<EventBoardTask>
    {
        Task<List<EventBoardTask>> GetTasksByColumnId(Guid eventBoardColumnId);
        Task<EventBoardTask> GetTaskById(Guid id);
        Task<EventBoardTask> CreateTask(EventBoardTaskCreateDTO eventBoardTaskCreateDTO);
        Task<EventBoardTask> UpdateTask(Guid taskId, EventBoardTaskUpdateDTO eventBoardTaskUpdateDTO);
        Task DeleteTask(Guid taskId);
    }
}
