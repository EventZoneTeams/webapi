using AutoMapper;
using EventZone.Domain.DTOs.EventPackageDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.PackageModels;
using EventZone.Services.Interface;

namespace EventZone.Services.Services
{
    public class EventPackageService : IEventPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventPackageService(IUnitOfWork unitOfWork, IMapper mapper, IEventService eventService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task<ApiResult<EventPackageDetailDTO>> CreatePackageWithProducts(Guid eventId, string thumbnailurl, CreatePackageRequest newPackage)
        {
            var existedEvent = await _unitOfWork.EventRepository.GetByIdAsync(eventId);
            if (existedEvent == null)
            {
                return new ApiResult<EventPackageDetailDTO>
                {
                    IsSuccess = false,
                    Message = "This event is not existed",
                    Data = null
                };
            }

            var check = await _unitOfWork.EventPackageRepository.CreatePackageWithProducts(eventId, newPackage.Description, thumbnailurl, newPackage.Products, newPackage.Title);
            if (check != null)
            {
                var result = _mapper.Map<EventPackageDetailDTO>(check.First().EventPackage);
                result.Products = _mapper.Map<List<EventProductDetailDTO>>(check.Select(x => x.EventProduct).ToList());
                return new ApiResult<EventPackageDetailDTO>
                {
                    IsSuccess = true,
                    Message = "Add sucessfully",
                    Data = result
                };
            }

            return new ApiResult<EventPackageDetailDTO>
            {
                IsSuccess = false,
                Message = "Failed",
                Data = null
            };
        }

        public async Task<ApiResult<List<EventPackageDetailDTO>>> DeleteEventPackagesAsync(List<Guid> packageIds)
        {
            var allPackages = await _unitOfWork.EventPackageRepository.GetAllPackageWithProducts();
            var existingIds = allPackages.Where(e => packageIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = packageIds.Except(existingIds).ToList();
            if (existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventPackageRepository.SoftRemoveRangeById(existingIds);
                string nonExistingIdsString = string.Join(", ", nonExistingIds);
                if (result)
                {
                    allPackages.ForEach(x => x.IsDeleted = true);
                    if (nonExistingIds.Count > 0)
                    {
                        return new ApiResult<List<EventPackageDetailDTO>>()
                        {
                            IsSuccess = false,
                            Message = "Removed successfully but there are still non-existed package: " + nonExistingIdsString,
                            Data = _mapper.Map<List<EventPackageDetailDTO>>(allPackages.Where(e => existingIds.Contains(e.Id)))
                        };
                    }

                    return new ApiResult<List<EventPackageDetailDTO>>()
                    {
                        IsSuccess = true,
                        Message = " Removed successfully",
                        Data = _mapper.Map<List<EventPackageDetailDTO>>(allPackages.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }

            return new ApiResult<List<EventPackageDetailDTO>>()
            {
                IsSuccess = false,
                Message = "There are no existed packages:" + string.Join(", ", packageIds) + " please try again",
                Data = null
            };
        }

        public async Task<ApiResult<EventPackageDetailDTO>> DeleteEventProductByIdAsync(Guid id)
        {
            var package = await _unitOfWork.EventPackageRepository.GetByIdAsync(id);

            if (package != null)
            {
                var result = await _unitOfWork.EventPackageRepository.SoftRemove(package);
                //save changes
                await _unitOfWork.SaveChangeAsync();
                if (result)
                {
                    return new ApiResult<EventPackageDetailDTO>()
                    {
                        IsSuccess = true,
                        Message = "Package " + id + " Removed successfully",
                        Data = _mapper.Map<EventPackageDetailDTO>(package)
                    };
                }
            }
            return new ApiResult<EventPackageDetailDTO>()
            {
                IsSuccess = false,
                Message = "There are no existed product id: " + id,
                Data = null
            };
        }

        public async Task<List<EventPackageDetailDTO>> GetAllWithProducts()
        {
            return await _unitOfWork.EventPackageRepository.GetAllPackageWithProducts();
        }

        public async Task<List<EventPackageDetailDTO>> GetAllPackageOfEvent(Guid eventId)
        {
            var result = await _unitOfWork.EventPackageRepository.GetAllPackageWithProductsByEventId(eventId);

            return _mapper.Map<List<EventPackageDetailDTO>>(result);
        }

        public async Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package()
        {
            return _mapper.Map<List<ProductInPackageDTO>>(await _unitOfWork.EventPackageRepository.GetProductsInPackagesWithProduct());
        }

        public async Task<ApiResult<EventPackageDetailDTO>> GetPackageById(Guid packageId)
        {
            var package = await _unitOfWork.EventPackageRepository.GetPackageById(packageId);
            if (package == null)
            {
                return new ApiResult<EventPackageDetailDTO>()
                {
                    IsSuccess = false,
                    Message = "This package id is not found",
                    Data = null
                };
            }

            var result = _mapper.Map<EventPackageDetailDTO>(package);

            result.Products = _mapper.Map<List<EventProductDetailDTO>>(package.ProductsInPackage.Select(x => x.EventProduct));

            return new ApiResult<EventPackageDetailDTO>()
            {
                IsSuccess = false,
                Message = "Found successfully package " + packageId,

                Data = result
            };
        }

        public async Task<Pagination<EventPackageDetailDTO>> GetPackagessByFiltersAsync(PaginationParameter paginationParameter, PackageFilterModel packageFilterModel)
        {
            var products = await _unitOfWork.EventPackageRepository.GetPackagessByFiltersAsync(paginationParameter, packageFilterModel);
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (products != null)
            {
                var result = _mapper.Map<List<EventPackageDetailDTO>>(products);
                return new Pagination<EventPackageDetailDTO>(result, products.TotalCount, products.CurrentPage, products.PageSize);
            }
            return null;
        }
    }
}