using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface IEventDonationRepository : IGenericRepository<EventDonation>
    {
        Task<List<EventDonation>> GetAllDonationByCampaignId(Guid id);
        Task<List<EventDonation>> GetMyDonation();
    }
}