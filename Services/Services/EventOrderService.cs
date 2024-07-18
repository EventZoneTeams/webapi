using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;
using Repositories.Interfaces;
using Services.DTO.EventOrderDTOs;
using Services.Interface;

namespace Services.Services
{
    public class EventOrderService : IEventOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly INotificationService _notificationService;

        public EventOrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _notificationService = notificationService;
        }

        public async Task<EventOrderReponseDTO> GetEventOrder(int orderId)
        {
            var order = await _unitOfWork.EventOrderRepository.GetByIdAsync(orderId, x => x.EventOrderDetails);
            if (order == null)
            {
                throw new Exception("Event Order not found");
            }
            return _mapper.Map<EventOrderReponseDTO>(order);
        }

        public async Task<PagedList<EventOrder>> GetEventOrders(int eventId, OrderParams orderParams)
        {
            var query = _unitOfWork.EventOrderRepository.FilterAllField(eventId, orderParams).AsQueryable();
            var eventOrders = await PagedList<EventOrder>.ToPagedList(query, orderParams.PageNumber, orderParams.PageSize);

            return eventOrders;
        }

        public async Task<EventOrderReponseDTO> CreateEventOrder(CreateEventOrderReponseDTO order)
        {
            var currentUser = _claimsService.GetCurrentUserId;
            if (currentUser == -1) throw new Exception("please login");

            var eventObject = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId);
            if (eventObject == null)
            {
                throw new Exception("Event not found");
            }

            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            var userObject = users.FirstOrDefault(x => x.Id == currentUser);
            if (userObject == null)
            {
                throw new Exception("User not found");
            }

            var newOrderDetailsList = _mapper.Map<List<EventOrderDetail>>(order.EventOrderDetails);
            var orderResponse = await _unitOfWork.EventOrderRepository.CreateOrderWithOrderDetails(order.EventId, currentUser, newOrderDetailsList);

            // Send notification
            var notificationToUser = new Notification
            {
                Body = "You create a new order: " + orderResponse.Id,
                Title = "Order Created!",
                UserId = currentUser,
                Url = "/profile/orders",
                Sender = "System"
            };

            //Send notification to event owner
            var eventDetails = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId, x => x.User);
            var notificationToEventOwner = new Notification
            {
                Body = "You have a new order " + orderResponse.Id + " from user: " + currentUser,
                Title = "1 Order Created!",
                UserId = eventDetails.UserId,
                Url = "/dashboard/my-events/" + order.EventId + "/orders",
                Sender = "System"
            };

            await _notificationService.PushNotification(notificationToUser).ConfigureAwait(true);
            await _notificationService.PushNotification(notificationToEventOwner).ConfigureAwait(true);

            return _mapper.Map<EventOrderReponseDTO>(orderResponse);
        }

        public async Task<EventOrderReponseDTO> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums)
        {
            var order = await _unitOfWork.EventOrderRepository.UpdateOrderStatus(orderId, eventOrderStatusEnums);
            return _mapper.Map<EventOrderReponseDTO>(order);
        }
    }
}