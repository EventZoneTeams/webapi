using EventZone.Domain.DTOs.EventBoardDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventBoardService
    {
        Task<EventBoardResponseDTO> CreateBoard(EventBoardCreateDTO eventBoardModel);
        Task<bool> DeleteBoard(Guid id);
        Task<List<EventBoardResponseDTO>> GetBoardsByEventId(Guid eventId);
        Task<EventBoardResponseDTO> GetBoardById(Guid id);
        Task<EventBoardResponseDTO> UpdateBoard(Guid id, EventBoardUpdateDTO eventBoardModel);
    }
}
