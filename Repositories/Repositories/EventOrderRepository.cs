using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Domain.Extensions;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
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

        public IQueryable<EventOrder> FilterAllField(Guid eventId, OrderParams orderParams)
        {
            var query = _context.EventOrders
               .Include(x => x.User)
               .Include(x => x.EventOrderDetails)
               .Search(orderParams.SearchTerm)
               .FilterEventId(eventId)
               .FilterByStatus(orderParams.Status.ToString())
               .FilterByEventOrderDate(orderParams.FromDate, orderParams.ToDate)
               .Sort();

            return query;
        }

        public async Task<EventOrder> CreateOrderWithOrderDetails(Guid eventId, Guid userId, List<EventOrderDetail> orderDetails)
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
                        OrderType = "PRODUCT",
                    };

                    newOrder = await AddAsync(newOrder);
                    await _context.SaveChangesAsync();

                    List<EventOrderDetail> eventOrderDetails = new List<EventOrderDetail>();
                    foreach (var item in orderDetails)
                    {
                        var product = await _context.EventProducts.FindAsync(item.EventProductId);
                        if (product == null)
                        {
                            throw new Exception("Product not found");
                        }

                        // Kiểm tra số lượng sản phẩm trong kho
                        if (product.QuantityInStock < item.Quantity)
                        {
                            throw new Exception($"Product '{product.Name}' does not have enough stock. Available: {product.QuantityInStock}");
                        }

                        // Trừ số lượng trong kho của sản phẩm
                        product.QuantityInStock -= item.Quantity;

                        var orderDetail = new EventOrderDetail
                        {
                            EventProductId = item.EventProductId,
                            EventOrderId = newOrder.Id,
                            Quantity = item.Quantity,
                            Price = product.Price,
                            EventOrder = newOrder,
                            EventProduct = product,
                            CreatedAt = _timeService.GetCurrentTime(),
                            CreatedBy = _claimsService.GetCurrentUserId,
                        };

                        eventOrderDetails.Add(orderDetail);
                        newOrder.TotalAmount += product.Price * item.Quantity;
                        _context.EventProducts.Update(product);
                    }

                    // Cập nhật lại trạng thái sản phẩm sau khi trừ số lượng

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

        public async Task<EventOrder> UpdateOrderStatus(Guid orderId, EventOrderStatusEnums eventOrderStatusEnums)
        {
            var order = await _context.EventOrders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Status = eventOrderStatusEnums.ToString();
            _context.Entry(order).State = EntityState.Modified;
            return order;
        }

        public async Task<List<EventOrder>> getCurrentUserOrder()
        {
            try
            {
                var currentUser = _claimsService.GetCurrentUserId;
                if (currentUser == Guid.Empty)
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