using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
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

    }
}
