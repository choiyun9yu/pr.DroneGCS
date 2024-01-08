using kisa_gcs_system.Services;

namespace kisa_gcs_system.Interfaces;

// public class DroneHub : Hub
// {
//     public async Task UpdateDroneMessage(object message)
//     {
//         await Clients.All.SendAsync("droneMessage", message);
//     }
//     
//     public void HandleDroneFlightMode(CustomMode flightMode)
//     {
//         Console.WriteLine($"Acting HandleDroneFlight : {flightMode}");
//     }
// }

public interface IDroneHub
{
    Task UpdateDroneMessage(object message);
    Task HandleDroneFlightMode(CustomMode flightMode);
    Task HandleDroneFlightCommand(DroneFlightCommand flightCommand);
    Task HandleDroneJoystick(ArrowButton arrow, ArrowButtonTarget target=ArrowButtonTarget.DRONE);
    Task HandleBodyJoystick(ArrowButton arrow, ArrowButtonTarget target=ArrowButtonTarget.BODY);
    Task HandleCameraJoystick(ArrowButton arrow, ArrowButtonTarget target=ArrowButtonTarget.CAMERA);
    
    // Task HandleCameraCommand();
}
