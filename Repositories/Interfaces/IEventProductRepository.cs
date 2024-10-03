using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.ProductModels;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventProductRepository : IGenericRepository<EventProduct>
    {
        Task<List<ProductImage>> AddImagesForProduct(Guid productId, List<ImageReturnDTO> images);

        Task<List<ProductImage>> AddImagesStringForProduct(Guid productId, List<string> images);

        Task<List<EventProduct>> GetAllProductsByEvent(Guid eventId);

        Task<List<EventProduct>> GetAllProductsWithImages();
        Task<ProductImage> GetProductImageByUrl(string url);
        Task<Pagination<EventProduct>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel);
    }
}