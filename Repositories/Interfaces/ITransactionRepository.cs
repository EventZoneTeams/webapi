using EventZone.Domain.Entities;

namespace EventZone.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionsByUserId(Guid userId, string walletRequestTypeEnums);
    }
}
