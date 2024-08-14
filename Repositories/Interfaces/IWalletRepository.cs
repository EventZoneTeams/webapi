using EventZone.Domain.Entities;
using EventZone.Domain.Enums;

namespace EventZone.Repositories.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<List<Wallet>> GetListWalletByUserId(Guid userId);
        Task<Wallet> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType);

        Task<Transaction> ConfirmTransaction(Guid transactionId);
        Task<Transaction> DepositMoney(Guid userId, WalletTypeEnums walletType, long amount);
        Task<Transaction> PurchaseItem(Guid userId, Guid orderId);
        Task<Transaction> WithdrawMoney(Guid userId, WalletTypeEnums walletType, long amount);

        Task<Transaction> Donation(Guid userId, long amount);
        Task<Transaction> ReceiveDonation(Guid userId, long amount);
    }
}
