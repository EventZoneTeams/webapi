using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repositories.Commons;
using Services.DTO.NotificationDTOs;
using Services.Interface;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly INotificationService _notificationService;

        public NotificationController(IHubContext<NotificationHub> notificationHubContext, INotificationService notificationService)
        {
            _notificationHubContext = notificationHubContext;
            _notificationService = notificationService;
        }
        /// <summary>
        /// [FOR TESTING PURPOSE] Send notification to all user
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("send-all")]
        public async Task<IActionResult> SendAll(string title, string content)
        {
            await _notificationHubContext.Clients.All.SendAsync("sendToUser", title, content);
            return Ok("oke");
        }

        /// <summary>
        /// [FOR TESTING PURPOSE] Send notification to specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("send-user/{userId}")]
        public async Task<IActionResult> SendToSpecificUser(int userId, [FromForm] string title, [FromForm] string content)
        {
            //var connections = _userConnectionManager.GetUserConnections(model.userId);
            //if (connections != null && connections.Count > 0)
            //{
            //    foreach (var connectionId in connections)
            //    {
            //        await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", model.articleHeading, model.articleContent);
            //    }
            //}
            return Ok();
        }
        /// <summary>
        ///     [FOR TESTING PURPOSE] Send notification to group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("send-group/{groupName}")]
        public async Task<IActionResult> SendToGroup(string groupName, [FromForm] string title, [FromForm] string content)
        {
            await _notificationHubContext.Clients.Group(groupName).SendAsync("sendToGroup", title, content);
            return Ok();
        }

        /// <summary>
        /// Get list notification
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetListNotification(int userId)
        {
            try
            {
                var result = await _notificationService.GetNotifications(userId);
                return Ok(ApiResult<List<NotificationDTO>>.Succeed(result, "Get List Notification Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Read all notification of user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("read-all/{userId}")]
        public async Task<IActionResult> ReadAllNotification(int userId)
        {
            try
            {
                var result = await _notificationService.ReadAllNotification(userId);
                return Ok(ApiResult<List<NotificationDTO>>.Succeed(result, "Read All Notification Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        ///   Get Unread Notification Quantity
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("unread-quantity/{userId}")]
        public async Task<IActionResult> GetUnreadNotificationQuantity(int userId)
        {
            try
            {
                var result = await _notificationService.GetUnreadNotificationQuantity(userId);
                return Ok(ApiResult<int>.Succeed(result, "Get Unread Notification Quantity Successfully!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}
