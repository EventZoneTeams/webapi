using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Services.DTO.ResponseModels;
using Services.DTO.WalletDTOs;
using Services.Interface;
using Services.Services.VnPayConfig;
using System.Reflection;
using System.Web;

namespace WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;
        private readonly IMapper _mapper;

        public WalletController(IWalletService walletService, IMapper mapper, IVnPayService vnPayService)
        {
            _walletService = walletService;
            _mapper = mapper;
            _vnPayService = vnPayService;
        }
        /// <summary>
        /// Get 2 Wallets By UserId
        /// </summary>
        [HttpGet("users/{userId}/wallets")]
        public async Task<IActionResult> GetListWalletByUserId(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new Exception("UserId is invalid");
                }
                var result = await _walletService.GetListWalletByUserId(userId);
                return Ok((ApiResult<List<WalletResponseDTO>>.Succeed(result, "Get 2 Wallet Of User with Id " + userId + " Successfully!")));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }


        /// <summary>
        /// Get List Transactions By UserId And Type
        /// </summary>
        [HttpGet("users/{userId}/transactions")]
        public async Task<IActionResult> GetTransactions(int userId, [FromQuery] TransactionTypeEnums transactionTypeEnums, [FromQuery] WalletTypeEnums walletTypeEnums)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new Exception("UserId is invalid");
                }
                var result = await _walletService.GetTransactions(userId);
                return Ok(ApiResult<List<TransactionResponsesDTO>>.Succeed(result, "Get Transactions Of User with Id " + userId + " Successfully!"));
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
        [HttpPost("wallets/{userId}/transactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Deposit(int userId, [FromBody] DepositRequestDTO depositRequest)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new Exception("UserId is invalid");
                }
                if (depositRequest.Amount <= 0)
                {
                    throw new Exception("Amount is invalid");
                }
                var result = await _walletService.Deposit(userId, depositRequest.Amount);

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
        public async Task<IActionResult> SubmitOldTransactionToPay(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
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



                htmlString += "vnpayResponseModel.vnp_ResponseCode: " + vnpayResponseModel.vnp_ResponseCode + "<br>";

                //Get root path and read index.html
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "index.html");

                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite))
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
                return Content(ex.ToString(), "text/html");
            }
        }

    }
}
