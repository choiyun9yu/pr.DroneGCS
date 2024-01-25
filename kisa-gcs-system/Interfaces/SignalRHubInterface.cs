using kisa_gcs_system.Models;

namespace kisa_gcs_system.Interfaces;

public interface IDroneHub
{
    Task UpdateDroneMessage(object message);
    Task HandleDroneFlightMode(CustomMode flightMode);
    Task HandleDroneFlightCommand(DroneFlightCommand flightCommand);

    Task HandleMissionAlt(int missionAlt);
    Task HandleMissionMarking(string startPoint, List<string> transitPoints, string targetPoint);
    
    Task HandleDroneStartingMarking(double lat, double lng);
    Task HandleDroneTargetMarking(double lat, double lng);
    Task HandleDroneMoveToTarget(double lat, double lng);
    Task HandleDroneMoveToBase(double lat, double lng);
    
    Task HandleDroneJoystick(ArrowButton arrow);
    Task HandleControlJoystick(ArrowButton arrow);
    // Task HandleCameraJoystick(ArrowButton arrow);
    // Task HandleCameraCommand();
}
