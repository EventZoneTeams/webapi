using AutoMapper;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Domain.Entities;
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
    }
}