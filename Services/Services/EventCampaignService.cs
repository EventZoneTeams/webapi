using AutoMapper;
using Domain.DTOs.EventCampaignDTOs;
using Domain.Entities;
using Domain.Enums;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Models.EventCampaignModels;
using Services.Interface;

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

        public async Task<EventCampaignStaticticDTO> GetACampaignsByIdAsync(Guid id)
        {
            try
            {
                var campaign = await _unitOfWork.EventCampaignRepository.GetCampainById(id);

                if (campaign == null)
                {
                    // Xử lý trường hợp không tìm thấy chiến dịch
                    throw new Exception("Campaign not found"); // Hoặc trả về một ResponseModel với trạng thái lỗi
                }

                var totalDonors = campaign.EventDonations.Count();
                var targetAchievementPercentage = ((decimal)campaign.CollectedAmount / campaign.GoalAmount) * 100;
                var average = (decimal)(totalDonors > 0 ? campaign.EventDonations.Average(d => d.Amount) : 0);
                var highest = totalDonors > 0 ? campaign.EventDonations.Max(d => d.Amount) : 0;

                var campaignDto = _mapper.Map<EventCampaignStaticticDTO>(campaign);
                campaignDto.TotalDonors = totalDonors;
                campaignDto.TargetAchievementPercentage = targetAchievementPercentage;
                campaignDto.AverageDonationAmount = average;
                campaignDto.HighestDonationAmount = highest;

                return campaignDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResult<EventCampaignDTO>> CreateEventCampaignAsync(EventCampaignCreateDTO eventCampaignDTO)
        {
            try
            {
                var checkEvent = await _unitOfWork.EventRepository.GetByIdAsync(eventCampaignDTO.EventId);
                if (checkEvent == null)
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = false,
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
                    Status = eventCampaignDTO.Status.ToString(),
                    GoalAmount = eventCampaignDTO.GoalAmount,
                    CollectedAmount = 0,
                    EventId = eventCampaignDTO.EventId,
                };

                if (checkEvent.Status == EventStatusEnums.COMPLETED.ToString())
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = false,
                        Message = " Added failed due to event status: " + checkEvent.Status,
                        Data = _mapper.Map<EventCampaignDTO>(newCampaign)
                    };
                };

                newCampaign.Status = string.IsNullOrEmpty(checkEvent.Status) ? newCampaign.Status : checkEvent.Status;

                var result = await _unitOfWork.EventCampaignRepository.AddAsync(newCampaign);
                // mapping data tra ve response truoc savechanges se ko lay duoc ID??
                var check = await _unitOfWork.SaveChangeAsync();
                if (check > 0)
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<EventCampaignDTO>(result)
                    };
                }
                return new ApiResult<EventCampaignDTO>()
                {
                    Success = false,
                    Message = " Added failed",
                    Data = _mapper.Map<EventCampaignDTO>(result)
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

        public async Task<ApiResult<List<EventCampaignDTO>>> DeleteEventCampaignAsync(List<Guid> campaignIds)
        {
            var allCampaigns = await _unitOfWork.EventCampaignRepository.GetAllAsync();
            var existingIds = allCampaigns.Where(e => campaignIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = campaignIds.Except(existingIds).ToList();

            if (existingIds.Count > 0)
            {
                var result = await _unitOfWork.EventCampaignRepository.SoftRemoveRangeById(existingIds);
                if (result)
                {
                    return new ApiResult<List<EventCampaignDTO>>()
                    {
                        Success = true,
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

                    return new ApiResult<List<EventCampaignDTO>>()
                    {
                        Success = false,
                        Message = "There are few ids that are no existed product id: " + nonExistingIdsString,
                        Data = _mapper.Map<List<EventCampaignDTO>>(allCampaigns.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            return new ApiResult<List<EventCampaignDTO>>()
            {
                Success = false,
                Message = "failed",
                Data = null
            };
        }

        public async Task<ApiResult<EventCampaignDTO>> DeleteCampaignByIdAsync(Guid id)
        {
            var product = await _unitOfWork.EventCampaignRepository.GetByIdAsync(id);

            if (product != null)
            {
                await _unitOfWork.EventCampaignRepository.SoftRemove(product);
                //save changes
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = true,
                        Message = "Product " + id + " Removed successfully",
                        Data = _mapper.Map<EventCampaignDTO>(product)
                    };
                }
                //else
                //{
                //    return new ResponseGenericModel<EventCampaignDTO>()
                //    {
                //        Status = false,
                //        Message = "Deleted failed, something wrong has happenned",
                //        Data = _mapper.Map<EventCampaignDTO>(product)
                //    };
                //}
            }
            return new ApiResult<EventCampaignDTO>()
            {
                Success = false,
                Message = "There are no existed campaign with id: " + id,
                Data = null
            };
        }

        public async Task<List<EventCampaignDTO>> GetAllCampaignsAsync()
        {
            var result = await _unitOfWork.EventCampaignRepository.GetAllAsync();

            return _mapper.Map<List<EventCampaignDTO>>(result);
        }

        public async Task<List<EventCampaignDTO>> GetAllCampaignsByEventAsync(Guid eventId)
        {
            var result = await _unitOfWork.EventCampaignRepository.GetAllCampaignByEvent(eventId);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<List<EventCampaignDTO>>(result);
        }

        public async Task<ApiResult<EventCampaignDTO>> UpdateEventCampaignAsync(Guid id, EventCampaignUpdateDTO eventCampaignDTO)
        {
            var esistingCampaign = await _unitOfWork.EventCampaignRepository.GetByIdAsync(id);
            if (esistingCampaign != null)
            {
                esistingCampaign = _mapper.Map(eventCampaignDTO, esistingCampaign);
                await _unitOfWork.EventCampaignRepository.Update(esistingCampaign);
                var updatedResult = await _unitOfWork.SaveChangeAsync();
                if (updatedResult > 0)
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = true,
                        Message = "Updated successfuly",
                        Data = _mapper.Map<EventCampaignDTO>(esistingCampaign)
                    };
                }
                else
                {
                    return new ApiResult<EventCampaignDTO>()
                    {
                        Success = false,
                        Message = "Something has been failed while updating",
                        Data = null
                    };
                }
            }
            return new ApiResult<EventCampaignDTO>()
            {
                Success = false,
                Message = "This campaign id is not existed",
                Data = null
            };
        }

        public async Task<Pagination<EventCampaignDTO>> GetCampaignsByFiltersAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilterModel)
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