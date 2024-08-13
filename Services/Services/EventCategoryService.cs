using AutoMapper;
using Domain.DTOs.EventCategoryDTOs;
using Domain.Entities;
using Repositories.Helper;
using Repositories.Interfaces;
using Services.Interface;

namespace Services.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;

        public EventCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _redisService = redisService;
        }

        public async Task<EventCategoryResponseDTO> CreateEventCategory(EventCategoryDTO eventCategoryModel)
        {
            // check if event category already exists
            var existingCategory = await _unitOfWork.EventCategoryRepository
                .GetAllAsync();

            var isExist = existingCategory.FirstOrDefault(x => x.Title.ToLower() == eventCategoryModel.Title.ToLower());

            if (isExist != null)
            {
                throw new Exception("Event category already exists");
            }

            // create new event category
            var eventCategory = new EventCategory
            {
                Title = eventCategoryModel.Title,
                ImageUrl = eventCategoryModel.ImageUrl,
                Description = eventCategoryModel.Description,
            };
            var newCategory = await _unitOfWork.EventCategoryRepository.AddAsync(eventCategory);

            // Clear cache as new category is added
            await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);

            // mapper
            var result = _mapper.Map<EventCategoryResponseDTO>(newCategory);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<bool> DeleteEventCategory(Guid id)
        {
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(id);

            if (eventCategory == null)
            {
                throw new Exception("Event category not found");
            }

            var isDeleted = await _unitOfWork.EventCategoryRepository.SoftRemove(eventCategory);

            if (isDeleted)
            {
                // Clear specific cache key
                await _redisService.DeleteKeyAsync(CacheKeys.EventCategory(id));
                // Clear general list cache
                await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);
            }

            return isDeleted;
        }

        public async Task<List<EventCategoryResponseDTO>> GetEventCategories(CategoryParam categoryParam)
        {
            List<EventCategoryResponseDTO> result;

            // Bước 1: Kiểm tra cache
            var cachedCategories = await _redisService.GetStringAsync(CacheKeys.EventCategories);
            if (!string.IsNullOrEmpty(cachedCategories))
            {
                // Nếu cache tồn tại, giải mã và sử dụng dữ liệu từ cache
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventCategoryResponseDTO>>(cachedCategories);
            }
            else
            {
                // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
                var eventCategories = await _unitOfWork.EventCategoryRepository.GetAllAsync();

                result = _mapper.Map<List<EventCategoryResponseDTO>>(eventCategories);

                // Lưu kết quả vào cache
                var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                await _redisService.SetStringAsync(CacheKeys.EventCategories, serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            }

            // Bước 2: Áp dụng tìm kiếm và sắp xếp trên kết quả
            if (!string.IsNullOrEmpty(categoryParam.SearchTerm))
            {
                result = result
                    .Where(x => x.Title.ToLower().Contains(categoryParam.SearchTerm.ToLower()))
                    .ToList();
            }

            result = result.ToList();

            return result;
        }

        public async Task<EventCategoryResponseDTO> GetEventCategoryById(Guid id)
        {
            // Try to get from cache
            var cachedCategory = await _redisService.GetStringAsync(CacheKeys.EventCategory(id));
            if (!string.IsNullOrEmpty(cachedCategory))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<EventCategoryResponseDTO>(cachedCategory);
            }

            // If not in cache, query the database
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(id);

            if (eventCategory == null)
            {
                throw new Exception("Event category not found");
            }

            var result = _mapper.Map<EventCategoryResponseDTO>(eventCategory);

            // Cache the result
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _redisService.SetStringAsync(CacheKeys.EventCategory(id), serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes

            return result;
        }

        public async Task<EventCategoryResponseDTO> UpdateEventCategory(Guid id, EventCategoryDTO eventCategoryModel)
        {
            var eventCategory = _mapper.Map<EventCategory>(await GetEventCategoryById(id));

            if (eventCategory == null)
            {
                throw new Exception("Event category not found");
            }

            eventCategory.Title = eventCategoryModel.Title;
            eventCategory.ImageUrl = eventCategoryModel?.ImageUrl ?? eventCategory?.ImageUrl;
            eventCategory.Description = eventCategoryModel?.Description ?? eventCategory?.Description;

            var isUpdated = await _unitOfWork.EventCategoryRepository.Update(eventCategory);

            if (isUpdated == false)
            {
                throw new Exception("Failed to update event category");
            }

            // Clear specific cache key
            await _redisService.DeleteKeyAsync(CacheKeys.EventCategory(id));
            // Clear general list cache
            await _redisService.DeleteKeyAsync(CacheKeys.EventCategories);

            var result = _mapper.Map<EventCategoryResponseDTO>(eventCategory);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }
    }
}
