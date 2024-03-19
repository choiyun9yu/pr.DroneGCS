using DotNetty.Transport.Channels;
using gcs_system.MAVSDK;

namespace gcs_system.Interfaces;
public interface ISignalRHub;

public interface IDroneControl
{
    Task GetDroneList();
    Task SelectDrone(string selectedDroneId); 
    Task HandleMavlinkMessage(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg);
    Task HandleDroneFlightMode(FlightMode flightMode);
    Task HandleDroneFlightCommand(FlightCommand flightCommand);
    Task GetTest();
}
