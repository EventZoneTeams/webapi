using Domain.DTOs.EventCampaignDTOs;
using Repositories.Commons;
using Repositories.Models.EventCampaignModels;

namespace Services.Interface
{
    public interface IEventCampaignService
    {
        Task<ApiResult<EventCampaignDTO>> CreateEventCampaignAsync(EventCampaignCreateDTO eventCampaignDTO);

        Task<ApiResult<EventCampaignDTO>> DeleteCampaignByIdAsync(Guid id);

        Task<ApiResult<List<EventCampaignDTO>>> DeleteEventCampaignAsync(List<Guid> campaignIds);

        Task<EventCampaignStaticticDTO> GetACampaignsByIdAsync(Guid id);

        Task<List<EventCampaignDTO>> GetAllCampaignsAsync();

        Task<List<EventCampaignDTO>> GetAllCampaignsByEventAsync(Guid eventId);

        Task<Pagination<EventCampaignDTO>> GetCampaignsByFiltersAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilterModel);

        Task<ApiResult<EventCampaignDTO>> UpdateEventCampaignAsync(Guid id, EventCampaignUpdateDTO eventCampaignDTO);
    }
}