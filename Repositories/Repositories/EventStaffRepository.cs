using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;

namespace EventZone.Repositories.Repositories
{
    public class EventStaffRepository : GenericRepository<EventStaff>, IEventStaffRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventStaffRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }
    }
}
