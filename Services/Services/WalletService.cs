using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Repositories.Helper;
using Repositories.Interfaces;
using Services.DTO.WalletDTOs;
using Services.Interface;

namespace Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<List<WalletResponseDTO>> GetListWalletByUserId(Guid userId)
        {
            var wallets = await _unitOfWork.WalletRepository.GetListWalletByUserId(userId);
            var result = _mapper.Map<List<WalletResponseDTO>>(wallets);
            return result;
        }

        public async Task<WalletResponseDTO> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType)
        {
            var wallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAndType(userId, walletType);
            var result = _mapper.Map<WalletResponseDTO>(wallet);
            return result;
        }
        // Deposit money to wallet
        public async Task<Transaction> Deposit(Guid userId, Int64 amount)
        {
            var transaction = await _unitOfWork.WalletRepository.DepositMoney(userId, WalletTypeEnums.PERSONAL, amount);
            var result = _mapper.Map<Transaction>(transaction);
            return result;
        }

        // Purchase
        public async Task<TransactionResponsesDTO> PurchaseOrder(Guid orderId, Guid userId)
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

            // Send notification
            var notification = new Notification
            {
                Title = "Purchase order " + orderId,
                Body = "Amount: " + order.TotalAmount,
                UserId = userId,
                Url = "/profile/orders",
                Sender = "System"
            };

            //await _notificationService.PushNotification(notification);

            // Purchase
            var transation = await _unitOfWork.WalletRepository.PurchaseItem(userId, orderId);

            //Increase money of event owner
            var eventModel = await _unitOfWork.EventRepository.GetByIdAsync(order.EventId);
            var eventOwnerWallet = await _unitOfWork.WalletRepository.GetWalletByUserIdAndType(eventModel.UserId, WalletTypeEnums.PERSONAL);
            eventOwnerWallet.Balance += order.TotalAmount;
            await _unitOfWork.WalletRepository.Update(eventOwnerWallet);
            await _unitOfWork.SaveChangeAsync();

            //Notification to event order
            var notificationToOrganizer = new Notification
            {
                Title = "One person buy your package" + orderId,
                Body = "Amount: " + order.TotalAmount,
                UserId = eventModel.UserId,
                Url = "/dashboard/my-events/" + eventModel.Id + "/orders",
                Sender = "System"
            };
            await _notificationService.PushNotification(notificationToOrganizer);

            var result = _mapper.Map<TransactionResponsesDTO>(transation);
            return result;
        }

        public async Task<List<TransactionResponsesDTO>> GetTransactionsByUserId(Guid userId, WalletRequestTypeEnums walletRequestTypeEnums)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetTransactionsByUserId(userId, walletRequestTypeEnums.ToString());
            var result = _mapper.Map<List<TransactionResponsesDTO>>(transactions);
            return result;
        }
        public async Task<Transaction> GetTransactionById(Guid transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            var result = _mapper.Map<Transaction>(transaction);
            return result;
        }

        public async Task<TransactionResponsesDTO> ConfirmTransaction(Guid transactionId)
        {
            var transaction = await _unitOfWork.WalletRepository.ConfirmTransaction(transactionId);
            var result = _mapper.Map<TransactionResponsesDTO>(transaction);
            return result;
        }

        // Withdraw money from wallet
    }
}
