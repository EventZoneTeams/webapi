using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.ProductModels;

namespace EventZone.Services.Interface
{
    public interface IEventProductService
    {
        Task<ApiResult<EventProductDetailDTO>> CreateEventProductAsync(EventProductCreateDTO newProduct, List<ImageReturnDTO> images);

        Task<List<EventProductDetailDTO>> GetAllProductsAsync();

        Task<ApiResult<EventProductDetailDTO>> UpdateEventProductAsync(Guid productId, EventProductUpdateDTO updateModel);

        // Task<ApiResult<List<EventProductDetailDTO>>> DeleteEventProductAsync(List<Guid> productIds);

        Task<ApiResult<List<EventProductDetailDTO>>> CreateEventProductAsync(List<EventProductCreateDTO> newProducts);

        Task<List<EventProductDetailDTO>> GetAllProductsByEventAsync(Guid eventId);

        Task<Pagination<EventProductDetailDTO>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);

        Task<ApiResult<EventProductDetailDTO>> GetProductById(Guid productId);

        Task<ApiResult<EventProductDetailDTO>> DeleteEventProductByIdAsync(Guid id);
    }
}