using Domain.DTOs.EventCategoryDTOs;
using Services.DTO.EventCategoryDTOs;

namespace Services.Interface
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