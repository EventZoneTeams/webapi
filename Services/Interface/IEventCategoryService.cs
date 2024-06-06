using Services.DTO.EventCategoryDTOs;

namespace Services.Interface
{
    public interface IEventCategoryService
    {
        Task<List<EventCategoryResponseDTO>> GetEventCategories(CategoryParam categoryParam);
        Task<EventCategoryResponseDTO> GetEventCategoryById(int id);
        Task<EventCategoryResponseDTO> CreateEventCategory(EventCategoryDTO eventCategoryModel);
        Task<EventCategoryResponseDTO> UpdateEventCategory(int id, EventCategoryDTO eventCategoryModel);
        Task<bool> DeleteEventCategory(int id);
    }
}
