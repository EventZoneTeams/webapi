using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardColumnRepository : GenericRepository<EventBoardColumn>, IEventBoardColumnRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventBoardColumnRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventBoardColumn>> GetColumnsByEventBoardId(Guid eventBoardId)
        {
            return await _context.Set<EventBoardColumn>()
                                 .Where(c => c.EventBoardId == eventBoardId && !c.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<EventBoardColumn> GetColumnById(Guid id)
        {
            return await _context.Set<EventBoardColumn>().FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<EventBoardColumn> CreateColumn(EventBoardColumn column)
        {
            var result = await _context.Set<EventBoardColumn>().AddAsync(column);
            return result.Entity;
        }

        public async Task<EventBoardColumn> UpdateColumn(EventBoardColumn column)
        {
            _context.Set<EventBoardColumn>().Update(column);
            return column;
        }

        public async Task SoftDeleteColumn(Guid id)
        {
            var column = await GetByIdAsync(id);
            if (column != null)
            {
                column.IsDeleted = true;
                _context.Set<EventBoardColumn>().Update(column);
            }
        }
    }
}
