using AutoMapper;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Repositories.Interfaces;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.EventDonationDTOs;
using Services.DTO.ResponseModels;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EventDonationService : IEventDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public EventDonationService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<ResponseGenericModel<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data)
        {
            try
            {
                var checkEvent = await _unitOfWork.EventCampaignRepository.GetByIdAsync(data.EventCampaignId);
                var checkUser = _claimsService.GetCurrentUserId == -1 ? 1 : _claimsService.GetCurrentUserId; //test

                if (checkEvent == null)
                {
                    return new ResponseGenericModel<EventDonationDetailDTO>()
                    {
                        Status = false,
                        Message = " This campaign is not existed",
                        Data = null
                    };
                }

                var newDonation = new EventDonation
                {
                    EventCampaignId = data.EventCampaignId,
                    UserId = checkUser,
                    Amount = data.Amount,
                    DonationDate = DateTime.UtcNow.AddHours(7)
                };

                checkEvent.CollectedAmount += newDonation.Amount;

                if (checkEvent.CollectedAmount > checkEvent.GoalAmount)
                {
                    return new ResponseGenericModel<EventDonationDetailDTO>()
                    {
                        Status = false,
                        Message = "added failed, cannot donate more than goal amount",
                        Data = _mapper.Map<EventDonationDetailDTO>(newDonation)
                    };
                }
                await _unitOfWork.EventCampaignRepository.Update(checkEvent);

                var newData = await _unitOfWork.EventDonationRepository.AddAsync(newDonation);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseGenericModel<EventDonationDetailDTO>()
                    {
                        Status = true,
                        Message = "Successfully added",
                        Data = _mapper.Map<EventDonationDetailDTO>(newData)
                    };
                }
                else
                {
                    throw new Exception("Added failed please check");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(int id)
        {
            var result = await _unitOfWork.EventDonationRepository.GetAllDonationByCampaignId(id);
            return _mapper.Map<List<EventDonationDetailDTO>>(result);
        }
    }
}