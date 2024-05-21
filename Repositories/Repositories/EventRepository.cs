using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext)
        {
            _studentEventForumDbContext = studentEventForumDbContext;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            //get all events
            return await _studentEventForumDbContext.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventEntity = await _studentEventForumDbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
            return eventEntity;
        }
    }
}
