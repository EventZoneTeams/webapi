using Domain.Entities;
using Repositories.DTO;

namespace Repositories.Interfaces
{
    public interface IEventPackageRepository : IGenericRepository<EventPackage>
    {
        Task<List<ProductInPackage>> CreatePackageWithProducts(int eventId, string description, List<ProductQuantityDTO> productIds);
        Task<List<EventPackage>> GetAllPackgesByEventId(int eventId);
        Task<List<EventPackageDetailDTO>> GetAllPackageWithProducts();
        Task<List<ProductInPackage>> GetProductsInPackagesWithProduct();
        Task<List<EventPackageDetailDTO>> GetAllPackageWithProductsByEventId(int eventId);
    }
}
