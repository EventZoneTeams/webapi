using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;
using Services.DTO.WalletDTOs;

namespace Services.Interface
{
    public interface IWalletService
    {
        Task<TransactionResponsesDTO> ConfirmTransaction(int transactionId);
        Task<Transaction> Deposit(int userId, Int64 amount);
        Task<List<WalletResponseDTO>> GetListWalletByUserId(int userId);
        Task<Transaction> GetTransactionById(int transactionId);
        Task<List<TransactionResponsesDTO>> GetTransactionsByUserId(int userId, WalletRequestTypeEnums walletRequestTypeEnums);
        Task<WalletResponseDTO> GetWalletByUserIdAndType(int userId, WalletTypeEnums walletType);
        Task<TransactionResponsesDTO> PurchaseOrder(int orderId, int userId);
    }
}
