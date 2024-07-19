using Domain.Entities;
using Domain.Enums;

namespace Repositories.Interfaces
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<List<Wallet>> GetListWalletByUserId(int userId);
        Task<Wallet> GetWalletByUserIdAndType(int userId, WalletTypeEnums walletType);

        Task<Transaction> ConfirmTransaction(int transactionId);
        Task<Transaction> DepositMoney(int userId, WalletTypeEnums walletType, Int64 amount);
        Task<Transaction> PurchaseItem(int userId, int orderId);
        Task<Transaction> WithdrawMoney(int userId, WalletTypeEnums walletType, Int64 amount);

        Task<Transaction> Donation(int userId, Int64 amount);
        Task<Transaction> ReceiveDonation(int userId, Int64 amount);
    }
}
