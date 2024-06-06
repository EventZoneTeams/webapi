using AutoMapper;
using Domain.Entities;
using Repositories.Helper;
using Repositories.Interfaces;
using Services.DTO.EventModels;
using Services.Interface;

namespace Services.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedList<Event>> GetEvent(EventParams eventParams)
        {
            var query = _unitOfWork.EventRepository.FilterAllField(eventParams).AsQueryable();

            var products = await PagedList<Event>.ToPagedList(query, eventParams.PageNumber, eventParams.PageSize);

            //.Sort(eventParams.OrderBy);

            return products;
        }

        public async Task<ResponseEventModel> GetEventById(int id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }

            var result = _mapper.Map<ResponseEventModel>(existingEvent);
            return result;
        }
        public async Task<ResponseEventModel> CreateEvent(EventModel eventModel)
        {
            var eventEntity = _mapper.Map<Event>(eventModel);
            //check user
            var user = await _unitOfWork.UserRepository.GetAllUsersAsync();
            var isExist = user.FirstOrDefault(x => x.Id == eventModel.UserId);
            if (isExist == null)
            {
                throw new Exception("User not null when create event");
            }

            //check eventCategory
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(eventModel.EventCategoryId);
            if (eventCategory == null)
            {
                throw new Exception("Event category not null when create event");
            }
            eventEntity.EventCategory = eventCategory;

            var newEvent = await _unitOfWork.EventRepository.AddAsync(eventEntity);

            var result = _mapper.Map<ResponseEventModel>(newEvent);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<ResponseEventModel> UpdateEvent(int id, EventModel eventModel)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }

            existingEvent.Name = eventModel.Name ?? existingEvent.Name;
            existingEvent.Description = eventModel.Description ?? existingEvent.Description;
            existingEvent.ThumbnailUrl = eventModel.ThumbnailUrl ?? existingEvent.ThumbnailUrl;
            existingEvent.DonationStartDate = eventModel.DonationStartDate ?? existingEvent.DonationStartDate;
            existingEvent.DonationEndDate = eventModel.DonationEndDate ?? existingEvent.DonationEndDate;
            existingEvent.EventStartDate = eventModel.EventStartDate ?? existingEvent.EventStartDate;
            existingEvent.EventEndDate = eventModel.EventEndDate ?? existingEvent.EventEndDate;
            existingEvent.Location = eventModel.Location ?? existingEvent.Location;
            if (eventModel.UserId != 0)
            {
                existingEvent.UserId = eventModel.UserId;
            }
            if (eventModel.EventCategoryId != 0)
            {
                existingEvent.EventCategoryId = eventModel.EventCategoryId;
            }
            existingEvent.University = eventModel.University ?? existingEvent.University;
            existingEvent.Status = eventModel.Status.ToString() ?? existingEvent.Status;
            existingEvent.OriganizationStatus = eventModel.OriganizationStatus.ToString() ?? existingEvent.OriganizationStatus;
            existingEvent.IsDonation = eventModel.IsDonation ?? existingEvent.IsDonation;
            existingEvent.TotalCost = eventModel.TotalCost ?? existingEvent.TotalCost;

            var isUpdated = await _unitOfWork.EventRepository.Update(existingEvent);

            if (isUpdated == false)
            {
                throw new Exception("Failed to update event");
            }

            var result = _mapper.Map<ResponseEventModel>(existingEvent);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }
        public async Task<ResponseEventModel> DeleteEvent(int id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }

            var isDeleted = await _unitOfWork.EventRepository.SoftRemove(existingEvent);
            if (isDeleted)
            {
                var result = _mapper.Map<ResponseEventModel>(existingEvent);
                await _unitOfWork.SaveChangeAsync();
                return result;
            }
            else
            {
                throw new Exception("Failed to delete event");
            }
        }
    }
}
