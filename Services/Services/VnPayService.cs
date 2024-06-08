using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;
using Repositories.Utils;
using Services.DTO.WalletDTOs;
using Services.Interface;
using System.Net;
using System.Text;

namespace Services.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;

        public VnPayService(IConfiguration configuration, IClaimsService claimsService, ICurrentTime currentTime)
        {
            _configuration = configuration;
            _claimsService = claimsService;
            _currentTime = currentTime;
        }

        public SortedList<string, string> requestData
           = new SortedList<string, string>(new VnpayCompare());

        public string CreateLink(VnpayOrderInfo orderInfo)
        {
            var vnp_ReturnUrl = _configuration["Vnpay:ReturnUrl"];
            var vnp_PaymentUrl = _configuration["Vnpay:PaymentUrl"];
            var vnp_TmnCode = _configuration["Vnpay:TmnCode"];
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var vnp_Version = _configuration["Vnpay:Version"];

            this.vnp_Version = vnp_Version;
            this.vnp_Amount = orderInfo.Amount * 100;
            this.vnp_Command = "pay";
            this.vnp_CreateDate = _currentTime.GetCurrentTime().ToString("yyyyMMddHHmmss");
            this.vnp_CurrCode = "VND";
            this.vnp_IpAddr = _claimsService.IpAddress; // LẤY RA IP ADDRESS TRONG CLAIMS
            this.vnp_Locale = "vn";
            this.vnp_OrderInfo = "Deposit " + orderInfo.Amount + " into wallet with transaction id: " + orderInfo.TransactionId;
            this.vnp_OrderType = "other";
            this.vnp_ReturnUrl = vnp_ReturnUrl;
            this.vnp_TmnCode = vnp_TmnCode;
            this.vnp_TxnRef = orderInfo.TransactionId.ToString();
            this.vnp_ExpireDate = _currentTime.GetCurrentTime().AddMinutes(15).ToString("yyyyMMddHHmmss");
            this.vnp_BankCode = string.Empty;

            return GetLink(vnp_PaymentUrl, vnp_HashSecret);
        }

        public string GetLink(string baseUrl, string secretKey)
        {
            MakeRequestData();
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string result = baseUrl + "?" + data.ToString();
            var secureHash = HashHelper.HmacSHA512(secretKey, data.ToString().Remove(data.Length - 1, 1));
            return result += "vnp_SecureHash=" + secureHash;
        }

        public void MakeRequestData()
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
        public decimal? vnp_Amount { get; set; }
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
