using Domain.Entities;
using Repositories.Commons;
using Repositories.Models;
using Repositories.Models.PackageModels;

namespace Repositories.Interfaces
{
    public interface IEventPackageRepository : IGenericRepository<EventPackage>
    {
        Task<List<ProductInPackage>> CreatePackageWithProducts(int eventId, string description, string thumbnailUr, List<ProductQuantityDTO> productIds);

        Task<List<EventPackage>> GetAllPackgesByEventId(int eventId);

        Task<List<EventPackageDetailDTO>> GetAllPackageWithProducts();

        Task<List<ProductInPackage>> GetProductsInPackagesWithProduct();

        Task<List<EventPackage>> GetAllPackageWithProductsByEventId(int eventId);

        Task<Pagination<EventPackage>> GetPackagessByFiltersAsync(PaginationParameter paginationParameter, PackageFilterModel packageFilterModel);
    }
}