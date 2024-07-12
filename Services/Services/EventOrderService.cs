using AutoMapper;
using Domain.Entities;
using Domain.Enums;
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

		public EventOrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_claimsService = claimsService;
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

		public async Task<List<EventOrderReponseDTO>> GetEventOrders(int eventId)
		{
			var orders = await _unitOfWork.EventOrderRepository.GetEventOrdersByEventId(eventId);
			return _mapper.Map<List<EventOrderReponseDTO>>(orders);
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

			//newOrder = await _unitOfWork.EventOrderRepository.AddAsync(newOrder);
			return _mapper.Map<EventOrderReponseDTO>(orderResponse);
		}

		public async Task<EventOrderReponseDTO> UpdateOrderStatus(int orderId, EventOrderStatusEnums eventOrderStatusEnums)
		{
			var order = await _unitOfWork.EventOrderRepository.UpdateOrderStatus(orderId, eventOrderStatusEnums);
			return _mapper.Map<EventOrderReponseDTO>(order);
		}
	}
}