using Repositories.Commons;
using Repositories.Models;
using Repositories.Models.ImageDTOs;
using Repositories.Models.ProductModels;
using Services.DTO.EventProductsModel;
using Services.DTO.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventProductService
    {
        Task<ApiResult<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct, List<ImageReturnDTO> images);

        Task<List<EventProductDetailModel>> GetAllProductsAsync();

        Task<ApiResult<EventProductDetailModel>> UpdateEventProductAsync(int productId, EventProductUpdateModel updateModel);

        Task<ApiResult<List<EventProductDetailModel>>> DeleteEventProductAsync(List<int> productIds);

        Task<ApiResult<List<EventProductDetailModel>>> CreateEventProductAsync(List<EventProductCreateModel> newProducts);

        Task<List<EventProductDetailModel>> GetAllProductsByEventAsync(int eventId);

        Task<Pagination<EventProductDetailModel>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);

        Task<ApiResult<EventProductDetailModel>> GetProductById(int productId);

        Task<ApiResult<EventProductDetailModel>> DeleteEventProductByIdAsync(int id);
    }
}