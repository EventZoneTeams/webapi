using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventBoardColumnRepository : IGenericRepository<EventBoardColumn>
    {
        Task<List<EventBoardColumn>> GetColumnsByEventBoardId(Guid eventBoardId);
        Task<EventBoardColumn> GetColumnById(Guid id);
        Task<EventBoardColumn> CreateColumn(EventBoardColumn column);
        Task<EventBoardColumn> UpdateColumn(EventBoardColumn column);
        Task SoftDeleteColumn(Guid id);
    }
}
