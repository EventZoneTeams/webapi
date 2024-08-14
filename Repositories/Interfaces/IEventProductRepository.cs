using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.ProductModels;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventProductRepository : IGenericRepository<EventProduct>
    {
        Task<List<ProductImage>> AddImagesForProduct(Guid productId, List<ImageReturnDTO> images);

        Task<List<EventProduct>> GetAllProductsByEvent(Guid eventId);

        Task<List<EventProduct>> GetAllProductsWithImages();

        Task<Pagination<EventProduct>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);
    }
}