using Repositories.Models.ImageDTOs;
using Services.DTO.EventProductsModel;
using Services.DTO.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventProductService
    {
        Task<ResponseGenericModel<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct, List<ImageReturnDTO> images);

        Task<List<EventProductDetailModel>> GetAllProductsAsync();

        Task<ResponseGenericModel<EventProductDetailModel>> UpdateEventProductAsync(int productId, EventProductUpdateModel updateModel);

        Task<ResponseGenericModel<List<EventProductDetailModel>>> DeleteEventProductAsync(List<int> productIds);

        Task<ResponseGenericModel<List<EventProductDetailModel>>> CreateEventProductAsync(List<EventProductCreateModel> newProducts);

        Task<List<EventProductDetailModel>> GetAllProductsByEventAsync(int eventId);
    }
}