using Domain.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventOrderRepository : GenericRepository<EventOrder>, IEventOrderRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventOrderRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }
    }
}
