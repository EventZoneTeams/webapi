using Repositories.DTO;
using Services.BusinessModels.EventPackageModels;
using Services.BusinessModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventPackageService
    {
        Task<ResponseGenericModel<List<ProductInPackageDTO>>> CreatePackageWithProducts(int eventId, string thumbnailurl, CreatePackageRequest newPackage);
        Task<List<EventPackageDetailDTO>> GetAllWithProducts();
        Task<List<EventPackageDetailDTO>> GetAllPackageOfEvent(int eventId);
        Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package();
        Task<ResponseGenericModel<List<EventPackageDetailDTO>>> DeleteEventPackagesAsync(List<int> packageIds);
    }
}
