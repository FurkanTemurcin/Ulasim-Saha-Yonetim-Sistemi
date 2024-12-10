using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Portal.Hubs
{
    public class NotificationHub : Hub
    {
        // Bildirim gönderme işlevi
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
