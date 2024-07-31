using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<User> _userManager;

        public NotificationRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims, UserManager<User> userManager) : base(studentEventForumDbContext, timeService, claims)

        {
            _context = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
            _userManager = userManager;
        }

        public async Task<List<Notification>> ReadAllNotification(Guid userId)
        {
            var notifications = await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<int> GetUnreadNotificationQuantity(Guid userId)
        {
            var result = await _context.Notifications.Where(x => x.UserId == userId && x.IsRead == false).CountAsync();
            return result;
        }

        public async Task<List<Notification>> GetListByUserId(Guid userId)
        {
            var notifications = new List<Notification>();
            //check role of user to get notification
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                notifications = await _context.Notifications.Where(x => x.Sender == "Manager").ToListAsync();
            }
            else if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                notifications = await _context.Notifications.Where(x => x.Sender == "Admin").ToListAsync();
            }
            else
            {
                notifications = await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            }

            //sort created date
            notifications = notifications.OrderByDescending(x => x.CreatedAt).ToList();
            return notifications;
        }
    }
}
