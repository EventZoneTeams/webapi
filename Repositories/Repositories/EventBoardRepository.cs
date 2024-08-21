using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;

namespace EventZone.Repositories.Repositories
{
    public class EventBoardRepository : IEventBoardRepository
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

        public Task<EventBoard> CreateBoard(EventBoard board)
        {
            throw new NotImplementedException();
        }

        public Task<EventBoard> GetBoardById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<EventBoard>> GetBoardsByEventId(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteBoard(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<EventBoard> UpdateBoard(EventBoard board)
        {
            throw new NotImplementedException();
        }
    }
}
