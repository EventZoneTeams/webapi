using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardRepository : IGenericRepository<EventBoard>
    {
        Task<List<EventBoard>> GetBoardsByEventId(Guid eventId);
        Task<EventBoard> GetBoardById(Guid id);
        Task<EventBoard> CreateBoard(EventBoard board);
        Task<EventBoard> UpdateBoard(EventBoard board);
        Task SoftDeleteBoard(Guid id);
    }
}