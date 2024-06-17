using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public NotificationRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims) : base(studentEventForumDbContext, timeService, claims)

        {
            _context = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
        }

        public async Task<List<Notification>> ReadAllNotification(int userId)
        {
            var notifications = await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<int> GetUnreadNotificationQuantity(int userId)
        {
            var result = await _context.Notifications.Where(x => x.UserId == userId && x.IsRead == false).CountAsync();
            return result;
        }
    }
}
