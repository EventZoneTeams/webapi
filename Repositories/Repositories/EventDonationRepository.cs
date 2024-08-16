using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class EventDonationRepository : GenericRepository<EventDonation>, IEventDonationRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public EventDonationRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventDonation>> GetAllDonationByCampaignId(Guid id)
        {
            var data = await _context.EventDonations.Include(x => x.User).Where(c => c.EventCampaignId == id).ToListAsync();
            return data;
        }

        public async Task<List<EventDonation>> GetMyDonation()
        {
            var userId = _claimsService.GetCurrentUserId;
            var data = await _context.EventDonations.Include(x => x.User).Where(c => c.UserId == userId).ToListAsync();
            return data;
        }
    }
}