using AutoMapper;
using Repositories.Entities;
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

        public async Task<EventCategoryModel> CreateEventCategory(EventCategoryModel eventCategoryModel)
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
                ImageUrl = eventCategoryModel.ImageUrl
            };
            var newCategory = await _unitOfWork.EventCategoryRepository.AddAsync(eventCategory);

            // mapper
            var result = _mapper.Map<EventCategoryModel>(newCategory);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<EventCategoryModel> DeleteEventCategory(int id)
        {
            return null;
        }

        public async Task<List<EventCategoryModel>> GetEventCategories(CategoryParam? categoryParam)
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
