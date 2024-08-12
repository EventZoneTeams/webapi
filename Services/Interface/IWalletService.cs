using Domain.DTOs.WalletDTOs;
using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;

namespace Services.Interface
{
    public interface IWalletService
    {
        Task<TransactionResponsesDTO> ConfirmTransaction(Guid transactionId);

        Task<Transaction> Deposit(Guid userId, Int64 amount);

        Task<List<WalletResponseDTO>> GetListWalletByUserId(Guid userId);

        Task<Transaction> GetTransactionById(Guid transactionId);

        Task<List<TransactionResponsesDTO>> GetTransactionsByUserId(Guid userId, WalletRequestTypeEnums walletRequestTypeEnums);

        Task<WalletResponseDTO> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType);

        Task<TransactionResponsesDTO> PurchaseOrder(Guid orderId, Guid userId);
    }
}