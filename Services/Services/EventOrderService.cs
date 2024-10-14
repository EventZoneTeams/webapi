using AutoMapper;
using EventZone.Domain.DTOs.EventCategoryDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventOrderService : IEventOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly INotificationService _notificationService;
        private readonly IRedisService _redisService;
        private readonly IWalletService _walletService;

        public EventOrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, INotificationService notificationService, IRedisService redisService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _notificationService = notificationService;
            _redisService = redisService;
            _walletService = walletService;
        }

        public async Task<EventOrderReponseDTO> GetEventOrderById(Guid orderId) // old version
        {
            var order = await _unitOfWork.EventOrderRepository.GetByIdAsync(orderId, x => x.EventOrderDetails);
            return _mapper.Map<EventOrderReponseDTO>(order);
        }

        public async Task<EventOrderReponseDTO> GetEventOrder(Guid id)
        {
            // Try to get from cache
            var cachedOrder = await _redisService.GetStringAsync(CacheKeys.EventOrder(id));
            if (!string.IsNullOrEmpty(cachedOrder))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<EventOrderReponseDTO>(cachedOrder);
            }

            // If not in cache, query the database
            var eventOrder = await _unitOfWork.EventOrderRepository.GetByIdAsync(id);

            if (eventOrder == null)
            {
                throw new Exception("Event category not found");
            }

            var result = _mapper.Map<EventOrderReponseDTO>(eventOrder);

            // Cache the result
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _redisService.SetStringAsync(CacheKeys.EventOrder(id), serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes

            return result;
        }

        public async Task<PagedList<EventOrder>> GetEventOrders(Guid eventId, OrderParams orderParams)
        {
            var query = _unitOfWork.EventOrderRepository.FilterAllField(eventId, orderParams).AsQueryable();
            var eventOrders = await PagedList<EventOrder>.ToPagedList(query, orderParams.PageNumber, orderParams.PageSize);

            return eventOrders;
        }

        public async Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order)
        {
            var currentUser = _claimsService.GetCurrentUserId;
            if (currentUser == Guid.Empty) throw new Exception("please login");

            var eventObject = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId);
            if (eventObject == null)
            {
                throw new Exception("Event not found");
            }

            var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (user == null)
            {
                throw new Exception("User not existing");
            }

            var newOrderDetailsList = _mapper.Map<List<EventOrderDetail>>(order.EventOrderDetails);
            var orderResponse = await _unitOfWork.EventOrderRepository.CreateOrderWithOrderDetails(order.EventId, currentUser, newOrderDetailsList);
            // update user wallet amount
            var walletTransaction = await _walletService.PurchaseOrder(orderResponse.Id, user.Id);
            // Send notification
            //var notificationToUser = new Notification
            //{
            //    Body = "You create a new order: " + orderResponse.Id,
            //    Title = "Order Created!",
            //    UserId = currentUser,
            //    Url = "/profile/orders",
            //    Sender = "System"
            //};

            //Send notification to event owner
            var eventDetails = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId, x => x.User);
            //var notificationToEventOwner = new Notification
            //{
            //    Body = "You have a new order " + orderResponse.Id + " from user: " + currentUser,
            //    Title = "1 Order Created!",
            //    UserId = eventDetails.UserId,
            //    Url = "/dashboard/my-events/" + order.EventId + "/orders",
            //    Sender = "System"
            //};

            ////await _notificationService.PushNotification(notificationToUser).ConfigureAwait(true);
            //await _notificationService.PushNotification(notificationToEventOwner).ConfigureAwait(true);

            // Clear cache as new category is added
            await _redisService.DeleteKeyAsync(CacheKeys.EventOrders);
            // mapper
            return _mapper.Map<EventOrderReponseDTO>(orderResponse);
        }

        public async Task<EventOrderReponseDTO> UpdateOrderStatus(Guid orderId, EventOrderStatusEnums eventOrderStatusEnums)
        {
            //already checking order existence in calling method
            var order = await _unitOfWork.EventOrderRepository.UpdateOrderStatus(orderId, eventOrderStatusEnums);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                // Clear specific cache key
                await _redisService.DeleteKeyAsync(CacheKeys.EventOrder(orderId));
                // Clear general list cache
                await _redisService.DeleteKeyAsync(CacheKeys.EventOrders);

                return _mapper.Map<EventOrderReponseDTO>(order);
            }
            else
            {
                throw new Exception("Failed to update event order status");
            }
        }

        public async Task<EventOrderDetailDTO> CheckinProductStuatus(Guid orderDetailId)
        {
            //already checking order existence in calling method
            var result = await _unitOfWork.EventOrderRepository.CheckInStatusProduct(orderDetailId);
            if (result == null)
            {
                return null;
            }
            var existingOrder = await _unitOfWork.EventOrderRepository.GetByIdAsync(result.EventOrderId);

            if (existingOrder == null || existingOrder.Status != EventOrderStatusEnums.PAID.ToString())
            {
                throw new Exception("The order is no longer existing or has been completed or not paid");
            }

            if (!existingOrder.EventOrderDetails.Any(x => x.IsReceived == false))
            {
                existingOrder.Status = EventStatusEnums.COMPLETED.ToString();
            }

            if (await _unitOfWork.SaveChangeAsync() <= 0)
            {
                throw new Exception("Invalid error during update process");
            }

            return _mapper.Map<EventOrderDetailDTO>(result);
        }
    }
}