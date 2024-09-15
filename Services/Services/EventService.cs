using AutoMapper;
using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IClaimsService _claimsService;
        private readonly IRedisService _redisService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService, IClaimsService claimsService, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _claimsService = claimsService;
            _redisService = redisService;
        }

        public async Task<PagedList<Event>> GetEvent(EventParams eventParams)
        {
            var query = _unitOfWork.EventRepository.FilterAllField(eventParams).AsQueryable();

            var events = await PagedList<Event>.ToPagedList(query, eventParams.PageNumber, eventParams.PageSize);

            return events;
        }

        public async Task<EventResponseDTO> GetEventByIdOld(Guid id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id, x => x.EventCategory, x => x.User, x => x.EventCampaigns);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }

            var result = _mapper.Map<EventResponseDTO>(existingEvent);
            return result;
        }

        public async Task<EventResponseDTO> GetEventById(Guid id)
        {
            // Try to get from cache
            var cachedOrder = await _redisService.GetStringAsync(CacheKeys.Event(id));
            if (!string.IsNullOrEmpty(cachedOrder))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<EventResponseDTO>(cachedOrder);
            }

            // If not in cache, query the database
            var eventOrder = await _unitOfWork.EventRepository.GetByIdAsync(id, x => x.User);

            if (eventOrder == null)
            {
                throw new Exception("Event not found");
            }

            var result = _mapper.Map<EventResponseDTO>(eventOrder);

            // Cache the result
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _redisService.SetStringAsync(CacheKeys.Event(id), serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes

            return result;
        }

        public async Task<EventResponseDTO> CreateEvent(EventCreateDTO eventModel)
        {
            var eventEntity = _mapper.Map<Event>(eventModel);
            //check user
            Guid userId = _claimsService.GetCurrentUserId;
            var isExistUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (isExistUser == null)
            {
                throw new Exception("User does not exist!");
            }
            eventEntity.UserId = isExistUser.Id;
            eventEntity.User = isExistUser;
            //check eventCategory
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(eventModel.EventCategoryId);
            if (eventCategory == null)
            {
                throw new Exception("Event category not null when create event");
            }
            eventEntity.EventCategory = eventCategory;

            //eventEntity.Name = eventModel.Name;
            //eventEntity.Description = eventModel.Description;
            //eventEntity.ThumbnailUrl = eventModel.ThumbnailUrl;
            //eventEntity.EventStartDate = eventModel.EventStartDate;
            //eventEntity.EventEndDate = eventModel.EventEndDate;

            eventEntity.Status = EventStatusEnums.DRAFT.ToString();

            var newEvent = await _unitOfWork.EventRepository.AddAsync(eventEntity);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            var result = _mapper.Map<EventResponseDTO>(newEvent);

            if (isSuccess)
            {
                //    await _notificationService.PushNotificationToManager(new Notification
                //    {
                //        Title = "User " + isExistUser.Email + " Has Created Event",
                //        Body = $"Event Name: " + eventModel.Name,
                //        Url = "/dashboard/feedback/event/" + newEvent.Id,
                //    });

                // Clear cache as new category is added
                await _redisService.DeleteKeyAsync(CacheKeys.Events);
                return result;
            }
            else
            {
                throw new Exception("Failed to create event");
            }
        }

        public async Task<EventResponseDTO> UpdateEvent(Guid id, EventDTO eventModel)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }
            //check user
            //var user = await _unitOfWork.UserRepository.GetAllUsersAsync();
            //var isExistUser = user.FirstOrDefault(x => x.Id == eventModel.UserId);
            //if (isExistUser == null)
            //{
            //    throw new Exception("User does not exist!");
            //}
            //existingEvent.User = isExistUser;
            //check eventCategory
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(eventModel.EventCategoryId);
            if (eventCategory == null)
            {
                throw new Exception("Event category does not exist!");
            }
            existingEvent.EventCategory = eventCategory;

            existingEvent.Name = eventModel.Name ?? existingEvent.Name;
            existingEvent.Description = eventModel.Description ?? existingEvent.Description;
            existingEvent.ThumbnailUrl = eventModel.ThumbnailUrl ?? existingEvent.ThumbnailUrl;
            existingEvent.EventStartDate = eventModel.EventStartDate ?? existingEvent.EventStartDate;
            existingEvent.EventEndDate = eventModel.EventEndDate ?? existingEvent.EventEndDate;
            existingEvent.Status = eventModel.Status.ToString() ?? existingEvent.Status;

            var isUpdated = await _unitOfWork.EventRepository.Update(existingEvent);

            if (isUpdated == false)
            {
                throw new Exception("Failed to update event");
            }

            await _unitOfWork.SaveChangeAsync();
            // Clear specific cache key
            await _redisService.DeleteKeyAsync(CacheKeys.Event(id));
            // Clear general list cache
            await _redisService.DeleteKeyAsync(CacheKeys.Events);
            var result = _mapper.Map<EventResponseDTO>(existingEvent);
            return result;
        }

        public async Task<EventResponseDTO> DeleteEvent(Guid id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (existingEvent == null)
            {
                throw new Exception("Event not found");
            }

            var isDeleted = await _unitOfWork.EventRepository.SoftRemove(existingEvent);
            if (isDeleted)
            {
                var result = _mapper.Map<EventResponseDTO>(existingEvent);
                await _unitOfWork.SaveChangeAsync();
                // Clear specific cache key
                await _redisService.DeleteKeyAsync(CacheKeys.Event(id));
                // Clear general list cache
                await _redisService.DeleteKeyAsync(CacheKeys.Events);
                return result;
            }
            else
            {
                throw new Exception("Failed to delete event");
            }
        }
    }
}