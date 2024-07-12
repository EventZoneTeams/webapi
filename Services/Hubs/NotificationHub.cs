using Microsoft.AspNetCore.SignalR;

namespace Services.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string title, string content)
        {
            await Clients.All.SendAsync("ReceiveNotification", title, content);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
