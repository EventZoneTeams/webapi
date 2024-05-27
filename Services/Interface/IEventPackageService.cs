using Repositories.DTO;
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
        Task<ResponseGenericModel<List<ProductInPackageDTO>>> CreatePackageWithProducts(int eventId, string description, List<ProductQuantityDTO> products);
        Task<List<EventPackageDetailDTO>> GetAllWithProducts();
        Task<List<EventPackageDetailDTO>> GetAllPackageOfEvent(int eventId);
        Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package();
        Task<ResponseGenericModel<List<EventPackageDetailDTO>>> DeleteEventPackagesAsync(List<int> packageIds);
    }
}
