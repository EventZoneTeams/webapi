using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventFeedbackRepository : GenericRepository<EventFeedback>, IEventFeedbackRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventFeedbackRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<EventFeedback> CreateFeedbackAsync(EventFeedback newFeedback)
        {
            try
            {
                newFeedback.UserId = _claimsService.GetCurrentUserId == -1 ? 1 : _claimsService.GetCurrentUserId; // handle tạm thời khi test
                await AddAsync(newFeedback);
                return newFeedback;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventFeedback>> GettAllFeedbacksAsync()
        {
            return await _context.EventFeedbacks.Include(x => x.Event).Include(u => u.User).ToListAsync();
        }
    }
}