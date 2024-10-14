using AutoMapper;
using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.ProductModels;
using EventZone.Repositories.Models.TicketModels;
using EventZone.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IClaimsService _claimsService;
        private readonly IWalletService _walletService;

        public AttendeeService(IUnitOfWork unitOfWork, IMapper mapper, IRedisService redisService, IClaimsService claimsService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
            _claimsService = claimsService;
            _walletService = walletService;
        }

        public async Task<List<BookedTicketDetailDTO>> BookANewTicketForEvent(BookedTicketRequestDTO bookedTicketDTO)
        {
            try
            {
                //check existing event
                var existingTicket = await _unitOfWork.EventTicketRepository.GetByIdAsync(bookedTicketDTO.EventTicketId);
                var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(bookedTicketDTO.EventId);

                if (existingTicket == null)
                {
                    throw new Exception("Event ticket is not existed");
                }
                if (existingEvent == null)
                {
                    throw new Exception("Event is not existed");
                }

                var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
                if (user == null)
                {
                    throw new Exception("User is not sign-in");
                }
                // create new order for purchase ticket

                var newOrder = new EventOrder
                {
                    EventId = existingEvent.Id,
                    UserId = user.Id,
                    TotalAmount = bookedTicketDTO.Quantity * existingTicket.Price,
                    OrderType = "TICKET",
                    Status = EventOrderStatusEnums.PENDING.ToString(),
                    BookedTickets = []
                };
                newOrder = await _unitOfWork.EventOrderRepository.AddAsync(newOrder);
                var saveCheck = await _unitOfWork.SaveChangeAsync();
                // update user wallet amount
                var walletTransaction = await _walletService.PurchaseOrder(newOrder.Id, user.Id);
                // purchase successfully, create booked ticket
                var newBookedTicketList = new List<BookedTicket>();
                if (saveCheck > 0)
                {
                    for (int i = 0; i < bookedTicketDTO.Quantity; i++)
                    {
                        var newBookedTicket = new BookedTicket
                        {
                            EventOrderId = newOrder.Id,
                            EventTicketId = bookedTicketDTO.EventTicketId,
                            EventId = bookedTicketDTO.EventId,
                            UserId = user.Id,
                            PaidPrice = existingTicket.Price,
                            AttendeeNote = "Ticket " + existingTicket.Name + " no:" + i,
                        };

                        newBookedTicketList.Add(newBookedTicket);
                        newOrder.BookedTickets.Add(newBookedTicket);
                    }
                    existingTicket.InStock -= bookedTicketDTO.Quantity; // update stock quantity
                    await _unitOfWork.EventTicketRepository.Update(existingTicket);
                    var result = await _unitOfWork.AttendeeRepository.AddRangeAsync(newBookedTicketList);

                    saveCheck = await _unitOfWork.SaveChangeAsync();
                    if (saveCheck > 0)
                    {
                        return _mapper.Map<List<BookedTicketDetailDTO>>(result);
                    }
                    else
                    {
                        throw new Exception("Error while attempting to add new booked ticket");
                    }
                }
                else
                {
                    throw new Exception("Error while attempting to add new order");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BookedTicketDetailDTO>> GetAllBookedTickets()
        {
            List<BookedTicketDetailDTO> result;

            // Bước 1: Kiểm tra cache
            var cachedBooked = await _redisService.GetStringAsync(CacheKeys.BookedTickets);
            if (!string.IsNullOrEmpty(cachedBooked))
            {
                // Nếu cache tồn tại, giải mã và sử dụng dữ liệu từ cache
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BookedTicketDetailDTO>>(cachedBooked);
            }
            else
            {
                // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
                var bookedTickets = await _unitOfWork.AttendeeRepository.GetAllBookedTickets();

                result = _mapper.Map<List<BookedTicketDetailDTO>>(bookedTickets);

                // Lưu kết quả vào cache
                var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                await _redisService.SetStringAsync(CacheKeys.BookedTickets, serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            }

            return result;
        }

        public async Task<List<BookedTicketDetailDTO>> GetAllBookedTicketByOrderID(Guid orderId)
        {
            var order = await _unitOfWork.EventOrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            List<BookedTicketDetailDTO> result = await GetAllBookedTickets();

            result = result
                .Where(x => x.EventOrderId == orderId)
                .ToList();

            return result;
        }

        public async Task<EventOrderBookedTicketDTO> GetEventOrderWithTicket(Guid orderId)
        {
            return _mapper.Map<EventOrderBookedTicketDTO>(await _unitOfWork.AttendeeRepository.GetOrderTicket(orderId));
        }

        public async Task<EventOrderBookedTicketDTO> GetAllBookedTicketOfCurrentUser()
        {
            var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (user == null)
            {
                throw new Exception("User is not sign-in");
            }
            return _mapper.Map<EventOrderBookedTicketDTO>(await _unitOfWork.AttendeeRepository.GetAllBookedTicketsOfUser(user.Id));
        }

        public async Task<ApiResult<BookedTicketDetailDTO>> UpdateBookedAsync(Guid bookedId, BookedTicketUpdateDTO updateModel)
        {
            var existingBookedTicket = await _unitOfWork.AttendeeRepository.GetByIdAsync(bookedId);
            if (existingBookedTicket != null)
            {
                existingBookedTicket = _mapper.Map(updateModel, existingBookedTicket);
                await _unitOfWork.AttendeeRepository.Update(existingBookedTicket);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    // Clear specific cache key
                    await _redisService.DeleteKeyAsync(CacheKeys.BookedTicket(bookedId));
                    // Clear general list cache
                    await _redisService.DeleteKeyAsync(CacheKeys.BookedTickets);

                    return new ApiResult<BookedTicketDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<BookedTicketDetailDTO>(existingBookedTicket)
                    };
                }
                else
                {
                    throw new Exception("Something wrong in the updating process");
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<ApiResult<BookedTicketDetailDTO>> CheckinBookedAsync(Guid bookedId)
        {
            var existingBookedTicket = await _unitOfWork.AttendeeRepository.GetByIdAsync(bookedId);
            if (existingBookedTicket != null)
            {
                existingBookedTicket.IsCheckedIn = existingBookedTicket.IsCheckedIn ? false : true;
                await _unitOfWork.AttendeeRepository.Update(existingBookedTicket);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    // Clear specific cache key
                    await _redisService.DeleteKeyAsync(CacheKeys.BookedTicket(bookedId));
                    // Clear general list cache
                    await _redisService.DeleteKeyAsync(CacheKeys.BookedTickets);

                    return new ApiResult<BookedTicketDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<BookedTicketDetailDTO>(existingBookedTicket)
                    };
                }
                else
                {
                    throw new Exception("Something wrong in the updating process");
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Pagination<BookedTicketDetailDTO>> GetBookedsByFiltersAsync(PaginationParameter paginationParameter, BookedTicketFilterModel bookedTicketFilterModel)
        {
            var products = await _unitOfWork.AttendeeRepository.GetBookedTicketsByFilterAsync(paginationParameter, bookedTicketFilterModel);
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (products != null)
            {
                var result = _mapper.Map<List<BookedTicketDetailDTO>>(products);
                return new Pagination<BookedTicketDetailDTO>(result, products.TotalCount, products.CurrentPage, products.PageSize);
            }
            return null;
        }
    }
}