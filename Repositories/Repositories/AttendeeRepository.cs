using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Repositories
{
    public class AttendeeRepository : GenericRepository<BookedTicket>, IAttendeeRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public AttendeeRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<BookedTicket>> GetBookedTicketsByOrderId(Guid orderId)
        {
            var result = await _context.BookedTickets.Where(x => x.EventOrderId == orderId).ToListAsync();
            return result;
        }
    }
}