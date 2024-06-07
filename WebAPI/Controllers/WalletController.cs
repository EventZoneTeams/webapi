using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Services.DTO.ResponseModels;
using Services.DTO.WalletDTOs;
using Services.Interface;
using System.Reflection;

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

        [HttpGet("{userId}/wallets")]
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
        /// Create payment to get link
        /// </summary>
        /// <returns>
        ///     URL of payment
        /// </returns>
        [HttpPost("{userId}/wallets/deposit")]
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
                    throw new Exception("Deposit Failed!");
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

        [HttpGet("/payment/vnpay-return")]
        public async Task<IActionResult> PaymentReturn([FromQuery] VnpayResponseModel vnpayResponseModel)
        {
            try
            {
                var htmlString = string.Empty;


                var message = vnpayResponseModel.vnp_ResponseCode switch
                {
                    "00" => "Hóa đơn đã được cập nhật thành công",
                    "01" => "Hóa đơn không tìm thấy",
                    "02" => "Bill đã được thanh toán hoặc đã bị hủy",
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

                //update transaction status
                await _walletService.ConfirmTransaction(int.Parse(vnpayResponseModel.vnp_TxnRef));

                return Content(htmlFormat, "text/html");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString(), "text/html");
            }
        }

    }
}
