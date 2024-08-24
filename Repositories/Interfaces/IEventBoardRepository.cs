using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardRepository : IGenericRepository<EventBoard>
    {
        Task<List<EventBoard>> GetBoardsByEventId(Guid eventId);
        Task<EventBoard> GetBoardById(Guid id);
        Task<EventBoard> CreateBoard(EventBoardCreateDTO eventBoardCreateDTO);
        Task<EventBoard> UpdateBoard(Guid boardId, EventBoardUpdateDTO eventBoardUpdateDTO);
        Task SoftDeleteBoard(Guid id);
    }
}