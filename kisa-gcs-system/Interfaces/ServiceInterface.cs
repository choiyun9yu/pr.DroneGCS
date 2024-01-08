using MAVSDK;

namespace kisa_gcs_system.Interfaces;

// IDroneController 인터페이스
public interface IDroneController
{
    Task HandleMavlinkMessage(MAVLink.MAVLinkMessage msg, DroneCommunication link);
    Task HandleDroneFlightMode(CustomMode flightMode);
}

// IMavlinkHandler 인터페이스
public interface IMavlinkHandler
{
    Task SetCustomModeAsync(MAVLink.MAVLinkMessage msg);
}