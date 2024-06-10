using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;
using Repositories.Utils;
using Services.DTO.WalletDTOs;
using Services.Interface;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Services.Services.VnPayConfig
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;

        public VnPayService(IConfiguration configuration, IClaimsService claimsService, ICurrentTime currentTime, IUnitOfWork unitOfWork, IWalletService walletService)
        {
            _configuration = configuration;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _unitOfWork = unitOfWork;
            _walletService = walletService;
        }

        public SortedList<string, string> requestData
           = new SortedList<string, string>(new VnPayCompare());
        public SortedList<string, string> responseData
            = new SortedList<string, string>(new VnPayCompare());

        public string CreateLink(VnpayOrderInfo orderInfo)
        {
            var vnp_ReturnUrl = _configuration["Vnpay:ReturnUrl"];
            var vnp_PaymentUrl = _configuration["Vnpay:PaymentUrl"];
            var vnp_TmnCode = _configuration["Vnpay:TmnCode"];
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var vnp_Version = _configuration["Vnpay:Version"];

            this.vnp_Version = vnp_Version;
            vnp_Amount = $"{(int)orderInfo.Amount * 100}";
            vnp_Command = "pay";
            vnp_CreateDate = _currentTime.GetCurrentTime().ToString("yyyyMMddHHmmss");
            vnp_CurrCode = "VND";
            vnp_IpAddr = _claimsService.IpAddress; // LẤY RA IP ADDRESS TRONG CLAIMS
            vnp_Locale = "vn";
            vnp_OrderInfo = "Deposit " + orderInfo.Amount + " into wallet with transaction id: " + orderInfo.TransactionId;
            vnp_OrderType = "other";
            this.vnp_ReturnUrl = vnp_ReturnUrl;
            this.vnp_TmnCode = vnp_TmnCode;
            vnp_TxnRef = orderInfo.TransactionId.ToString();
            vnp_ExpireDate = _currentTime.GetCurrentTime().AddMinutes(15).ToString("yyyyMMddHHmmss");
            vnp_BankCode = "NCB";

            // Add request data to SortedList
            BindRequestData();

            return GetLink(vnp_PaymentUrl, vnp_HashSecret);
        }

        private string GetLink(string baseUrl, string secretKey)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string result = baseUrl + "?" + data.ToString();
            var secureHash = HashHelper.HmacSHA512(secretKey, data.ToString().Remove(data.Length - 1, 1));
            return result += "vnp_SecureHash=" + secureHash;
        }

        public async Task<string> IPNReceiver(string vnpTmnCode, string vnpSecureHash, string vnpTxnRef,
            string vnpTransactionStatus, string vnpResponseCode, string vnpTransactionNo, string vnpBankCode,
            string vnpAmount, string vnpPayDate, string vnpBankTranNo, string vnpCardType,
            NameValueCollection requestNameValue)
        {
            foreach (string s in requestNameValue)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    responseData.Add(s, requestNameValue[s]);
                }
            }

            // Get HashSecret from env
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];

            var checkSignature = ValidateSignature(vnpSecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                var txnId = 0;
                try
                {
                    txnId = int.Parse(vnpTxnRef);
                }
                catch (Exception ex)
                {
                    return "69";
                }
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(txnId);
                if (transaction == null)
                {
                    //payment not found
                    return "01";
                }
                if (transaction.Status == TransactionStatusEnums.SUCCESS.ToString())
                {
                    //payment paid
                    return "02";
                }
                if (transaction.Status == TransactionStatusEnums.FAILED.ToString())
                {
                    //payment failed
                    return "03";
                }

                if (vnpResponseCode == "00") //giao dịch thành công
                {
                    await UpdateStatusTransaction(transaction);
                }
                return "00";
            }
            //Invalid signature
            return "97";
        }

        private async Task UpdateStatusTransaction(Transaction transaction)
        {
            if (Enum.IsDefined(typeof(TransactionTypeEnums), transaction.TransactionType.ToUpper()))
            {
                switch ((TransactionTypeEnums)Enum.Parse(typeof(TransactionTypeEnums), transaction.TransactionType))
                {
                    case TransactionTypeEnums.DEPOSIT:
                        await _walletService.ConfirmTransaction(transaction.Id);
                        break;
                    case TransactionTypeEnums.WITHDRAW:
                        await _walletService.ConfirmTransaction(transaction.Id);
                        break;
                    case TransactionTypeEnums.PURCHASE:
                        await _walletService.ConfirmTransaction(transaction.Id);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Handle values not in enum here if needed
                throw new InvalidOperationException("Transaction type is not valid");
            }
        }

        private bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = HashHelper.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            if (responseData.ContainsKey("vnp_SecureHashType"))
            {
                responseData.Remove("vnp_SecureHashType");
            }
            if (responseData.ContainsKey("vnp_SecureHash"))
            {
                responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        private void BindRequestData()
        {
            if (vnp_Amount != null)
                requestData.Add("vnp_Amount", vnp_Amount.ToString() ?? string.Empty);
            if (vnp_Command != null)
                requestData.Add("vnp_Command", vnp_Command);
            if (vnp_CreateDate != null)
                requestData.Add("vnp_CreateDate", vnp_CreateDate);
            if (vnp_CurrCode != null)
                requestData.Add("vnp_CurrCode", vnp_CurrCode);
            if (vnp_BankCode != null)
                requestData.Add("vnp_BankCode", vnp_BankCode);
            if (vnp_IpAddr != null)
                requestData.Add("vnp_IpAddr", vnp_IpAddr);
            if (vnp_Locale != null)
                requestData.Add("vnp_Locale", vnp_Locale);
            if (vnp_OrderInfo != null)
                requestData.Add("vnp_OrderInfo", vnp_OrderInfo);
            if (vnp_OrderType != null)
                requestData.Add("vnp_OrderType", vnp_OrderType);
            if (vnp_ReturnUrl != null)
                requestData.Add("vnp_ReturnUrl", vnp_ReturnUrl);
            if (vnp_TmnCode != null)
                requestData.Add("vnp_TmnCode", vnp_TmnCode);
            if (vnp_ExpireDate != null)
                requestData.Add("vnp_ExpireDate", vnp_ExpireDate);
            if (vnp_TxnRef != null)
                requestData.Add("vnp_TxnRef", vnp_TxnRef);
            if (vnp_Version != null)
                requestData.Add("vnp_Version", vnp_Version);
        }
        public string? vnp_Amount { get; set; }
        public string? vnp_Command { get; set; }
        public string? vnp_CreateDate { get; set; }
        public string? vnp_CurrCode { get; set; }
        public string? vnp_BankCode { get; set; }
        public string? vnp_IpAddr { get; set; }
        public string? vnp_Locale { get; set; }
        public string? vnp_OrderInfo { get; set; }
        public string? vnp_OrderType { get; set; }
        public string? vnp_ReturnUrl { get; set; }
        public string? vnp_TmnCode { get; set; }
        public string? vnp_ExpireDate { get; set; }
        public string? vnp_TxnRef { get; set; }
        public string? vnp_Version { get; set; }
    }
}
