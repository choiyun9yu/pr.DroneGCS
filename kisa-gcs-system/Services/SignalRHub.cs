using kisa_gcs_system.Interfaces;
using MAVSDK;

namespace kisa_gcs_system.Services;

public class DroneHub : Hub
{
    private readonly MAVLink.MavlinkParse _parser = new();
    public async Task UpdateDroneMessage(object message)
    {
        await Clients.All.SendAsync("droneMessage", message);
    }
    
    public async Task SendMavlinkCommand(MAVLink.MAVLinkMessage message, string? droneId) { }

    public async Task SendDroneFlightModeChange(MavCopterFlightMode flightMode)
    {
        var commandBody = new MAVLink.mavlink_set_mode_t()
        {
            custom_mode = (uint)flightMode,
            base_mode = 1,
        };
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.SET_MODE, commandBody));
        await SendMavlinkCommand(msg, null);
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
