using EventZone.Domain.Entities;

namespace EventZone.Services.Interface
{
    public interface IWithdrawnRequestService
    {
        Task<WithdrawnRequest> CreateARequest(WithdrawnRequest request);
        Task<List<WithdrawnRequest>> GetRequestByUserId();
        Task<List<WithdrawnRequest>> GetAllRequest();
        Task<WithdrawnRequest> ApproveRequest(Guid requestId, string imageUrl);
        Task<WithdrawnRequest> RejectRequest(Guid requestId);
    }
}
