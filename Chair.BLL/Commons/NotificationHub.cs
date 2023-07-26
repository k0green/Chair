using Microsoft.AspNetCore.SignalR;

namespace Chair.BLL.Commons
{
    public class NotificationHub : Hub
    {
        public async Task SendReviewNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveReviewNotification", message);
        }

        public async Task SendOrderNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveOrderNotification", message);
        }
    }
}
