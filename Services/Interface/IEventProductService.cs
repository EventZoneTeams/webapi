using Domain.DTOs.ImageDTOs;
using Repositories.Commons;
using Repositories.Models.ProductModels;
using Services.DTO.EventProductsModel;

namespace Services.Interface
{
    public interface IEventProductService
    {
        Task<ApiResult<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct, List<ImageReturnDTO> images);

        Task<List<EventProductDetailModel>> GetAllProductsAsync();

        Task<ApiResult<EventProductDetailModel>> UpdateEventProductAsync(Guid productId, EventProductUpdateModel updateModel);

        Task<ApiResult<List<EventProductDetailModel>>> DeleteEventProductAsync(List<Guid> productIds);

        Task<ApiResult<List<EventProductDetailModel>>> CreateEventProductAsync(List<EventProductCreateModel> newProducts);

        Task<List<EventProductDetailModel>> GetAllProductsByEventAsync(Guid eventId);

        Task<Pagination<EventProductDetailModel>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);

        Task<ApiResult<EventProductDetailModel>> GetProductById(Guid productId);

        Task<ApiResult<EventProductDetailModel>> DeleteEventProductByIdAsync(Guid id);
    }
}