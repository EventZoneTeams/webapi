using Domain.Entities;
using Repositories.Commons;
using Repositories.Models.EventCampaignModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IEventCampaignRepository : IGenericRepository<EventCampaign>
    {
        Task<List<EventCampaign>> GetAllCampaignByEvent(int id);
        Task<Pagination<EventCampaign>> GetCampaignsByFilterAsync(PaginationParameter paginationParameter, CampaignFilterModel campaignFilter);
    }
}