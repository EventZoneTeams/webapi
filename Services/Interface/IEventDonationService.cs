using Domain.DTOs.EventDonationDTOs;
using Repositories.Commons;

namespace Services.Interface
{
    public interface IEventDonationService
    {
        Task<ApiResult<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data);

        Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(Guid id);

        Task<List<EventDonationDetailDTO>> GetMyDonation();
    }
}