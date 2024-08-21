using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardLabelRepository : IGenericRepository<EventBoardLabel>
    {
        Task<List<EventBoardLabel>> GetLabelsByEventId(Guid eventId);
        Task<EventBoardLabel> GetLabelById(Guid id);
        Task<EventBoardLabel> CreateLabel(EventBoardLabel label);
        Task<EventBoardLabel> UpdateLabel(EventBoardLabel label);
        Task SoftDeleteLabel(Guid id);
    }
}
