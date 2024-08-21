using AutoMapper;
using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Interfaces;
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

        public AttendeeService(IUnitOfWork unitOfWork, IMapper mapper, IRedisService redisService, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
            _claimsService = claimsService;
        }

        public async Task<List<BookedTicketDetailDTO>> BookANewTicketForEvent(BookedTicketDTO bookedTicketDTO)
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
                var newOrder = new EventOrder
                {
                    EventId = existingEvent.Id,
                    UserId = user.Id,
                    TotalAmount = bookedTicketDTO.Quantity.Value * existingTicket.Price,
                    OrderType = "TICKET",
                    Status = EventOrderStatusEnums.PENDING.ToString(),
                };

                newOrder = await _unitOfWork.EventOrderRepository.AddAsync(newOrder);
                var saveCheck = await _unitOfWork.SaveChangeAsync();
                var newBookedTicketList = new List<BookedTicket>();
                if (saveCheck > 0)
                {
                    for (int i = 0; i < bookedTicketDTO.Quantity; i++)
                    {
                        var newBookedTicketDTO = new BookedTicket
                        {
                            EventOrderId = newOrder.Id,
                            EventTicketId = bookedTicketDTO.EventTicketId,
                            EventId = bookedTicketDTO.EventId,
                            UserId = user.Id,
                            PaidPrice = existingTicket.Price,
                            AttendeeNote = "Ticket " + existingTicket.Name + " no:" + i,
                        };

                        newBookedTicketList.Add(newBookedTicketDTO);
                    }
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

        public async Task<List<BookedTicketDetailDTO>> GetAllBookedTicket()
        {
            return _mapper.Map<List<BookedTicketDetailDTO>>(await _unitOfWork.AttendeeRepository.GetAllAsync());
        }
    }
}