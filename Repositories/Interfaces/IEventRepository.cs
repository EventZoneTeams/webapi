using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        public Task<List<Event>> GetEventsAsync();
        public Task<Event> GetEventByIdAsync(int id);
        public Task<Event> CreateEventAsync(Event eventModel);
        public Task<Event> DeleteEventAsync(int id);
    }
}
