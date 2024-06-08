using AutoMapper;
using Repositories.Interfaces;
using Repositories.Models;
using Services.DTO.EventPackageModels;
using Services.DTO.ResponseModels;
using Services.Interface;

namespace Services.Services
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

        public async Task<ResponseGenericModel<List<ProductInPackageDTO>>> CreatePackageWithProducts(int eventId, string thumbnailurl, CreatePackageRequest newPackage)
        {
            var existedEvent = await _unitOfWork.EventRepository.GetByIdAsync(eventId);
            if (existedEvent == null)
            {
                return new ResponseGenericModel<List<ProductInPackageDTO>>
                {
                    Status = false,
                    Message = "This event is not existed",
                    Data = null
                };
            }

            var result = await _unitOfWork.EventPackageRepository.CreatePackageWithProducts(eventId, newPackage.Description, thumbnailurl, newPackage.Products);
            if (result != null)
            {
                return new ResponseGenericModel<List<ProductInPackageDTO>>
                {
                    Status = true,
                    Message = "Add sucessfully",
                    Data = _mapper.Map<List<ProductInPackageDTO>>(result)
                };
            }

            return new ResponseGenericModel<List<ProductInPackageDTO>>
            {
                Status = false,
                Message = "Failed",
                Data = null
            };
        }

        public async Task<ResponseGenericModel<List<EventPackageDetailDTO>>> DeleteEventPackagesAsync(List<int> packageIds)
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
                        return new ResponseGenericModel<List<EventPackageDetailDTO>>()
                        {
                            Status = false,
                            Message = "Removed successfully but there are still non-existed package: " + nonExistingIdsString,
                            Data = _mapper.Map<List<EventPackageDetailDTO>>(allPackages.Where(e => existingIds.Contains(e.Id)))
                        };
                    }

                    return new ResponseGenericModel<List<EventPackageDetailDTO>>()
                    {
                        Status = true,
                        Message = " Removed successfully",
                        Data = _mapper.Map<List<EventPackageDetailDTO>>(allPackages.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }

            return new ResponseGenericModel<List<EventPackageDetailDTO>>()
            {
                Status = false,
                Message = "There are no existed packages:" + string.Join(", ", packageIds) + " please try again",
                Data = null
            };
        }

        public async Task<List<EventPackageDetailDTO>> GetAllWithProducts()
        {
            return await _unitOfWork.EventPackageRepository.GetAllPackageWithProducts();
        }

        public async Task<List<EventPackageDetailDTO>> GetAllPackageOfEvent(int eventId)
        {
            return await _unitOfWork.EventPackageRepository.GetAllPackageWithProductsByEventId(eventId);
        }

        public async Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package()
        {
            return _mapper.Map<List<ProductInPackageDTO>>(await _unitOfWork.EventPackageRepository.GetProductsInPackagesWithProduct());
        }
    }
}