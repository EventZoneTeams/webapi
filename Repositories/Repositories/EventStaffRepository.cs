using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<object>> GetUserListOrderAndTicket(Guid eventId)
        {
            var result = await _context.Users
                .Where(user => user.BookedTickets.Any(bt => bt.EventId == eventId))
                .Select(user => new
                {
                    User = new
                    {
                        user.Id,
                        user.FullName,
                        user.Email,
                        user.PhoneNumber
                    },
                    BookedTickets = user.BookedTickets
                        .Where(bt => bt.EventId == eventId)
                        .Select(bt => new
                        {
                            bt.Id,
                            bt.PaidPrice,
                            bt.IsCheckedIn,
                            bt.AttendeeNote
                        })
                        .ToList(),
                    Products = _context.EventOrderDetails
                        .Where(eod => eod.EventOrder.EventId == eventId && eod.EventOrder.UserId == user.Id)
                        .Select(eod => new
                        {
                            eod.Id,
                            eod.EventProduct.Name,
                            eod.Quantity,
                            eod.Price
                        })
                        .ToList()
                })
                .ToListAsync();

            return result.Cast<object>().ToList();
        }
    }
}
