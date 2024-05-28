using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;
using Repositories.Utils;
using Services.BusinessModels.ResponseModels;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/v1/payments")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly VnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = (VnPayService)vnPayService;
        }

        /// <summary>
        /// Create payment to get link
        /// </summary>
        /// <returns>
        ///     URL of payment
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVnPayURL([FromQuery] int id, [FromQuery] int tien)
        {
            var orderInfo = new VnPayService.OrderInfo
            {
                OrderId = id,
                Amount = tien
            };

            var url = _vnPayService.CreateLink(orderInfo);

            return Ok(ApiResult<string>.Succeed(url, "Create payment url successfully"));
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayResponseModel vnpayResponseModel)
        {
            var template = ResourceHelper.ReadJsonResource("/Data/index.html", GetType().Assembly);

            var html = string.Format(template);
            return new ContentResult { Content = html, ContentType = "text/html; charset=utf-8", StatusCode = 200 };
        }
    }
}
