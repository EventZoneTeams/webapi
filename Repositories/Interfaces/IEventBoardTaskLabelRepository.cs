using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardTaskLabelRepository : IGenericRepository<EventBoardTaskLabel>
    {
        Task<List<EventBoardTaskLabel>> GetLabelsByEventBoardId(Guid eventBoardId);
    }
}
