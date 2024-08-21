using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardRepository : GenericRepository<EventBoard>, IEventBoardRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventBoardRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventBoard>> GetBoardsByEventId(Guid eventId)
        {
            return await _context.EventBoards
                                 .Where(b => b.EventId == eventId && !b.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<EventBoard> GetBoardById(Guid id)
        {
            return await _context.EventBoards
                                .Include(b => b.Event) // Include the related Event
                                .Include(b => b.Leader) // Include the related Leader (one-to-one)
                                .Include(b => b.EventBoardColumns) // Include the related EventBoardColumns
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                    .ThenInclude(t => t.EventBoardColumn) // Include the related EventBoardColumn for each task
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                    .ThenInclude(t => t.EventBoardTaskAssignments) // Include task assignments (many-to-many)
                                        .ThenInclude(ta => ta.User) // Include the related User for each assignment
                                .Include(b => b.EventBoardLabelAssignments) // Include the many-to-many relationship with EventBoardLabels
                                    .ThenInclude(l => l.EventBoardLabel) // Include the actual EventBoardLabel in the assignment
                                .Include(b => b.EventBoardTaskLabels) // Include the many-to-many relationship with EventBoardLabels
                                .Include(b => b.EventBoardTasks) // Include the related EventBoardTasks
                                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<EventBoard> CreateBoard(EventBoard board)
        {
            var result = await _context.EventBoards.AddAsync(board);
            return result.Entity;
        }

        public async Task<EventBoard> UpdateBoard(EventBoard board)
        {
            _context.EventBoards.Update(board);
            return board;
        }

        public async Task SoftDeleteBoard(Guid id)
        {
            var board = await GetByIdAsync(id);
            if (board != null)
            {
                board.IsDeleted = true;
                _context.EventBoards.Update(board);
            }
        }
    }
}
