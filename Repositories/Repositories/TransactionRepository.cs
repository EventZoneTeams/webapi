using EventZone.Domain.Entities;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public TransactionRepository(StudentEventForumDbContext studentEventForumDbContext, ICurrentTime timeService, IClaimsService claims) : base(studentEventForumDbContext, timeService, claims)

        {
            _context = studentEventForumDbContext;
            _timeService = timeService;
            _claimsService = claims;
        }

        public async Task<List<Transaction>> GetTransactionsByUserId(Guid userId, string walletRequestTypeEnums)
        {
            if (walletRequestTypeEnums == WalletRequestTypeEnums.ALL.ToString())
            {
                return await _context.Transactions.Where(x => x.Wallet.UserId == userId).ToListAsync();
            }
            else if (walletRequestTypeEnums == WalletRequestTypeEnums.PERSONAL.ToString())
            {
                return await _context.Transactions.Where(x => x.Wallet.UserId == userId && x.Wallet.WalletType == WalletRequestTypeEnums.PERSONAL.ToString()).ToListAsync();
            }
            else if (walletRequestTypeEnums == WalletRequestTypeEnums.ORGANIZATIONAL.ToString())
            {
                return await _context.Transactions.Where(x => x.Wallet.UserId == userId && x.Wallet.WalletType == WalletRequestTypeEnums.ORGANIZATIONAL.ToString()).ToListAsync();
            }
            else
            {
                throw new Exception("Invalid wallet type");
            }

        }
    }
}
