using Services.BusinessModels.EventCategoryModels;

namespace Services.Interface
{
    public interface IEventCategoryService
    {
        Task<List<EventCategoryModel>> GetEventCategories(CategoryParam categoryParam);
        Task<EventCategoryModel> GetEventCategoryById(int id);
        Task<EventCategoryModel> CreateEventCategory(EventCategoryModel eventCategoryModel);
        Task<EventCategoryModel> UpdateEventCategory(int id, CreateEventCategoryModel eventCategoryModel);
        Task<EventCategoryModel> DeleteEventCategory(int id);
    }
}
