using Services.BusinessModels.EventCategoryModels;

namespace Services.Interface
{
    public interface IEventCategoryService
    {
        Task<List<EventCategoryModel>> GetEventCategories();
        Task<EventCategoryModel> GetEventCategoryById(int id);
        Task<EventCategoryModel> CreateEventCategory(CreateEventCategoryModel eventCategoryModel);
        Task<EventCategoryModel> UpdateEventCategory(int id, CreateEventCategoryModel eventCategoryModel);
        Task<EventCategoryModel> DeleteEventCategory(int id);
    }
}
