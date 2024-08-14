using EventZone.Domain.DTOs.EventPackageDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.PackageModels;

namespace EventZone.Services.Interface
{
    public interface IEventPackageService
    {
        Task<ApiResult<EventPackageDetailDTO>> CreatePackageWithProducts(Guid eventId, string thumbnailurl, CreatePackageRequest newPackage);

        Task<List<EventPackageDetailDTO>> GetAllWithProducts();

        Task<List<EventPackageDetailDTO>> GetAllPackageOfEvent(Guid eventId);

        Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package();

        Task<ApiResult<List<EventPackageDetailDTO>>> DeleteEventPackagesAsync(List<Guid> packageIds);

        Task<Pagination<EventPackageDetailDTO>> GetPackagessByFiltersAsync(PaginationParameter paginationParameter, PackageFilterModel packageFilterModel);

        Task<ApiResult<EventPackageDetailDTO>> GetPackageById(Guid packageId);

        Task<ApiResult<EventPackageDetailDTO>> DeleteEventProductByIdAsync(Guid id);
    }
}