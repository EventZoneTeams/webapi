using Domain.Entities;
using Repositories.Commons;
using Repositories.Models.ImageDTOs;
using Repositories.Models.ProductModels;

namespace Repositories.Interfaces
{
    public interface IEventProductRepository : IGenericRepository<EventProduct>
    {
        Task<List<ProductImage>> AddImagesForProduct(int productId, List<ImageReturnDTO> images);

        Task<List<EventProduct>> GetAllProductsByEvent(int eventId);

        Task<List<EventProduct>> GetAllProductsWithImages();
        Task<Pagination<EventProduct>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);
    }
}