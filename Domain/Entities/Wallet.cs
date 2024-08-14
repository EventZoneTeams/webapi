namespace EventZone.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; set; }
        public string WalletType { get; set; }
        public long Balance { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
