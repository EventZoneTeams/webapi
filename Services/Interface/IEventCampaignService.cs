using Repositories.Commons;
using Repositories.Models.EventCampaignModels;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventCampaignService
    {
        Task<ApiResult<EventCampaignDTO>> CreateEventCampaignAsync(EventCampaignCreateDTO eventCampaignDTO);

        Task<ApiResult<EventCampaignDTO>> DeleteCampaignByIdAsync(int id);

        Task<ApiResult<List<EventCampaignDTO>>> DeleteEventCampaignAsync(List<int> campaignIds);

        Task<EventCampaignStaticticDTO> GetACampaignsByIdAsync(int id);

        Task<List<EventCampaignDTO>> GetAllCampaignsAsync();

        Task<List<EventCampaignDTO>> GetAllCampaignsByEventAsync(int eventId);

        Task<Pagination<EventCampaignDTO>> GetCampaignsByFiltersAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilterModel);

        Task<ApiResult<EventCampaignDTO>> UpdateEventCampaignAsync(int id, EventCampaignUpdateDTO eventCampaignDTO);
    }
}