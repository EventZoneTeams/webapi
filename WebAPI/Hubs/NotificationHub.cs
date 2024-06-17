using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class NotificationHub : Hub
    {
        //private readonly UserManager _userManager;
        //public NotificationUserHub(UserManager userManager)
        //{
        //    _userManager = _userManager;
        //}
        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            // _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }

        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            //_userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);//adding dump code to follow the template of Hub > OnDisconnectedAsync
        }
    }
}
