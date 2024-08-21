using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventBoardColumnService
    {
        Task<EventBoardColumnDTO> CreateColumn(EventBoardColumnCreateDTO eventBoardColumnModel);
        Task<bool> DeleteColumn(Guid id);
        Task<List<EventBoardColumnDTO>> GetColumnsByEventBoardId(Guid eventBoardId);
        Task<EventBoardColumnDTO> GetColumnById(Guid id);
        Task<EventBoardColumnDTO> UpdateColumn(Guid id, EventBoardColumnUpdateDTO eventBoardColumnModel);
    }
}
