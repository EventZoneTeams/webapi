using Domain.Entities;
using Repositories.Helper;

namespace Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        public IQueryable<Event> FilterAllField(EventParams eventParams);
    }
}
