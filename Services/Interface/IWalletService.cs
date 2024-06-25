using Domain.Entities;
using Domain.Enums;
using Services.DTO.WalletDTOs;

namespace Services.Interface
{
    public interface IWalletService
    {
        Task<TransactionResponsesDTO> ConfirmTransaction(int transactionId);
        Task<Transaction> Deposit(int userId, Int64 amount);
        Task<List<WalletResponseDTO>> GetListWalletByUserId(int userId);
        Task<Transaction> GetTransactionById(int transactionId);
        Task<List<TransactionResponsesDTO>> GetTransactions(int userId);
        Task<WalletResponseDTO> GetWalletByUserIdAndType(int userId, WalletTypeEnums walletType);
        Task<TransactionResponsesDTO> PurchaseOrder(int orderId, int userId);
    }
}
