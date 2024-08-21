using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventBoardTaskLabelService
    {
        Task<EventBoardTaskLabelDTO> CreateLabel(EventBoardTaskLabelCreateDTO eventBoardTaskLabelModel);
        Task<bool> DeleteLabel(Guid id);
        Task<List<EventBoardTaskLabelDTO>> GetLabelsByEventBoardId(Guid eventBoardId);
        Task<EventBoardTaskLabelDTO> GetLabelById(Guid id);
        Task<EventBoardTaskLabelDTO> UpdateLabel(Guid id, EventBoardTaskLabelUpdateDTO eventBoardTaskLabelModel);
    }
}
