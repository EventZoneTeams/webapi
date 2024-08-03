using Domain.Entities;
using Repositories.Commons;
using Repositories.Models;
using Repositories.Models.PackageModels;

namespace Repositories.Interfaces
{
    public interface IEventPackageRepository : IGenericRepository<EventPackage>
    {
        Task<List<ProductInPackage>> CreatePackageWithProducts(Guid eventId, string description, string thumbnailUr, List<ProductQuantityDTO> productIds, string title);

        Task<List<EventPackage>> GetAllPackgesByEventId(Guid eventId);

        Task<List<EventPackageDetailDTO>> GetAllPackageWithProducts();

        Task<List<ProductInPackage>> GetProductsInPackagesWithProduct();

        Task<List<EventPackage>> GetAllPackageWithProductsByEventId(Guid eventId);

        Task<Pagination<EventPackage>> GetPackagessByFiltersAsync(PaginationParameter paginationParameter, PackageFilterModel packageFilterModel);

        Task<EventPackage> GetPackageById(Guid id);
    }
}