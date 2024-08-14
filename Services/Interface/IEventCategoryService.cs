using EventZone.Domain.DTOs.EventCategoryDTOs;

namespace EventZone.Services.Interface
{
    public interface IEventCategoryService
    {
        Task<List<EventCategoryResponseDTO>> GetEventCategories(CategoryParam categoryParam);

        Task<EventCategoryResponseDTO> GetEventCategoryById(Guid id);

        Task<EventCategoryResponseDTO> CreateEventCategory(EventCategoryDTO eventCategoryModel);

        Task<EventCategoryResponseDTO> UpdateEventCategory(Guid id, EventCategoryDTO eventCategoryModel);

        Task<bool> DeleteEventCategory(Guid id);
    }
}