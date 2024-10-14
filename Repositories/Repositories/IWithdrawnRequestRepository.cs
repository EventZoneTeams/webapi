using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;

namespace EventZone.Repositories.Repositories
{
    public interface IWithdrawnRequestRepository : IGenericRepository<WithdrawnRequest>
    {
        Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request);
        Task<List<WithdrawnRequest>> GetRequestByUserId();
        Task<List<WithdrawnRequest>> GetAllRequest();
        Task<WithdrawnRequest> ApproveRequest(Guid requestId, string imageUrl);
        Task<WithdrawnRequest> RejectRequest(Guid requestId);
    }
}