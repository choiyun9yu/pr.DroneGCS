using kisa_gcs_system.Interfaces;

namespace kisa_gcs_system.Services;

public class DroneHub : Hub
{
    public async Task UpdateDroneMessage(object message)
    {
        await Clients.All.SendAsync("droneMessage", message);
    }
    
    public void HandleDroneFlightMode(CustomMode flightMode)
    {
        Console.WriteLine($"Acting HandleDroneFlight : {flightMode}");
    }
}

// using MAVSDK;
//
// using kisa_gcs_system.Interfaces;
//
// namespace SignalR.Hubs;
//
// public interface IDroneHubClient
// {
//     Task UpdateDroneMessage(object message);
//     Task ChangeFlightMode(string doneId, MavCopterFlightMode flightMode);
// }
