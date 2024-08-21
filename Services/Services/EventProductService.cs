using AutoMapper;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.ProductModels;
using EventZone.Services.Interface;
using Microsoft.Extensions.Logging;

namespace EventZone.Services.Services
{
    public class EventProductService : IEventProductService
    {
        private readonly IEventProductRepository _eventProductrepository;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUnitOfWork _unitOfWork;

        public EventProductService(IEventProductRepository eventProductrepository, IMapper mapper, IRedisService redisService, IUnitOfWork unitOfWork)
        {
            _eventProductrepository = eventProductrepository;
            _mapper = mapper;
            _redisService = redisService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<EventProductDetailDTO>> CreateEventProductAsync(EventProductCreateDTO newProduct, List<ImageReturnDTO> images)
        {
            //check existing event
            if (await _unitOfWork.EventRepository.GetByIdAsync(newProduct.EventId) == null)
            {
                throw new Exception("Event is not existed");
            }

            var product = new EventProduct
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                EventId = newProduct.EventId,
                Price = newProduct.Price,
                QuantityInStock = newProduct.QuantityInStock
            };

            //executing
            var result = await _unitOfWork.EventProductRepository.AddAsync(product);
            var returnData = _mapper.Map<EventProductDetailDTO>(result);
            var check = await _unitOfWork.SaveChangeAsync();
            if (check > 0)
            {
                var imagerResult = await _unitOfWork.EventProductRepository.AddImagesForProduct(result.Id, images);
                check = await _unitOfWork.SaveChangeAsync();
                returnData.ProductImages = _mapper.Map<List<ImageReturnDTO>>(imagerResult);
                // Clear cache as new is added
                await _redisService.DeleteKeyAsync(CacheKeys.EventProducts);
                return new ApiResult<EventProductDetailDTO>()
                {
                    IsSuccess = true,
                    Message = " Added successfully",
                    Data = returnData
                };
            }
            else
            {
                throw new Exception("Error while attempting to add new product");
            }
        }

        public async Task<ApiResult<List<EventProductDetailDTO>>> CreateEventProductAsync(List<EventProductCreateDTO> newProducts)
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
                await _redisService.DeleteKeyAsync(CacheKeys.EventProducts);
                return new ApiResult<List<EventProductDetailDTO>>()
                {
                    IsSuccess = true,
                    Message = " Added successfully",
                    Data = _mapper.Map<List<EventProductDetailDTO>>(createProducts)
                };
            }
            else
            {
                throw new Exception("Error while attempting to add new product");
            }
        }

        //public async Task<ApiResult<List<EventProductDetailDTO>>> DeleteEventProductAsync(List<Guid> productIds)
        //{
        //    var allProduct = await _unitOfWork.EventProductRepository.GetAllAsync();
        //    var existingIds = allProduct.Where(e => productIds.Contains(e.Id)).Select(e => e.Id).ToList();
        //    var nonExistingIds = productIds.Except(existingIds).ToList();

        //    if (existingIds.Count > 0)
        //    {
        //        var result = await _unitOfWork.EventProductRepository.SoftRemoveRangeById(existingIds);
        //        //save changes
        //        await _unitOfWork.SaveChangeAsync();
        //        if (result)
        //        {
        //            return new ApiResult<List<EventProductDetailDTO>>()
        //            {
        //                IsSuccess = true,
        //                Message = " Added successfully",
        //                Data = _mapper.Map<List<EventProductDetailDTO>>(allProduct.Where(e => existingIds.Contains(e.Id)))
        //            };
        //        }
        //    }
        //    else
        //    {
        //        if (nonExistingIds.Count > 0)
        //        {
        //            string nonExistingIdsString = string.Join(", ", nonExistingIds);

        //            return new ApiResult<List<EventProductDetailDTO>>()
        //            {
        //                IsSuccess = false,
        //                Message = "There are few ids that are no existed product id: " + nonExistingIdsString,
        //                Data = _mapper.Map<List<EventProductDetailDTO>>(allProduct.Where(e => existingIds.Contains(e.Id)))
        //            };
        //        }
        //    }
        //    return new ApiResult<List<EventProductDetailDTO>>()
        //    {
        //        IsSuccess = false,
        //        Message = "failed",
        //        Data = null
        //    };
        //}

        public async Task<ApiResult<EventProductDetailDTO>> DeleteEventProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.EventProductRepository.GetByIdAsync(id);

            if (product != null)
            {
                await _unitOfWork.EventProductRepository.SoftRemove(product);
                //save changes
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    // Clear specific cache key
                    await _redisService.DeleteKeyAsync(CacheKeys.EventProduct(id));
                    // Clear general list cache
                    await _redisService.DeleteKeyAsync(CacheKeys.EventProducts);

                    return ApiResult<EventProductDetailDTO>
                        .Succeed(_mapper.Map<EventProductDetailDTO>(product), "Product " + id + " Removed successfully");
                }
            }
            return ApiResult<EventProductDetailDTO>.Error(null, "There are no existed product id: " + id);
        }

        public async Task<List<EventProductDetailDTO>> GetAllProductsAsync()
        {
            List<EventProductDetailDTO> result;

            // Bước 1: Kiểm tra cache
            var cachedProducts = await _redisService.GetStringAsync(CacheKeys.EventProducts);
            if (!string.IsNullOrEmpty(cachedProducts))
            {
                // Nếu cache tồn tại, giải mã và sử dụng dữ liệu từ cache
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventProductDetailDTO>>(cachedProducts);
            }
            else
            {
                // Nếu cache không tồn tại, truy vấn từ cơ sở dữ liệu
                var eventProducts = await _unitOfWork.EventProductRepository.GetAllAsync();

                result = _mapper.Map<List<EventProductDetailDTO>>(eventProducts);

                // Lưu kết quả vào cache
                var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                await _redisService.SetStringAsync(CacheKeys.EventProducts, serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            }

            return result;
        }

        public async Task<List<EventProductDetailDTO>> GetAllProductsByEventAsyncOldVersion(Guid eventId)
        {
            var result = await _unitOfWork.EventProductRepository.GetAllProductsByEvent(eventId);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<List<EventProductDetailDTO>>(result);
        }

        public async Task<List<EventProductDetailDTO>> GetAllProductsByEventAsync(Guid eventId)
        {
            List<EventProductDetailDTO> result = await GetAllProductsAsync();

            result = result
                .Where(x => x.EventId == eventId)
                .ToList();

            return result;
        }

        public async Task<ApiResult<EventProductDetailDTO>> UpdateEventProductAsync(Guid productId, EventProductUpdateDTO updateModel)
        {
            var existingProduct = await _unitOfWork.EventProductRepository.GetByIdAsync(productId);
            if (existingProduct != null)
            {
                existingProduct = _mapper.Map(updateModel, existingProduct);
                existingProduct.QuantityInStock = updateModel.QuantityInStock == 0 ? existingProduct.QuantityInStock : updateModel.QuantityInStock;
                await _unitOfWork.EventProductRepository.Update(existingProduct);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    return new ApiResult<EventProductDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<EventProductDetailDTO>(existingProduct)
                    };
                }
                else
                {
                    return new ApiResult<EventProductDetailDTO>()
                    {
                        IsSuccess = false,
                        Message = "FAILED",
                        Data = null
                    };
                }
            }
            else
            {
                throw new Exception("There is no such product existing");
            }
        }

        public async Task<ApiResult<EventProductDetailDTO>> GetProductByIdd(Guid productId)
        {
            var product = await _unitOfWork.EventProductRepository.GetByIdAsync(productId, x => x.ProductImages);
            if (product == null)
            {
                return new ApiResult<EventProductDetailDTO>()
                {
                    IsSuccess = false,
                    Message = "This product id is not found",
                    Data = null
                };
            }

            return new ApiResult<EventProductDetailDTO>()
            {
                IsSuccess = true,
                Message = "Found successfully product " + productId,
                Data = _mapper.Map<EventProductDetailDTO>(product)
            };
        }

        public async Task<ApiResult<EventProductDetailDTO>> GetProductById(Guid id)
        {
            // Try to get from cache
            var cachedProduct = await _redisService.GetStringAsync(CacheKeys.EventProduct(id));
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                var product = Newtonsoft.Json.JsonConvert.DeserializeObject<EventProductDetailDTO>(cachedProduct);
                return new ApiResult<EventProductDetailDTO>()
                {
                    IsSuccess = true,
                    Message = "Found successfully product " + id,
                    Data = product
                };
            }

            // If not in cache, query the database
            var eventProduct = await _unitOfWork.EventProductRepository.GetByIdAsync(id);

            if (eventProduct == null)
            {
                throw new Exception("Event product not found");
            }

            var result = _mapper.Map<EventProductDetailDTO>(eventProduct);

            // Cache the result
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            await _redisService.SetStringAsync(CacheKeys.EventProduct(id), serializedResult, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            return new ApiResult<EventProductDetailDTO>()
            {
                IsSuccess = true,
                Message = "Found successfully product " + id,
                Data = result
            };
        }

        public async Task<Pagination<EventProductDetailDTO>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, ProductFilterModel productFilterModel)
        {
            var products = await _unitOfWork.EventProductRepository.GetProductsByFiltersAsync(paginationParameter, productFilterModel);
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (products != null)
            {
                var result = _mapper.Map<List<EventProductDetailDTO>>(products);
                return new Pagination<EventProductDetailDTO>(result, products.TotalCount, products.CurrentPage, products.PageSize);
            }
            return null;
        }
    }
}