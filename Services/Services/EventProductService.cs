using AutoMapper;
using Domain.Entities;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Models;
using Repositories.Models.ImageDTOs;
using Repositories.Models.ProductModels;
using Services.DTO.EventProductsModel;
using Services.DTO.ResponseModels;
using Services.Interface;

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

        public async Task<ResponseGenericModel<EventProductDetailModel>> CreateEventProductAsync(EventProductCreateModel newProduct, List<ImageReturnDTO> images)
        {
            try
            {
                if (await _unitOfWork.EventRepository.GetByIdAsync(newProduct.EventId) == null)
                {
                    return new ResponseGenericModel<EventProductDetailModel>()
                    {
                        Status = false,
                        Message = " Added failed, event is not existed",
                        Data = null
                    };
                }

                var product = new EventProduct
                {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    EventId = newProduct.EventId,
                    Price = newProduct.Price,
                    QuantityInStock = newProduct.QuantityInStock
                };

                var result = await _unitOfWork.EventProductRepository.AddAsync(product);
                var returnData = _mapper.Map<EventProductDetailModel>(result);

                var check = await _unitOfWork.SaveChangeAsync();
                if (check > 0)
                {
                    var imagerResult = await _unitOfWork.EventProductRepository.AddImagesForProduct(result.Id, images);
                    check = await _unitOfWork.SaveChangeAsync();
                    returnData.ProductImages = _mapper.Map<List<ImageReturnDTO>>(imagerResult);

                    return new ResponseGenericModel<EventProductDetailModel>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = returnData
                    };
                }
                return new ResponseGenericModel<EventProductDetailModel>()
                {
                    Status = false,
                    Message = " Added failed",
                    Data = returnData
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseGenericModel<List<EventProductDetailModel>>> CreateEventProductAsync(List<EventProductCreateModel> newProducts)
        {
            try
            {
                var createProducts = new List<EventProduct>();
                foreach (var newProduct in newProducts)
                {
                    var product = new EventProduct
                    {
                        Name = newProduct.Name,
                        Description = newProduct.Description,
                        EventId = newProduct.EventId,
                        Price = newProduct.Price,
                        QuantityInStock = newProduct.QuantityInStock
                    };
                    createProducts.Add(product);
                }

                await _unitOfWork.EventProductRepository.AddRangeAsync(createProducts);

                var check = await _unitOfWork.SaveChangeAsync();
                if (check > 0)
                {
                    return new ResponseGenericModel<List<EventProductDetailModel>>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<EventProductDetailModel>>(createProducts)
                    };
                }
                return new ResponseGenericModel<List<EventProductDetailModel>>()
                {
                    Status = false,
                    Message = " Added failed",
                    Data = _mapper.Map<List<EventProductDetailModel>>(createProducts)
                };
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

            if (existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventProductRepository.SoftRemoveRangeById(existingIds);
                if (result)
                {
                    return new ResponseGenericModel<List<EventProductDetailModel>>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<EventProductDetailModel>>(allProduct.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            else
            {
                if (nonExistingIds.Count > 0)
                {
                    string nonExistingIdsString = string.Join(", ", nonExistingIds);

                    return new ResponseGenericModel<List<EventProductDetailModel>>()
                    {
                        Status = false,
                        Message = "There are few ids that are no existed product id: " + nonExistingIdsString,
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
            var result = await _unitOfWork.EventProductRepository.GetAllProductsWithImages();

            return _mapper.Map<List<EventProductDetailModel>>(result);
        }

        public async Task<List<EventProductDetailModel>> GetAllProductsByEventAsync(int eventId)
        {
            var result = await _unitOfWork.EventProductRepository.GetAllProductsByEvent(eventId);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<List<EventProductDetailModel>>(result);
        }

        public async Task<ResponseGenericModel<EventProductDetailModel>> UpdateEventProductAsync(int productId, EventProductUpdateModel updateModel)
        {
            var existingProduct = await _unitOfWork.EventProductRepository.GetByIdAsync(productId);
            if (existingProduct != null)
            {
                existingProduct = _mapper.Map(updateModel, existingProduct);
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

        public async Task<Pagination<EventProductDetailModel>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel)
        {
            var products = await _unitOfWork.EventProductRepository.GetProductsByFiltersAsync(paginationParameter, productFilterModel);
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (products != null)
            {
                var result = _mapper.Map<List<EventProductDetailModel>>(products);
                return new Pagination<EventProductDetailModel>(result, products.TotalCount, products.CurrentPage, products.PageSize);
            }
            return null;
        }
    }
}