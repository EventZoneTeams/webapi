namespace EventZone.Domain.DTOs.WalletDTOs
{
    public class WalletResponseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string WalletType { get; set; }
        public long Balance { get; set; }
    }
}
