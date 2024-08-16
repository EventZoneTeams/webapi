using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;

namespace EventZone.Repositories.Repositories
{
    public class EventCategoryRepository : GenericRepository<EventCategory>, IEventCategoryRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventCategoryRepository(StudentEventForumDbContext context, ICurrentTime currentTime, IClaimsService claimsService) : base(context, currentTime, claimsService)
        {
            _context = context;
            _timeService = currentTime;
            _claimsService = claimsService;
        }
    }
}
