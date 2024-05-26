using Services.BusinessModels.EventProductsModel;
using Services.BusinessModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventProductService
    {
        Task<ResponseGenericModel<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct);
        Task<List<EventProductDetailModel>> GetAllProductsAsync();
        Task<ResponseGenericModel<EventProductDetailModel>> UpdateEventProductAsync(int productId, EventProductUpdateModel updateModel);
        Task<ResponseGenericModel<List<EventProductDetailModel>>> DeleteEventProductAsync(List<int> productIds);

    }
}
