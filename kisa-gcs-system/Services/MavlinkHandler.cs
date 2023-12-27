using SignalR.Hubs;

namespace kisa_gcs_system.Services;

public class MavlinkHandler : SimpleChannelInboundHandler<object>
{
    
    private readonly IHubContext<DroneHub> _hubContext;
    
    public MavlinkHandler(IHubContext<DroneHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));  // ?? 이하는 null인경우 예외처리 코드
    }


    private MavlinkMapper _mapper = new();
    
    protected override async void ChannelRead0(IChannelHandlerContext ctx, object msg)
    {
        _mapper.predictionMapping(msg);
        _mapper.gcsMapping(msg);
        string predictionJson = _mapper.predictionToJson();
        string gcsJson = _mapper.gcsToJson();
        await _hubContext.Clients.All.SendAsync("PredictionMessage", predictionJson);       // SendEventToClients 메서드 호출하여 클라이언트에게 이벤트 전송
    }
}