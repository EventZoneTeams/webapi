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
        public async Task<Transaction> Deposit(int userId, decimal amount)
        {
            var transaction = await _unitOfWork.WalletRepository.DepositMoney(userId, WalletTypeEnums.PERSONAL, amount);
            var result = _mapper.Map<Transaction>(transaction);
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

        public async Task<Transaction> ConfirmTransaction(int transactionId)
        {
            var transaction = await _unitOfWork.WalletRepository.ConfirmTransaction(transactionId);
            var result = _mapper.Map<Transaction>(transaction);
            return result;
        }

        // Withdraw money from wallet
    }
}
