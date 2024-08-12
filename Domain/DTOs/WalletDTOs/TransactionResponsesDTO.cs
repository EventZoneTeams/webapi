namespace Domain.DTOs.WalletDTOs
{
    public class TransactionResponsesDTO
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}