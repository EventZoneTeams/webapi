using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<List<EventOrder>> GetEventOrdersByEventId(int eventId)
        {
            return await _context.EventOrders
                .Where(x => x.EventId == eventId)
                .Include(x => x.EventOrderDetails)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
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
                        TotalAmount = 0,
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
                            EventOrderId = newOrder.Id,
                            Quantity = item.Quantity,
                            Price = package.TotalPrice,
                            EventOrder = newOrder,
                            EventPackage = package,
                            CreatedAt = _timeService.GetCurrentTime(),
                            CreatedBy = _claimsService.GetCurrentUserId,
                        };

                        eventOrderDetails.Add(orderDetail);
                        newOrder.TotalAmount += ((Int64)package.TotalPrice * item.Quantity);
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

        public async Task<EventOrder> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums)
        {
            var order = await _context.EventOrders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Status = eventOrderStatusEnums.ToString();
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<EventOrder>> getCurrentUserOrder()
        {
            try
            {
                var currentUser = _claimsService.GetCurrentUserId;
                if (currentUser < 0)
                {
                    throw new Exception("User havent sign in, can not found");
                    //return null;
                }

                return await _context.EventOrders
                    .Where(x => x.UserId == currentUser)
                    .Include(x => x.EventOrderDetails)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}