using Domain.Entities;

namespace Repositories.Interfaces
{
    public interface IEventDonationRepository : IGenericRepository<EventDonation>
    {
        Task<List<EventDonation>> GetAllDonationByCampaignId(Guid id);
        Task<List<EventDonation>> GetMyDonation();
    }
}