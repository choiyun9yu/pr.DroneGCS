using MAVSDK;
using kisa_gcs_system.Models;

namespace kisa_gcs_system.Interfaces;

public interface IDroneHub
{
    Task UpdateDroneMessage(object message);
    Task HandleDroneFlightMode(CustomMode flightMode);
    Task HandleDroneFlightCommand(DroneFlightCommand flightCommand);
    Task HandleMissionAlt(int missionAlt);
    Task HandleDroneStartMarking(double lat, double lng);
    Task HandleDroneTargetMarking(double lat, double lng);
    Task HandleDroneTransitMarking(object transitList);
    Task HandleDroneMoveToTarget(double lat, double lng);
    Task HandleDroneMoveToBase(double lat, double lng);
    Task HandleDroneMoveToMission(string startPoint, string targetPoint, List<string> transitPoint, int alt, string totalDistance);
    Task HandleDroneJoystick(ArrowButton arrow);
    Task HandleControlJoystick(ArrowButton arrow);
    Task HandleCameraJoystick(ArrowButton arrow);
    Task HandleCameraCommand();
}
