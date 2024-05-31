namespace Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public int UserId { get; set; }
        public string WalletType { get; set; }
        public decimal Balance { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
