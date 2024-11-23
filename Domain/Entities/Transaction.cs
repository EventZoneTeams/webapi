namespace EventZone.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? OrderCode { get; set; }
        public string Status { get; set; }
        public Wallet Wallet { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
