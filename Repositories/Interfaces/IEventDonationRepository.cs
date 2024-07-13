using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IEventDonationRepository : IGenericRepository<EventDonation>
    {
        Task<List<EventDonation>> GetAllDonationByCampaignId(int id);
    }
}