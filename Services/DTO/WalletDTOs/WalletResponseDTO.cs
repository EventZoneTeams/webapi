namespace Services.DTO.WalletDTOs
{
    public class WalletResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string WalletType { get; set; }
        public decimal Balance { get; set; }
    }
}
