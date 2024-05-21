using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {

        private readonly StudentEventForumDbContext _studentEventForumDbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims) : base(studentEventForumDbContext, timeService, claims)

        {
            _studentEventForumDbContext = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
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
            //var Event = await _studentEventForumDbContext.Events.FirstOrDefault(e => e.Id == id);

            //if (Even)

            return null;

        }
    }
}
