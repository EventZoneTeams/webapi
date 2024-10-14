using EventZone.Domain.Entities;
using EventZone.Domain.Extensions;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EventZone.Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims) : base(studentEventForumDbContext, timeService, claims)

        {
            _context = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
        }

        public IQueryable<Event> FilterAllField(EventParams eventParams)
        {
            var query = _context.Events
                .Include(x => x.User)
                .Include(x => x.EventCategory)
                .Search(eventParams.SearchTerm)
                .Filter(eventParams.EventCategoryId)
                .FilterByEventDate(eventParams.EventStartDate, eventParams.EventEndDate)
                .FilterByStatus(eventParams.Status.ToString())
                .FilterByUserId(eventParams.UserId);

            return query;
        }

        public async Task<Event> DeleteEventDatabaseAsync(Guid id)
        {
            var existingEvent = await GetByIdAsync(id);
            if (existingEvent == null)
            {
                return null;
            }
            var result = _context.Events.Remove(existingEvent);
            return result.Entity;
        }
    }
}