using kisa_gcs_system.Models;

namespace SignalR.Hubs;

public class DroneHub : Hub
{
    public async Task droneMessage(object message)
    {
        await Clients.All.SendAsync("droneMessage", message);
    }
}
    
