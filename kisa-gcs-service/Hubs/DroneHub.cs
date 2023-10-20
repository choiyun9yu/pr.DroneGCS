using Microsoft.AspNetCore.SignalR;

namespace kisa_gcs_service.Hubs
{
    public class DroneHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
