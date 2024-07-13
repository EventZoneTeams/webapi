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
        Task<ResponseGenericModel<EventCampaignDTO>> CreateEventCampaignAsync(EventCampaignDTO eventCampaignDTO);

        Task<ResponseGenericModel<List<EventCampaignDTO>>> DeleteEventCampaignAsync(List<int> campaignIds);
        Task<EventCampaignStaticticDTO> GetACampaignsByIdAsync(int id);
        Task<List<EventCampaignDTO>> GetAllCampaignsAsync();

        Task<List<EventCampaignDTO>> GetAllCampaignsByEventAsync(int eventId);

        Task<Pagination<EventCampaignDTO>> GetCampaignsByFiltersAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilterModel);

        Task<ResponseGenericModel<EventCampaignDTO>> UpdateEventCampaignAsync(int id, EventCampaignDTO eventCampaignDTO);
    }
}