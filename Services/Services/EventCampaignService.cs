using AutoMapper;
using Domain.Entities;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Models.ImageDTOs;
using Repositories.Models.ProductModels;
using Repositories.Models;
using Services.DTO.EventProductsModel;
using Services.DTO.ResponseModels;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DTO.EventCampaignDTOs;
using Repositories.Models.EventCampaignModels;

namespace Services.Services
{
    public class EventCampaignService : IEventCampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventCampaignService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseGenericModel<EventCampaignDTO>> CreateEventCampaignAsync(EventCampaignDTO eventCampaignDTO)
        {
            try
            {
                if (await _unitOfWork.EventRepository.GetByIdAsync(eventCampaignDTO.EventId) == null)
                {
                    return new ResponseGenericModel<EventCampaignDTO>()
                    {
                        Status = false,
                        Message = " Added failed, event is not existed",
                        Data = null
                    };
                }

                var newCampaign = new EventCampaign
                {
                    Name = eventCampaignDTO.Name,
                    Description = eventCampaignDTO.Description,
                    StartDate = eventCampaignDTO.StartDate,
                    EndDate = eventCampaignDTO.EndDate,
                    Status = eventCampaignDTO.Status,
                    GoalAmount = eventCampaignDTO.GoalAmount,
                    CollectedAmount = eventCampaignDTO.CollectedAmount,
                    EventId = eventCampaignDTO.EventId,
                };

                var result = await _unitOfWork.EventCampaignRepository.AddAsync(newCampaign);
                var returnData = _mapper.Map<EventCampaignDTO>(result);

                var check = await _unitOfWork.SaveChangeAsync();
                if (check > 0)
                {
                    return new ResponseGenericModel<EventCampaignDTO>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = returnData
                    };
                }
                return new ResponseGenericModel<EventCampaignDTO>()
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

        //public async Task<ResponseGenericModel<List<EventProductDetailModel>>> CreateEventProductAsync(List<EventProductCreateModel> newProducts)
        //{
        //    try
        //    {
        //        var createProducts = new List<EventProduct>();
        //        foreach (var newProduct in newProducts)
        //        {
        //            var product = new EventProduct
        //            {
        //                Name = newProduct.Name,
        //                Description = newProduct.Description,
        //                EventId = newProduct.EventId,
        //                Price = newProduct.Price,
        //                QuantityInStock = newProduct.QuantityInStock
        //            };
        //            createProducts.Add(product);
        //        }

        //        await _unitOfWork.EventProductRepository.AddRangeAsync(createProducts);

        //        var check = await _unitOfWork.SaveChangeAsync();
        //        if (check > 0)
        //        {
        //            return new ResponseGenericModel<List<EventProductDetailModel>>()
        //            {
        //                Status = true,
        //                Message = " Added successfully",
        //                Data = _mapper.Map<List<EventProductDetailModel>>(createProducts)
        //            };
        //        }
        //        return new ResponseGenericModel<List<EventProductDetailModel>>()
        //        {
        //            Status = false,
        //            Message = " Added failed",
        //            Data = _mapper.Map<List<EventProductDetailModel>>(createProducts)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<ResponseGenericModel<List<EventCampaignDTO>>> DeleteEventCampaignAsync(List<int> campaignIds)
        {
            var allCampaigns = await _unitOfWork.EventCampaignRepository.GetAllAsync();
            var existingIds = allCampaigns.Where(e => campaignIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = campaignIds.Except(existingIds).ToList();

            if (existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventCampaignRepository.SoftRemoveRangeById(existingIds);
                if (result)
                {
                    return new ResponseGenericModel<List<EventCampaignDTO>>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<EventCampaignDTO>>(allCampaigns.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            else
            {
                if (nonExistingIds.Count > 0)
                {
                    string nonExistingIdsString = string.Join(", ", nonExistingIds);

                    return new ResponseGenericModel<List<EventCampaignDTO>>()
                    {
                        Status = false,
                        Message = "There are few ids that are no existed product id: " + nonExistingIdsString,
                        Data = _mapper.Map<List<EventCampaignDTO>>(allCampaigns.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            return new ResponseGenericModel<List<EventCampaignDTO>>()
            {
                Status = false,
                Message = "failed",
                Data = null
            };
        }

        public async Task<List<EventCampaignDTO>> GetAllProductsAsync()
        {
            var result = await _unitOfWork.EventCampaignRepository.GetAllAsync();

            return _mapper.Map<List<EventCampaignDTO>>(result);
        }

        public async Task<List<EventCampaignDTO>> GetAllProductsByEventAsync(int eventId)
        {
            var result = await _unitOfWork.EventCampaignRepository.GetAllCampaignByEvent(eventId);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<List<EventCampaignDTO>>(result);
        }

        public async Task<ResponseGenericModel<EventCampaignDTO>> UpdateEventProductAsync(int id, EventCampaignDTO eventCampaignDTO)
        {
            var esistingCampaign = await _unitOfWork.EventCampaignRepository.GetByIdAsync(id);
            if (esistingCampaign != null)
            {
                esistingCampaign = _mapper.Map(eventCampaignDTO, esistingCampaign);
                await _unitOfWork.EventCampaignRepository.Update(esistingCampaign);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    return new ResponseGenericModel<EventCampaignDTO>()
                    {
                        Status = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<EventCampaignDTO>(esistingCampaign)
                    };
                }
                else
                {
                    return new ResponseGenericModel<EventCampaignDTO>()
                    {
                        Status = false,
                        Message = "FAILED",
                        Data = null
                    };
                }
            }
            return new ResponseGenericModel<EventCampaignDTO>()
            {
                Status = false,
                Message = "This campaign id is not existed",
                Data = null
            };
        }

        public async Task<Pagination<EventCampaignDTO>> GetProductsByFiltersAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilterModel)
        {
            var campaigns = await _unitOfWork.EventCampaignRepository.GetCampaignsByFilterAsync(paginationParameter, campaignFilterModel);
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (campaigns != null)
            {
                var result = _mapper.Map<List<EventCampaignDTO>>(campaigns);
                return new Pagination<EventCampaignDTO>(result, campaigns.TotalCount, campaigns.CurrentPage, campaigns.PageSize);
            }
            return null;
        }
    }
}