using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using static EventZone.Services.Services.PayOSObjects;

namespace EventZone.Services.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayOSService> _logger;
        private readonly PayOS _payOS;

        public PayOSService()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var ClientId = configuration["PayOS:ClientId"];
            var ChecksumKey = configuration["PayOS:ChecksumKey"];
            var ApiKey = configuration["PayOS:ApiKey"];
            _payOS = new PayOS(ClientId, ApiKey, ChecksumKey);
        }

        public async Task<string> CreateLink(int depositMoney)
        {
            var domain = "https://eventzone.id.vn/payment";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: 2000,
                description: "Nạp tiền: " + depositMoney,
                items: [new("Nạp tiền " + depositMoney, 1, depositMoney)],
                returnUrl: domain + "?success=true&transactionId=" + "GG" + "&amount=" + depositMoney,
                cancelUrl: domain + "?canceled=true&transactionId=" + "GG" + "&amount=" + depositMoney
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }

        public async Task<PayOSWebhookResponse> ReturnWebhook(PayOSWebhook payOSWebhook)
        {
            // Log the receipt of the webhook
            _logger.LogInformation("Received webhook with Code: {Code}, Success: {Success}", payOSWebhook.Code, payOSWebhook.Success);

            // Validate the webhook signature
            if (!PayOSUtils.IsValidData(payOSWebhook, payOSWebhook.Signature))
            {
                _logger.LogWarning("Invalid webhook signature for OrderCode: {OrderCode}", payOSWebhook.Data.OrderCode);
                return new PayOSWebhookResponse
                {
                    Success = false,
                    Note = "Invalid signature"
                };
            }

            // Log the validated data
            _logger.LogInformation("Valid webhook data: OrderCode: {OrderCode}, Amount: {Amount}, Status: {Code}",
                payOSWebhook.Data.OrderCode,
                payOSWebhook.Data.Amount,
                payOSWebhook.Code);

            // Handle the webhook based on the transaction status
            switch (payOSWebhook.Code)
            {
                case "00":
                    _logger.LogInformation("Payment successful for OrderCode: {OrderCode}", payOSWebhook.Data.OrderCode);

                    // Example: Update the order in the system, mark it as paid
                    //var order = await _unitOfWork.WalletRepository.ConfirmTransaction(payOSWebhook.Data.OrderCode);

                    return new PayOSWebhookResponse
                    {
                        Success = true,
                        Note = "Payment processed successfully"
                    };

                case "01":
                    _logger.LogError("Invalid parameters in the webhook for OrderCode: {OrderCode}", payOSWebhook.Data.OrderCode);
                    return new PayOSWebhookResponse
                    {
                        Success = false,
                        Note = "Invalid parameters"
                    };

                default:
                    _logger.LogWarning("Unhandled webhook code: {Code} for OrderCode: {OrderCode}", payOSWebhook.Code, payOSWebhook.Data.OrderCode);
                    return new PayOSWebhookResponse
                    {
                        Success = false,
                        Note = "Unhandled code"
                    };
            }
        }
    }

    public static class PayOSUtils
    {
        public static bool IsValidData(PayOSWebhook payOSWebhook, string transactionSignature)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
                var ChecksumKey = configuration["PayOS:ChecksumKey"];

                JObject jsonObject = JObject.Parse(payOSWebhook.Data.ToString().Replace("'", "\""));
                var sortedKeys = jsonObject.Properties().Select(p => p.Name).OrderBy(k => k).ToList();

                StringBuilder transactionStr = new StringBuilder();
                foreach (var key in sortedKeys)
                {
                    string value = jsonObject[key]?.ToString() ?? string.Empty;
                    transactionStr.Append($"{key}={value}");
                    if (key != sortedKeys.Last())
                    {
                        transactionStr.Append("&");
                    }
                }

                string signature = ComputeHmacSha256(transactionStr.ToString(), ChecksumKey);
                return signature.Equals(payOSWebhook.Signature, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public static string ComputeHmacSha256(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
    public class PayOSObjects
    {
        public class PayOSWebhookResponse
        {
            public bool Success { get; set; }
            public PayOSTransaction Data { get; set; }
            public string Note { get; set; }
        }
        public class PayOSWebhook
        {
            public string Code { get; set; }
            public string Desc { get; set; }
            public bool Success { get; set; }
            public PayOSTransaction Data { get; set; }
            public string Signature { get; set; }
        }

        public class PayOSTransaction
        {
            public int OrderCode { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public string AccountNumber { get; set; }
            public string Reference { get; set; }
            public string TransactionDateTime { get; set; }
            public string Currency { get; set; }
            public string PaymentLinkId { get; set; }
            public string Code { get; set; }
            public string Desc { get; set; }
            public string CounterAccountBankId { get; set; }
            public string CounterAccountBankName { get; set; }
            public string CounterAccountName { get; set; }
            public string CounterAccountNumber { get; set; }
            public string VirtualAccountName { get; set; }
            public string VirtualAccountNumber { get; set; }
        }
    }
}
