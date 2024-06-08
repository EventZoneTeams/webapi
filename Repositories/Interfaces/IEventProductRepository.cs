using Domain.Entities;
using Repositories.Models.ImageDTOs;

namespace Repositories.Interfaces
{
    public interface IEventProductRepository : IGenericRepository<EventProduct>
    {
        Task<List<ProductImage>> AddImagesForProduct(int productId, List<ImageReturnDTO> images);

        Task<List<EventProduct>> GetAllProductsByEvent(int eventId);

        Task<List<EventProduct>> GetAllProductsWithImages();
    }
}