using SignalR.Hubs;

namespace kisa_gcs_system.Services;

public class MavlinkHandler : SimpleChannelInboundHandler<object>
{
    
    private readonly IHubContext<DroneHub> _hubContext;
    
    public MavlinkHandler(IHubContext<DroneHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }


    private MavlinkMapper _mapper = new();
    
    protected override async void ChannelRead0(IChannelHandlerContext ctx, object msg)
    {
        _mapper.predictionMapping(msg);
        _mapper.gcsMapping(msg);
        string droneMessage = _mapper.objectToJson();
        // string[] droneMessage = new string[predictionJson, gcsJson];
        await _hubContext.Clients.All.SendAsync("droneMessage", droneMessage);
    }
}