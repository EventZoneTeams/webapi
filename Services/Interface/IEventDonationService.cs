using Repositories.Commons;
using Services.DTO.EventDonationDTOs;
using Services.DTO.ResponseModels;

namespace Services.Interface
{
    public interface IEventDonationService
    {
        Task<ApiResult<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data);

        Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(int id);

        Task<List<EventDonationDetailDTO>> GetMyDonation();
    }
}