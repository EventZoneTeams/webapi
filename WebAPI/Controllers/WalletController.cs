using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Services.DTO.ResponseModels;
using Services.DTO.WalletDTOs;
using Services.Interface;
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


                var response = await _vnPayService.IPNReceiver(
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

                var message = response switch
                {
                    "00" => "Hóa đơn đã được cập nhật thành công",
                    "01" => "Hóa đơn không tìm thấy",
                    "02" => "Bill đã được thanh toán hoặc đã bị hủy",
                    "03" => "Bill đã bị hủy",
                    "69" => "Mã giao dịch không hợp lệ",
                    "97" => "Chữ kí không hợp lệ",
                    "04" => "Số tiền không đúng",
                    _ => "Fatal Error"
                };
                Console.WriteLine(message);

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
                string htmlFormat = string.Format(htmlString, vnpayResponseModel.vnp_TxnRef, 100000, message, orderInfo);

                return Content(htmlFormat, "text/html");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), "text/html");
            }
        }

    }
}
