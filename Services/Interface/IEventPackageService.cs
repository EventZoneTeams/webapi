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
        Task<ResponseGenericModel<List<ProductInPackageDTO>>> CreatePackageWithProducts(int eventId, List<int> productIds);
        Task<List<EventPackageDetailDTO>> GetAll();
        Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package();
    }
}
