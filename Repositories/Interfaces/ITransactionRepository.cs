using Domain.Entities;

namespace Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionsByUserId(int userId, string walletRequestTypeEnums);
    }
}
