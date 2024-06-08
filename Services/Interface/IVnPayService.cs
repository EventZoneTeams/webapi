using Services.DTO.WalletDTOs;

namespace Services.Interface
{
    public interface IVnPayService
    {
        string CreateLink(VnpayOrderInfo orderInfo);
    }
}
