using Domain.Entities;
using Repositories.Commons;
using Repositories.Models.EventCampaignModels;

namespace Repositories.Interfaces
{
    public interface IEventCampaignRepository : IGenericRepository<EventCampaign>
    {
        Task<List<EventCampaign>> GetAllCampaignByEvent(Guid id);
        Task<Pagination<EventCampaign>> GetCampaignsByFilterAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilter);
        Task<EventCampaign> GetCampainById(Guid id);
    }
}