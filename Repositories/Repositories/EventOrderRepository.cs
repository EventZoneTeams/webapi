using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class EventOrderRepository : GenericRepository<EventOrder>, IEventOrderRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        public EventOrderRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<EventOrder> CreateOrderWithOrderDetails(int eventId, int userId, List<EventOrderDetail> orderDetails)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newOrder = new EventOrder
                    {
                        EventId = eventId,
                        UserId = userId,
                        TotalAmount = 0m,
                        Status = EventOrderStatusEnums.PENDING.ToString(),
                        CreatedAt = _timeService.GetCurrentTime(),
                        CreatedBy = _claimsService.GetCurrentUserId,
                    };

                    await _context.EventOrders.AddAsync(newOrder);
                    await _context.SaveChangesAsync();

                    List<EventOrderDetail> eventOrderDetails = new List<EventOrderDetail>();
                    foreach (var item in orderDetails)
                    {
                        var package = await _context.EventPackages.FindAsync(item.PackageId);
                        if (package == null)
                        {
                            throw new Exception("Package not found");
                        }

                        var orderDetail = new EventOrderDetail
                        {
                            PackageId = item.PackageId,
                            OrderId = newOrder.Id,
                            Quantity = item.Quantity,
                            Price = package.TotalPrice
                        };

                        eventOrderDetails.Add(orderDetail);
                        newOrder.TotalAmount += ((decimal)package.TotalPrice * item.Quantity);
                    }

                    _context.Entry(newOrder).State = EntityState.Modified;
                    await _context.EventOrderDetails.AddRangeAsync(eventOrderDetails);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return newOrder;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}
