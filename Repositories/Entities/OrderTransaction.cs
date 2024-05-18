namespace Repositories.Entities
{
    public class OrderTransaction : BaseEntity
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

        public virtual EventOrder EventOrder { get; set; }
    }
}
