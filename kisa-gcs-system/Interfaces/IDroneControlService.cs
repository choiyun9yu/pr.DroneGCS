using MAVSDK;
using kisa_gcs_system.Models;

namespace kisa_gcs_system.Interfaces;

public interface SignalRHub
{
}

public abstract class IDroneControlService : Hub<SignalRHub>
{
    public abstract Task GetDroneList();
    public abstract Task SelectDrone(string selectedDroneId);
    public abstract Task HandleMavlinkMessage(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg);
    public abstract Task HandleDroneFlightMode(CustomMode flightMode);
    public abstract Task HandleDroneFlightCommand(DroneFlightCommand flightCommand);
    // public abstract Task HandleMissionAlt();
    // public abstract Task HandleDroneStartMarking(double lat, double lng);
    // public abstract Task HandleDroneTargetMarking(double lat, double lng);
    // public abstract Task HandleDroneTransitMarking(object transitList);
    // public abstract Task HandleDroneMoveToTarget(double lat, double lng);
    // public abstract Task HandleDroneMoveToBase(double lat, double lng);
    // public abstract Task HandleDroneMoveToMission(string startPoint, string targetPoint, List<string> transitPoint, int alt, string totalDistance);
    // public abstract Task HandleDroneJoystick(ArrowButton arrow);
    // public abstract Task HandleControlJoystick(ArrowButton arrow);
    // public abstract Task HandleCameraJoystick(ArrowButton arrow);
    // public abstract Task HandleCameraCommand();
}
