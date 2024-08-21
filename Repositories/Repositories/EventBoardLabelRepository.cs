using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardLabelRepository : GenericRepository<EventBoardLabel>, IEventBoardLabelRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventBoardLabelRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventBoardLabel>> GetLabelsByEventId(Guid eventId)
        {
            return await _context.Set<EventBoardLabel>()
                                 .Where(l => l.EventId == eventId && !l.IsDeleted)
                                 .ToListAsync();
        }
        public async Task<EventBoardLabel> GetLabelById(Guid id)
        {
            return await _context.Set<EventBoardLabel>()
                                 .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
        }

        public async Task<EventBoardLabel> CreateLabel(EventBoardLabel label)
        {
            var result = await _context.Set<EventBoardLabel>().AddAsync(label);
            return result.Entity;
        }

        public async Task<EventBoardLabel> UpdateLabel(EventBoardLabel label)
        {
            _context.Set<EventBoardLabel>().Update(label);
            return label;
        }

        public async Task SoftDeleteLabel(Guid id)
        {
            var label = await GetByIdAsync(id);
            if (label != null)
            {
                label.IsDeleted = true;
                _context.Set<EventBoardLabel>().Update(label);
            }
        }
    }
}
