using EventZone.Domain.DTOs.WalletDTOs;
using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Helper;

namespace EventZone.Services.Interface
{
    public interface IWalletService
    {
        Task<TransactionResponsesDTO> ConfirmTransaction(Guid transactionId);

        Task<Transaction> Deposit(Guid userId, long amount);

        Task<List<WalletResponseDTO>> GetListWalletByUserId(Guid userId);

        Task<Transaction> GetTransactionById(Guid transactionId);

        Task<List<TransactionResponsesDTO>> GetTransactionsByUserId(Guid userId, WalletRequestTypeEnums walletRequestTypeEnums);

        Task<WalletResponseDTO> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType);

        Task<TransactionResponsesDTO> PurchaseOrder(Guid orderId, Guid userId);
    }
}