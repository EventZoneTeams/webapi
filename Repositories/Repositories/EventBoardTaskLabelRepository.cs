using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardTaskLabelRepository : GenericRepository<EventBoardTaskLabel>, IEventBoardTaskLabelRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventBoardTaskLabelRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventBoardTaskLabel>> GetLabelsByEventBoardId(Guid eventBoardId)
        {
            return await _context.Set<EventBoardTaskLabel>()
                                 .Where(l => l.EventBoardId == eventBoardId && !l.IsDeleted)
                                 .ToListAsync();
        }


    }
}
