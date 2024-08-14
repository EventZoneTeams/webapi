using EventZone.Domain.DTOs.EventPackageDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.PackageModels;
using Repositories.Models;

namespace EventZone.Repositories.Interfaces
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