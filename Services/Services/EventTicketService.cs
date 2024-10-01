using AutoMapper;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventTicketService : IEventTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventTicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
            var eventProducts = await _unitOfWork.EventTicketRepository.GetAllAsync();

            result = _mapper.Map<List<EventTicketDetailDTO>>(eventProducts);

            return result;
        }

        public async Task<List<EventTicketDetailDTO>> GetAllTicketsByEventIdAsync(Guid eventId)
        {
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
            var eventProduct = await _unitOfWork.EventTicketRepository.GetByIdAsync(id, x => x.Event);

            if (eventProduct == null)
            {
                return null;
            }

            var result = _mapper.Map<EventTicketDetailDTO>(eventProduct);

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