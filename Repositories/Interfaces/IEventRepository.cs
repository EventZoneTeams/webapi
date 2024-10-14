using EventZone.Domain.Entities;
using EventZone.Repositories.Helper;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<Event> DeleteEventDatabaseAsync(Guid id);
        public IQueryable<Event> FilterAllField(EventParams eventParams);
    }
}
