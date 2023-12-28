using MAVSDK;
using kisa_gcs_system.Interfaces;

namespace kisa_gcs_system.Services;

public class MavlinkHandler : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>
{
    private readonly IHubContext<DroneHub> _hubContext;
    private DroneConnectionProtocol _protocol = DroneConnectionProtocol.UDP;
    private IPEndPoint? _droneAddress;
    private IChannelHandlerContext? _context;
    private DroneController _controller;
    
    public MavlinkHandler(IHubContext<DroneHub> hubContext, DroneController controller)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _controller = controller;
    }
    
    protected override async void ChannelRead0(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
    {
        // 드론 주소 업데이트 및 채널 엔드포인트 가져오기
        UpdateDroneAddress(ctx);
        var ep = GetChannelEndpoint(ctx);
        
        // 드론 통신 객체 생성 및 초기화
        DroneCommunication link = new (_protocol, ep.Address.MapToIPv4() + ":" + ep.Port);
        _context = ctx;

        // 드론 메시지 처리
        await _controller.HandleMavlinkMessage(msg, link);
    }

    // 드론 주소를 업데이트(이 주소는 드론과의 통신에 사용)
    private void UpdateDroneAddress(IChannelHandlerContext ctx)
    {
        // 현재 채널의 엔드포이트 정보 가져오기 
        var senderEp = GetChannelEndpoint(ctx);
        
        // 드론 주소가 아직 설정되지 않았다면
        if (_droneAddress == null)
        {
            // 새로운 IPEndPoint를 생성하여 드론의 주소로 설정 
            _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
        }
        // 드론 주소가 설정되어 있다면
        else
        {
            // 스레드 안전하게 업데이트 (여러 스레드에서 동시에 업데이트가 발생할 수 있는 상황에서의 경합 조건을 방지)
            lock (_droneAddress)
            {
                _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
            }
        }
    }
    
    // IChannelHandlerContext에서 엔드포인트 정보를 가져오는 메서드
    private IPEndPoint GetChannelEndpoint(IChannelHandlerContext ctx)
    {
        // 현재 채널의 원격 엔드포인트 정보 가져오기
        var contextEp = (IPEndPoint)ctx.Channel.RemoteAddress;
        
        // 원격 엔드포인트 정보가 null이 아니면 (TCP 연결인 경우) 
        if (contextEp != null)
        {
            return contextEp;
        }
        // 원격 엔드포인트 정보가 null이면 (UDP 연결인 경우
            // 채널의 속성에서 "SenderAddress" 라는 키로 저장된 IPEndPoint 정보 가져오기
        return ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Get()!;
    }
}