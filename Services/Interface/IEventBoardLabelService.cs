using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventBoardLabelService
    {
        Task<EventBoardLabelDTO> CreateLabel(EventBoardLabelCreateDTO eventBoardLabelModel);
        Task<bool> DeleteLabel(Guid id);
        Task<List<EventBoardLabelDTO>> GetLabelsByEventId(Guid eventId);
        Task<EventBoardLabelDTO> GetLabelById(Guid id);
        Task<EventBoardLabelDTO> UpdateLabel(Guid id, EventBoardLabelUpdateDTO eventBoardLabelModel);
    }
}
