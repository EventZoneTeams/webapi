using AutoMapper;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Services
{
    public class EventTicketService : IEventTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;

        public EventTicketService(IUnitOfWork unitOfWork, IMapper mapper, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
        }

        public async Task<EventTicketDetailDTO> CreateNewTicketAsync(EventTicketDTO createTicket)
        {
            //check existing event
            if (await _unitOfWork.EventRepository.GetByIdAsync(createTicket.EventId) == null)
            {
                throw new Exception("Event is not existed");
            }

            var newTicket = new EventTicket();
            newTicket = _mapper.Map(createTicket, newTicket);

            //executing
            var result = await _unitOfWork.EventTicketRepository.AddAsync(newTicket);
            var check = await _unitOfWork.SaveChangeAsync();
            if (check > 0)
            {
                await _redisService.DeleteKeyAsync(CacheKeys.EventProducts);
                return _mapper.Map<EventTicketDetailDTO>(result);
            }
            else
            {
                throw new Exception("Add process has been failed due to errors");
            }
        }

        public async Task<List<EventTicketDetailDTO>> GetAllTicketsAsync()
        {
            List<EventTicketDetailDTO> result;

            // Bước 1: Kiểm tra cache
            var cachedTickets = await _redisService.GetStringAsync(CacheKeys.EventTickets);
            if (!string.IsNullOrEmpty(cachedTickets))
            {
                // Nếu cache tồn tại, giải mã và sử dụng dữ liệu từ cache
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventTicketDetailDTO>>(cachedTickets);
            }
            else
            {
                // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
                var eventProducts = await _unitOfWork.EventTicketRepository.GetAllAsync();

                result = _mapper.Map<List<EventTicketDetailDTO>>(eventProducts);

                // Lưu kết quả vào cache
                var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                await _redisService.SetStringAsync(CacheKeys.EventTickets, serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            }

            return result;
        }

        public async Task<List<EventTicketDetailDTO>> GetAllTicketsByEventIdAsync(Guid eventId)
        {
            //check existing event
            if (await _unitOfWork.EventRepository.GetByIdAsync(eventId) == null)
            {
                return null; // handle not found event exception
            }
            List<EventTicketDetailDTO> result = await GetAllTicketsAsync();

            result = result
                .Where(x => x.EventId == eventId)
                .ToList();

            return result;
        }

        public async Task<ApiResult<EventTicketDetailDTO>> UpdateEventTicketAsync(Guid ticketId, EventTicketUpdateDTO updateModel)
        {
            var existingTicket = await _unitOfWork.EventTicketRepository.GetByIdAsync(ticketId);
            if (existingTicket != null)
            {
                existingTicket = _mapper.Map(updateModel, existingTicket);
                // existingTicket.InStock = updateModel.InStock == 0 ? existingTicket.InStock : updateModel.InStock;
                await _unitOfWork.EventTicketRepository.Update(existingTicket);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    return new ApiResult<EventTicketDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<EventTicketDetailDTO>(existingTicket)
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

        public async Task<ApiResult<EventTicketDetailDTO>> GetTicketById(Guid id)
        {
            // Try to get from cache
            var cachedTicket = await _redisService.GetStringAsync(CacheKeys.EventTicket(id));
            if (!string.IsNullOrEmpty(cachedTicket))
            {
                var product = Newtonsoft.Json.JsonConvert.DeserializeObject<EventTicketDetailDTO>(cachedTicket);
                return new ApiResult<EventTicketDetailDTO>()
                {
                    IsSuccess = true,
                    Message = "Found successfully product " + id,
                    Data = product
                };
            }

            // If not in cache, query the database
            var eventProduct = await _unitOfWork.EventTicketRepository.GetByIdAsync(id, x=> x.Event);

            if (eventProduct == null)
            {
                return null;
            }

            var result = _mapper.Map<EventTicketDetailDTO>(eventProduct);

            // Cache the result
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _redisService.SetStringAsync(CacheKeys.EventProduct(id), serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            return new ApiResult<EventTicketDetailDTO>()
            {
                IsSuccess = true,
                Message = "Found successfully product " + id,
                Data = result
            };
        }

        public async Task<ApiResult<EventTicketDetailDTO>> DeleteEventTicketByIdAsync(Guid id)
        {
            var ticket = await _unitOfWork.EventTicketRepository.GetByIdAsync(id);

            if (ticket != null)
            {
                await _unitOfWork.EventTicketRepository.SoftRemove(ticket);
                //save changes
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    // Clear specific cache key
                    await _redisService.DeleteKeyAsync(CacheKeys.EventTicket(id));
                    // Clear general list cache
                    await _redisService.DeleteKeyAsync(CacheKeys.EventTickets);

                    return ApiResult<EventTicketDetailDTO>
                        .Succeed(_mapper.Map<EventTicketDetailDTO>(ticket), "Product " + id + " Removed successfully");
                }
                else
                {
                    throw new Exception("Something wrong in the deleting process");
                }
            }
            return null;
        }
    }
}