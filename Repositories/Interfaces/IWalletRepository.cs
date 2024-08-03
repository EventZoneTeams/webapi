using Domain.Entities;
using Domain.Enums;

namespace Repositories.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<List<Wallet>> GetListWalletByUserId(Guid userId);
        Task<Wallet> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType);

        Task<Transaction> ConfirmTransaction(Guid transactionId);
        Task<Transaction> DepositMoney(Guid userId, WalletTypeEnums walletType, Int64 amount);
        Task<Transaction> PurchaseItem(Guid userId, Guid orderId);
        Task<Transaction> WithdrawMoney(Guid userId, WalletTypeEnums walletType, Int64 amount);

        Task<Transaction> Donation(Guid userId, Int64 amount);
        Task<Transaction> ReceiveDonation(Guid userId, Int64 amount);
    }
}
