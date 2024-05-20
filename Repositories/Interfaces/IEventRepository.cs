using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface IEventRepository
    {
        public Task<List<Event>> GetEventsAsync();
        public Task<Event> GetEventByIdAsync(int id);
        //public Task<Event> AddEventAsync(Event eventModel);
        //public Task<Event> UpdateEventAsync(int id, Event eventModel);
        //public Task<bool> DeleteEventAsync(int id);
    }
}
