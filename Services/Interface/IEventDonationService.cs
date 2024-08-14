using EventZone.Domain.DTOs.EventDonationDTOs;
using EventZone.Repositories.Commons;

namespace EventZone.Services.Interface
{
    public interface IEventDonationService
    {
        Task<ApiResult<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data);

        Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(Guid id);

        Task<List<EventDonationDetailDTO>> GetMyDonation();
    }
}