using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event, int>, IEventRepository
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext) : base(studentEventForumDbContext)
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
        public async Task<Event> CreateEventAsync(Event eventEntity)
        {
            var entry = await _studentEventForumDbContext.Events.AddAsync(eventEntity);
            return entry.Entity; // test
        }

        public async Task<Event> DeleteEventAsync(int id)
        {
            var Event = await _studentEventForumDbContext.Events.FirstOrDefault(e => e.Id == id);

            if (Even)

        }
    }
}
