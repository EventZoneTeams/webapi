using AutoMapper;
using EventZone.Domain.DTOs.WalletDTOs;
using EventZone.Domain.Enums;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Helper;
using EventZone.Repositories.Interfaces;
using EventZone.Services.DTO.ResponseModels;
using EventZone.Services.Interface;
using EventZone.Services.Services;
using EventZone.Services.Services.VnPayConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Web;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IPayOSService _payOSService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, IMapper mapper, IVnPayService vnPayService, IClaimsService claimsService, INotificationService notificationService, IPayOSService payOSService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _claimsService = claimsService;
            _notificationService = notificationService;
            _payOSService = payOSService;
            _logger = logger;
        }

        /// <summary>
        /// Get 2 Wallets By UserId
        /// </summary>
        [HttpGet("wallets")]
        public async Task<IActionResult> GetListWalletByUserId()
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == Guid.Empty)
                {
                    throw new Exception("User Id is invalid");
                }
                var result = await _walletService.GetListWalletByUserId(userId);
                return Ok(ApiResult<List<WalletResponseDTO>>.Succeed(result, "Get 2 Wallet Of User with Id " + userId + " Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get List Transactions By UserId
        /// </summary>
        [HttpGet("wallets/transactions")]
        public async Task<IActionResult> GetTransactionsBtUserId([FromQuery] WalletRequestTypeEnums walletTypeEnums)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == Guid.Empty)
                {
                    throw new Exception("User Id is invalid or you are not login");
                }
                var result = await _walletService.GetTransactionsByUserId(userId, walletTypeEnums);
                return Ok(ApiResult<List<TransactionResponsesDTO>>.Succeed(result, "Get Transactions Of User with Id " + userId + " and type is " + walletTypeEnums.ToString() + " Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Create deposit transaction and return direct url
        /// </summary>
        /// <returns>
        ///     URL of payment
        /// </returns>
        [HttpPost("wallets/transactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Deposit([FromBody] DepositRequestDTO depositRequest)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == Guid.Empty)
                {
                    throw new Exception("UserId is invalid or you are not login");
                }
                if (depositRequest.Amount <= 0)
                {
                    throw new Exception("Amount is invalid");
                }
                var result = await _walletService.Deposit(userId, depositRequest.Amount, "VNPay");

                if (result == null)
                {
                    throw new Exception("Create Deposit Transaction Failed!");
                }
                else
                {
                    var orderInfo = new VnpayOrderInfo
                    {
                        Amount = result.Amount,
                        TransactionId = result.Id,
                    };

                    var paymentUrl = _vnPayService.CreateLink(orderInfo);
                    return Ok(ApiResult<string>.Succeed(paymentUrl, "Payment to deposit!"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Create direct url for old pending transaction
        /// </summary>
        /// <returns>
        ///     URL of payment
        /// </returns>
        [HttpPost("payment/{transactionId}/complete-pending")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SubmitOldTransactionToPay(Guid transactionId)
        {
            try
            {
                if (transactionId == Guid.Empty)
                {
                    throw new Exception("TransactionId is invalid");
                }
                var result = await _walletService.GetTransactionById(transactionId);

                if (result == null)
                {
                    throw new Exception("Create Payment Transaction Failed!");
                }
                else
                {
                    if (result.Status == TransactionStatusEnums.SUCCESS.ToString())
                    {
                        throw new Exception("Transaction is already confirmed!");
                    }
                    else if (result.Status == TransactionStatusEnums.FAILED.ToString())
                    {
                        throw new Exception("Transaction is already failed! Please create new transactions");
                    }

                    var orderInfo = new VnpayOrderInfo
                    {
                        Amount = result.Amount,
                        TransactionId = result.Id,
                    };

                    var paymentUrl = _vnPayService.CreateLink(orderInfo);
                    return Ok(ApiResult<string>.Succeed(paymentUrl, "Payment to pay!"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// [DONT'T TOUCH] VnPay IPN Receiver
        /// </summary>
        [HttpGet("payment/vnpay-ipn-receive")]
        public async Task<IActionResult> PaymentReturn([FromQuery] VnpayResponseModel vnpayResponseModel)
        {
            try
            {
                var htmlString = string.Empty;
                var requestNameValue = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());

                IPNReponse iPNReponse = await _vnPayService.IPNReceiver(
                    vnpayResponseModel.vnp_TmnCode,
                    vnpayResponseModel.vnp_SecureHash,
                    vnpayResponseModel.vnp_TxnRef,
                    vnpayResponseModel.vnp_TransactionStatus,
                    vnpayResponseModel.vnp_ResponseCode,
                    vnpayResponseModel.vnp_TransactionNo,
                    vnpayResponseModel.vnp_BankCode,
                    vnpayResponseModel.vnp_Amount,
                    vnpayResponseModel.vnp_PayDate,
                    vnpayResponseModel.vnp_BankTranNo,
                    vnpayResponseModel.vnp_CardType, requestNameValue);


                //Get root path and read index.html
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "index.html");

                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        htmlString = sr.ReadToEnd();
                    }
                }
                string orderInfo = vnpayResponseModel.vnp_OrderInfo ?? "Không có thông tin";
                //format html
                var isSuccess = iPNReponse.status.ToString() == TransactionStatusEnums.SUCCESS.ToString();
                var textColor = isSuccess ? "text-green-500 dark:text-green-300" : "text-red-500 dark:text-red-300";
                var statusHTML = $"<p class=\"mt-1 text-md {textColor}\">{iPNReponse.status.ToString()}</p>";

                // Send notification
                //var notification = new Notification
                //{
                //    Title = "Deposit " + int.Parse(iPNReponse.price) / 100,
                //    Body = iPNReponse.message,
                //    UserId = _claimsService.GetCurrentUserId == Guid.Empty ? Guid.Empty : _claimsService.GetCurrentUserId,
                //    Url = "/profile/wallet",
                //    Sender = "System"
                //};
                //await _notificationService.PushNotification(notification).ConfigureAwait(true);

                //format image
                var imageHTML = string.Empty;
                if (isSuccess)
                {
                    imageHTML = $"<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#00b894] to-[#55efc4] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        <div class=\"text-6xl font-bold star\">✨</div>\r\n                        <!-- <div className=\"text-6xl font-bold hidden\">❌</div> -->\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Successful</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>";
                }
                else
                {
                    imageHTML = "<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#FF4B4B] to-[#FF8B8B] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        \r\n                        <div className=\"text-6xl font-bold hidden\">❌</div>\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Failed</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>>";
                }

                string htmlFormat = string.Format(htmlString, imageHTML, iPNReponse.transactionId.ToString(), $"{int.Parse(iPNReponse.price) / 100}", statusHTML, iPNReponse.message);

                return Content(htmlFormat, "text/html");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString() + "Error", "text/html");
            }
        }

        /// <summary>
        /// Purchase a order
        /// </summary>
        [HttpPost("payment/event-orders/{orderId}")]
        public async Task<IActionResult> PurchaseOrder(Guid orderId)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == Guid.Empty)
                {
                    throw new Exception("User Id is invalid");
                }
                if (orderId == Guid.Empty)
                {
                    throw new Exception("OrderId is invalid");
                }
                var result = await _walletService.PurchaseOrder(orderId, userId);
                if (result.Status == TransactionStatusEnums.FAILED.ToString())
                {
                    throw new Exception("Purchase Order Failed!");
                }

                if (result.Status == TransactionStatusEnums.PENDING.ToString())
                {
                    throw new Exception("Purchase Order Pending!");
                }

                if (result.Status == TransactionStatusEnums.SUCCESS.ToString())
                {
                    return Ok(ApiResult<TransactionResponsesDTO>.Succeed(result, "Purchase Order Successfully!"));
                }

                throw new Exception("Purchase Order Failed! Not Have a Type!");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }



        [Route("create-payment-link-payos")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepositRequestDTO depositRequest)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId;

                if (userId == Guid.Empty)
                {
                    throw new Exception("UserId is invalid or you are not login");
                }
                if (depositRequest.Amount <= 0)
                {
                    throw new Exception("Amount is invalid");
                }
                var result = await _walletService.Deposit(userId, depositRequest.Amount, "PayOS");

                if (result == null)
                {
                    throw new Exception("Create Deposit Transaction Failed!");
                }
                else
                {
                    var url = await _payOSService.CreateLink(depositRequest.Amount);

                    return Ok(ApiResult<string>.Succeed(url, "Payment to deposit!"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        [HttpPost("webhook-payos")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] PayOSObjects.PayOSWebhook payload)
        {
            try
            {
                // Log the incoming webhook payload
                _logger.LogInformation("Received PayOS Webhook Payload: {Payload}", JsonConvert.SerializeObject(payload));

                // Call the service to handle the webhook
                var response = await _payOSService.ReturnWebhook(payload);

                // Check the result of the webhook processing
                if (response.Success)
                {
                    _logger.LogInformation("Webhook processed successfully for OrderCode: {OrderCode}", payload.Data.OrderCode);
                    return Ok(new { status = 200, message = response.Note });
                }
                else
                {
                    _logger.LogWarning("Webhook processing failed: {Message}", response.Note);
                    return BadRequest(new { status = 400, message = response.Note });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while processing the PayOS webhook.");
                return BadRequest(new { status = 400, message = ex.Message });
            }
        }

    }
}