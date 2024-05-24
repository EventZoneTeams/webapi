using AutoMapper;
using Repositories.Interfaces;
using Services.BusinessModels.EventCategoryModels;
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

        public async Task<EventCategoryModel> CreateEventCategory(CreateEventCategoryModel eventCategoryModel)
        {
            return null;

        }

        public async Task<EventCategoryModel> DeleteEventCategory(int id)
        {
            return null;
        }

        public async Task<List<EventCategoryModel>> GetEventCategories()
        {
            var eventCategories = await _unitOfWork.EventCategoryRepository.GetAllAsync();
            var result = new List<EventCategoryModel>();
            foreach (var category in eventCategories)
            {
                var eventFormat = _mapper.Map<EventCategoryModel>(category);
                result.Add(eventFormat);
            }
            return result;
        }

        public async Task<EventCategoryModel> GetEventCategoryById(int id)
        {
            //var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            //var eventCategoryModel = _mapper.Map<EventCategoryModel>(eventCategory);
            //return eventCategoryModel;

            return null;
        }

        public async Task<EventCategoryModel> UpdateEventCategory(int id, CreateEventCategoryModel eventCategoryModel)
        {
            return null;
        }
    }
}
