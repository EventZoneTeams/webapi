﻿using EventZone.Domain.Enums;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EventZone.Services.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayOSService> _logger;
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;

        public PayOSService(ILogger<PayOSService> logger, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _configuration = configuration;

            var ClientId = _configuration["PayOS:ClientId"];
            var ChecksumKey = _configuration["PayOS:ChecksumKey"];
            var ApiKey = _configuration["PayOS:ApiKey"];
            _payOS = new PayOS(ClientId, ApiKey, ChecksumKey);
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreateLink(int depositMoney, Guid txnRef)
        {
            var domain = "https://eventzone.id.vn/payment";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: depositMoney,
                description: txnRef.ToString().Substring(0, 8),
                items: [new("Nạp tiền " + depositMoney, 1, depositMoney)],
                returnUrl: domain + "?success=true&transactionId=" + "GG" + "&amount=" + depositMoney,
                cancelUrl: domain + "?canceled=true&transactionId=" + "GG" + "&amount=" + depositMoney
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }

        public async Task<WebhookResponse> ReturnWebhook(WebhookType webhookType)
        {
            try
            {
                // Log the receipt of the webhook
                //Seriablize the object to log
                _logger.LogInformation(JsonConvert.SerializeObject(webhookType));

                //WebhookData verifiedData = _payOS.verifyPaymentWebhookData(webhookType); //xác thực data from webhook
                //string responseCode = verifiedData.code;
                //string orderCode = verifiedData.orderCode.ToString();
                //string transactionId = "TRANS" + orderCode;

                //Get First transaction contains 8 first character
                var transactionss = await _unitOfWork.TransactionRepository.GetAllAsync();
                var transaction = transactionss.FirstOrDefault(x => x.Id.ToString().Substring(0, 8).Equals(webhookType.data.description.ToString()));

                // Handle the webhook based on the transaction status
                switch (webhookType.data.code)
                {
                    case "00":
                        // Update the transaction status
                        transaction.Status = TransactionStatusEnums.SUCCESS.ToString();
                        transaction.Description = "Nạp tiền thành công";
                        await _unitOfWork.TransactionRepository.Update(transaction);
                        await _unitOfWork.SaveChangeAsync();

                        return new WebhookResponse
                        {
                            Success = true,
                            Note = "Payment processed successfully"
                        };

                    case "01":
                        // Update the transaction status
                        transaction.Status = TransactionStatusEnums.FAILED.ToString();
                        transaction.Description = "Payment failed: Invalid parameters";
                        await _unitOfWork.TransactionRepository.Update(transaction);
                        await _unitOfWork.SaveChangeAsync();

                        return new WebhookResponse
                        {
                            Success = false,
                            Note = "Invalid parameters"
                        };

                    default:
                        return new WebhookResponse
                        {
                            Success = false,
                            Note = "Unhandled code"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }

    public static class PayOSUtils
    {
        public static bool IsValidData(WebhookType payOSWebhook, string transactionSignature, string ChecksumKey)
        {
            try
            {
                JObject jsonObject = JObject.Parse(payOSWebhook.data.ToString().Replace("'", "\""));
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
                return signature.Equals(transactionSignature, StringComparison.OrdinalIgnoreCase);
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

    public class DepositRequest
    {
        public int Amount { get; set; }
    }

    public class WebhookResponse
    {
        public bool Success { get; set; }
        public string Note { get; set; }
    }

}