using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Models.EventCampaignModels;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventCampaignRepository : IGenericRepository<EventCampaign>
    {
        Task<List<EventCampaign>> GetAllCampaignByEvent(Guid id);
        Task<Pagination<EventCampaign>> GetCampaignsByFilterAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilter);
        Task<EventCampaign> GetCampainById(Guid id);
    }
}