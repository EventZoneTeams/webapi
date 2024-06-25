using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Repositories.Interfaces;
using Services.DTO.WalletDTOs;
using Services.Interface;

namespace Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<WalletResponseDTO>> GetListWalletByUserId(int userId)
        {
            var wallets = await _unitOfWork.WalletRepository.GetListWalletByUserId(userId);
            var result = _mapper.Map<List<WalletResponseDTO>>(wallets);
            return result;
        }

        public async Task<WalletResponseDTO> GetWalletByUserIdAndType(int userId, WalletTypeEnums walletType)
        {
            var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAndType(userId, walletType);
            var result = _mapper.Map<WalletResponseDTO>(wallet);
            return result;
        }
        // Deposit money to wallet
        public async Task<Transaction> Deposit(int userId, Int64 amount)
        {
            var transaction = await _unitOfWork.WalletRepository.DepositMoney(userId, WalletTypeEnums.PERSONAL, amount);
            var result = _mapper.Map<Transaction>(transaction);
            return result;
        }

        // Purchase
        public async Task<TransactionResponsesDTO> PurchaseOrder(int orderId, int userId)
        {
            var order = await _unitOfWork.EventOrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if (order.UserId != userId)
            {
                throw new Exception("You are not owner of this order");
            }
            if (order.Status == EventOrderStatusEnums.PAID.ToString())
            {
                throw new Exception("This order has been paid already");
            }
            if (order.Status == EventOrderStatusEnums.CANCELLED.ToString())
            {
                throw new Exception("This order has been cancelled");
            }
            var wallet = await _unitOfWork.WalletRepository.GetListWalletByUserId(userId);
            var personalWallet = wallet.FirstOrDefault(x => x.WalletType == WalletTypeEnums.PERSONAL.ToString());
            if (personalWallet == null)
            {
                throw new Exception("Peronsal Wallet not found");
            }

            // Check balance
            if (personalWallet.Balance < order.TotalAmount)
            {
                throw new Exception("You dont have enough money to purchase this order");
            }

            var transation = await _unitOfWork.WalletRepository.PurchaseItem(userId, orderId);
            var result = _mapper.Map<TransactionResponsesDTO>(transation);
            return result;
        }

        public async Task<List<TransactionResponsesDTO>> GetTransactions(int walletId)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
            var filterTransactions = transactions.Where(x => x.WalletId == walletId).ToList();
            var result = _mapper.Map<List<TransactionResponsesDTO>>(filterTransactions);
            return result;
        }
        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            var result = _mapper.Map<Transaction>(transaction);
            return result;
        }

        public async Task<TransactionResponsesDTO> ConfirmTransaction(int transactionId)
        {
            var transaction = await _unitOfWork.WalletRepository.ConfirmTransaction(transactionId);
            var result = _mapper.Map<TransactionResponsesDTO>(transaction);
            return result;
        }

        // Withdraw money from wallet
    }
}
