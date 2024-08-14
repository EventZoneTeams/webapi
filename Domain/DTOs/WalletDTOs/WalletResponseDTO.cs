namespace EventZone.Domain.DTOs.WalletDTOs
{
    public class WalletResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string WalletType { get; set; }
        public long Balance { get; set; }
    }
}
