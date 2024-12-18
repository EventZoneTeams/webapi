﻿using EventZone.Domain.Entities;
using EventZone.Domain.Enums;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public WalletRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<Wallet>> GetListWalletByUserId(Guid userId)
        {
            //check user is exist
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            //get list wallet by userId
            var wallets = await _context.Wallets.Where(x => x.UserId == userId).ToListAsync();

            //check if wallet with type personal is null, create new personal wallet
            var personalWallet = wallets.FirstOrDefault(x => x.WalletType.ToUpper() == WalletTypeEnums.PERSONAL.ToString().ToUpper());
            if (personalWallet == null)
            {
                var newPersonalWallet = new Wallet
                {
                    UserId = userId,
                    WalletType = WalletTypeEnums.PERSONAL.ToString(),
                    Balance = 0
                };
                await AddAsync(newPersonalWallet);
                wallets.Add(newPersonalWallet);
            }

            //check if wallet with type organization is null, create new organization wallet
            var organizationWallet = wallets.FirstOrDefault(x => x.WalletType.ToUpper() == WalletTypeEnums.ORGANIZATIONAL.ToString().ToUpper());
            if (organizationWallet == null)
            {
                var newOrganizationWallet = new Wallet
                {
                    UserId = userId,
                    WalletType = WalletTypeEnums.ORGANIZATIONAL.ToString(),
                    Balance = 0
                };
                await AddAsync(newOrganizationWallet);
                wallets.Add(newOrganizationWallet);
            }

            await _context.SaveChangesAsync();

            return wallets;
        }

        public async Task<Wallet> GetWalletByUserIdAndType(Guid userId, WalletTypeEnums walletType)
        {
            var wallets = await GetListWalletByUserId(userId);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == userId && x.WalletType.ToUpper() == walletType.ToString().ToUpper());
            return wallet;
        }

        // Deposit money to wallet
        public async Task<Transaction> DepositMoney(Guid userId, WalletTypeEnums walletType, long amount, string? paymentMethod = "VNPay")
        {
            var wallet = await GetWalletByUserIdAndType(userId, walletType);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            //Create new transaction
            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                TransactionType = TransactionTypeEnums.DEPOSIT.ToString(),
                Amount = amount,
                Description = "Deposit money with amount: " + amount + ", Thanh toán qua: " + paymentMethod,
                TransactionDate = DateTime.UtcNow.AddHours(7),
                CreatedAt = _timeService.GetCurrentTime(),
                Status = TransactionStatusEnums.PENDING.ToString(),
                OrderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"))
            };
            //add transaction to database
            await _context.Transactions.AddAsync(transaction);

            await _context.SaveChangesAsync();
            return transaction;
        }

        // Withdraw money from wallet
        public async Task<Transaction> WithdrawMoney(Guid userId, WalletTypeEnums walletType, long amount)
        {
            var wallet = await GetWalletByUserIdAndType(userId, walletType);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (wallet.Balance < amount)
            {
                throw new Exception("Balance is not enough");
            }

            //Create new transaction
            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                TransactionType = TransactionTypeEnums.WITHDRAW.ToString(),
                Amount = amount,
                Description = "Withdraw money with amount: " + amount,
                TransactionDate = DateTime.UtcNow.AddHours(7),
                CreatedAt = _timeService.GetCurrentTime(),
                Status = TransactionStatusEnums.PENDING.ToString()
            };
            //add transaction to database
            await _context.Transactions.AddAsync(transaction);
            return transaction;
        }

        // Purchase item and deduct money from wallet
        public async Task<Transaction> PurchaseItem(Guid userId, Guid orderId)
        {
            //Get order
            var order = await _context.EventOrders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if (order.UserId != userId)
            {
                throw new Exception("Order not belong to user");
            }
            if (order.Status == EventOrderStatusEnums.PAID.ToString())
            {
                throw new Exception("This order has been paid already");
            }
            if (order.Status == EventOrderStatusEnums.CANCELLED.ToString())
            {
                throw new Exception("This order has been cancelled");
            }

            var wallet = await GetListWalletByUserId(userId);
            var personalWallet = wallet.FirstOrDefault(x => x.WalletType.ToUpper() == WalletTypeEnums.PERSONAL.ToString().ToUpper());
            if (personalWallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (personalWallet.Balance < order.TotalAmount)
            {
                throw new Exception("Balance is not enough");
            }

            using (var txn = _context.Database.BeginTransaction())
            {
                try
                {
                    //Create new transaction
                    var transaction = new Transaction
                    {
                        WalletId = personalWallet.Id,
                        TransactionType = TransactionTypeEnums.PURCHASE.ToString(),
                        Amount = order.TotalAmount,
                        Description = "Purchase order with amount: " + order.TotalAmount,
                        TransactionDate = _timeService.GetCurrentTime(),
                        CreatedAt = _timeService.GetCurrentTime(),
                        Status = TransactionStatusEnums.SUCCESS.ToString()
                    };

                    //add transaction to database
                    await _context.Transactions.AddAsync(transaction);
                    await _context.SaveChangesAsync();

                    //Create new transaction detail
                    var transactionDetail = new TransactionDetail
                    {
                        TransactionId = transaction.Id,
                        EventOrderId = order.Id,
                    };
                    //add transaction detail to database
                    await _context.TransactionDetails.AddAsync(transactionDetail);
                    await _context.SaveChangesAsync();

                    //Update wallet balance
                    personalWallet.Balance -= order.TotalAmount;
                    if (personalWallet.Balance < 0)
                    {
                        throw new Exception("PURCHASE: Balance is not enough");
                    }
                    _context.Entry(personalWallet).State = EntityState.Modified;

                    //update order status
                    order.Status = EventOrderStatusEnums.PAID.ToString();
                    _context.Entry(order).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    await txn.CommitAsync();
                    return transaction;
                }
                catch (Exception ex)
                {
                    await txn.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<Transaction> ConfirmTransaction(Guid transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found");
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.Id == transaction.WalletId);

            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            //Check transaction type
            if (Enum.IsDefined(typeof(TransactionTypeEnums), transaction.TransactionType))
            {
                switch ((TransactionTypeEnums)Enum.Parse(typeof(TransactionTypeEnums), transaction.TransactionType.ToUpper()))
                {
                    case TransactionTypeEnums.DEPOSIT:
                        wallet.Balance += transaction.Amount;
                        break;

                    case TransactionTypeEnums.WITHDRAW:
                        wallet.Balance -= transaction.Amount;
                        if (wallet.Balance < 0)
                        {
                            throw new Exception("WITHDRAW: Balance is not enough");
                        }
                        break;

                    case TransactionTypeEnums.PURCHASE:
                        wallet.Balance -= transaction.Amount;
                        if (wallet.Balance < 0)
                        {
                            throw new Exception("PURCHASE: Balance is not enough");
                        }
                        //Update order status
                        var transactionDetail = await _context.TransactionDetails.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
                        var order = await _context.EventOrders.FirstOrDefaultAsync(x => x.Id == transactionDetail.EventOrderId);
                        order.Status = EventOrderStatusEnums.PAID.ToString();
                        _context.EventOrders.Update(order);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                // Handle values not in enum here if needed
                throw new InvalidOperationException("Transaction type is not valid");
            }

            transaction.Status = TransactionStatusEnums.SUCCESS.ToString();
            _context.Wallets.Update(wallet);
            _context.Transactions.Update(transaction);

            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> Donation(Guid userId, long amount)
        {
            var wallet = await GetWalletByUserIdAndType(userId, WalletTypeEnums.PERSONAL);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (wallet.Balance < amount)
            {
                throw new Exception("Balance is not enough");
            }

            //Create new transaction
            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                TransactionType = TransactionTypeEnums.DONATION.ToString(),
                Amount = amount,
                Description = "DONATION money with amount: " + amount,
                TransactionDate = _timeService.GetCurrentTime(),
                CreatedAt = _timeService.GetCurrentTime(),
                Status = TransactionStatusEnums.SUCCESS.ToString()
            };
            //add transaction to database
            await _context.Transactions.AddAsync(transaction);
            return transaction;
        }

        public async Task<Transaction> ReceiveDonation(Guid userId, long amount)
        {
            var wallet = await GetWalletByUserIdAndType(userId, WalletTypeEnums.PERSONAL);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            //Create new transaction
            var transaction = new Transaction
            {
                WalletId = wallet.Id,
                TransactionType = TransactionTypeEnums.DONATION.ToString(),
                Amount = amount,
                Description = "RECEIVE DONATION money with amount: " + amount,
                TransactionDate = _timeService.GetCurrentTime(),
                CreatedAt = _timeService.GetCurrentTime(),
                Status = TransactionStatusEnums.SUCCESS.ToString()
            };
            //add transaction to database
            await _context.Transactions.AddAsync(transaction);
            return transaction;
        }
    }
}