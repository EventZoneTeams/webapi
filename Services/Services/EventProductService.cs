using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels.EventProductsModel;
using Services.BusinessModels.ResponseModels;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EventProductService : IEventProductService
    {
        private readonly IEventProductRepository _eventProductrepository;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public EventProductService(IEventProductRepository eventProductrepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _eventProductrepository = eventProductrepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseGenericModel<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct)
        {
            try
            {
                var product = new EventProduct
                {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    EventId = newProduct.EventId,
                    Price = newProduct.Price,
                    QuantityInStock = newProduct.QuantityInStock
                };

                var result = await _unitOfWork.EventProductRepository.AddAsync(product);

                var check = await _unitOfWork.SaveChangeAsync();
                if (check > 0)
                {
                    return new ResponseGenericModel<EventProductDetailModel>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<EventProductDetailModel>(result)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<ResponseGenericModel<List<EventProductDetailModel>>> DeleteEventProductAsync(List<int> productIds)
        {
            var allProduct = await _unitOfWork.EventProductRepository.GetAllAsync();
            var existingIds = allProduct.Where(e => productIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = productIds.Except(existingIds).ToList();

            if(existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventRepository.SoftRemoveRangeById(existingIds);
                if(result)
                {
                    return new ResponseGenericModel<List<EventProductDetailModel>>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<EventProductDetailModel>>(allProduct.Where(e=> existingIds.Contains(e.Id)))
                    };
                }
            }
            else
            {
                if (nonExistingIds.Count > 0)
                {
                    return new ResponseGenericModel<List<EventProductDetailModel>>()
                    {
                        Status = false,
                        Message = "There are few ids that are no existed" + nonExistingIds,
                        Data = _mapper.Map<List<EventProductDetailModel>>(allProduct.Where(e => existingIds.Contains(e.Id)))
                    };
                }
              
            }
            return new ResponseGenericModel<List<EventProductDetailModel>>()
            {
                Status = false,
                Message = "failed",
                Data = null
            };

        }

        public async Task<List<EventProductDetailModel>> GetAllProductsAsync()
        {
            var result = await _unitOfWork.EventProductRepository.GetAllAsync();

            return _mapper.Map<List<EventProductDetailModel>>(result);
        }

        public async Task<ResponseGenericModel<EventProductDetailModel>> UpdateEventProductAsync(int productId, EventProductDetailModel updateModel)
        {
            var existingProduct = await _unitOfWork.EventProductRepository.GetByIdAsync(productId);
            if(existingProduct != null)
            {
                existingProduct = _mapper.Map(updateModel,existingProduct);
                await _unitOfWork.EventProductRepository.Update(existingProduct);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    return new ResponseGenericModel<EventProductDetailModel>()
                    {
                        Status = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<EventProductDetailModel>(existingProduct)
                    };
                }
                else
                {
                    return new ResponseGenericModel<EventProductDetailModel>()
                    {
                        Status = false,
                        Message = "FAILED",
                        Data = null
                    };
                }
               
            }
            return new ResponseGenericModel<EventProductDetailModel>()
            {
                Status = false,
                Message = "This account is not existed",
                Data = null
            };


        }
    }
}
