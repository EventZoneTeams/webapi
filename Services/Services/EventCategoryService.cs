using AutoMapper;
using Domain.DTOs.EventCategoryDTOs;
using Domain.Entities;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interface;

namespace Services.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            return isDeleted;
        }

        public async Task<List<EventCategoryResponseDTO>> GetEventCategories(CategoryParam categoryParam)
        {
            var eventCategories = await _unitOfWork.EventCategoryRepository
                .GetQueryable()
                .Search(categoryParam.SearchTerm)
                .Sort(categoryParam.OrderBy)
                .ToListAsync<EventCategory>();

            return _mapper.Map<List<EventCategoryResponseDTO>>(eventCategories);
        }

        public async Task<EventCategoryResponseDTO> GetEventCategoryById(Guid id)
        {
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(id);

            if (eventCategory == null)
            {
                throw new Exception("Event category not found");
            }

            var result = _mapper.Map<EventCategoryResponseDTO>(eventCategory);

            return result;
        }

        public async Task<EventCategoryResponseDTO> UpdateEventCategory(Guid id, EventCategoryDTO eventCategoryModel)
        {
            var eventCategory = await _unitOfWork.EventCategoryRepository.GetByIdAsync(id);

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

            var result = _mapper.Map<EventCategoryResponseDTO>(eventCategory);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }
    }
}