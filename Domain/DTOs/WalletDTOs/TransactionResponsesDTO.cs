namespace EventZone.Domain.DTOs.WalletDTOs
{
    public class TransactionResponsesDTO
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}