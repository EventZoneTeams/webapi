using AutoMapper;
using Repositories.DTO;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels.ResponseModels;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EventPackageService : IEventPackageService
    {
        private readonly IEventPackageRepository _eventPackageRepository;
        private readonly IMapper _mapper;

        public EventPackageService(IEventPackageRepository eventPackageRepository, IMapper mapper)
        {
            _eventPackageRepository = eventPackageRepository;
            _mapper = mapper;
        }

        public async Task<ResponseGenericModel<List<ProductInPackageDTO>>> CreatePackageWithProducts(int eventId, List<int> productIds)
        {
            var result = await _eventPackageRepository.CreatePackageWithProducts(eventId, productIds);
            if ( result !=null )
            {
                return new ResponseGenericModel<List<ProductInPackageDTO>>
                {
                    Status=true,
                    Message="Add sucessfully",
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

        public async Task<List<EventPackageDetailDTO>> GetAll()
        {
            return await _eventPackageRepository.GetAllPackageWithProducts();
        }

        public async Task<List<ProductInPackageDTO>> GetProductsInPackagesWithProduct_Package()
        {
            return _mapper.Map<List<ProductInPackageDTO>>(await _eventPackageRepository.GetProductsInPackagesWithProduct());

        }




    }
}
