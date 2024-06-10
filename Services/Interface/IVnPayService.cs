using Services.DTO.WalletDTOs;
using System.Collections.Specialized;

namespace Services.Interface
{
    public interface IVnPayService
    {
        string CreateLink(VnpayOrderInfo orderInfo);
        Task<string> IPNReceiver(string vnpTmnCode, string vnpSecureHash, string vnpTxnRef, string vnpTransactionStatus, string vnpResponseCode, string vnpTransactionNo, string vnpBankCode, string vnpAmount, string vnpPayDate, string vnpBankTranNo, string vnpCardType, NameValueCollection requestNameValue);
    }
}
