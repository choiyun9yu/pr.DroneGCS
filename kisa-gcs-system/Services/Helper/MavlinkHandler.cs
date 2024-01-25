using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models;
using MAVSDK;

namespace kisa_gcs_system.Services.Helper;

public class MavlinkHandler : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>
{
    private IChannelHandlerContext? _context;
    private DroneControlService _controlService;
    private DroneConnectionProtocol _protocol = DroneConnectionProtocol.UDP;
    private IPEndPoint? _droneAddress;

    public MavlinkHandler(DroneControlService controlService)
    {
        _controlService = controlService ?? throw new ArgumentNullException(nameof(controlService));
    }
    
    protected override async void ChannelRead0(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
    {
        UpdateDroneAddress(ctx);
        
        // var ep = GetChannelEndpoint(ctx);
        // DroneCommunication link = new (_protocol, ep.Address.MapToIPv4() + ":" + ep.Port);
        
        await _controlService.HandleMavlinkMessage(msg, ctx, _droneAddress);
    }
    
    private void UpdateDroneAddress(IChannelHandlerContext ctx)
    {
        var senderEp = GetChannelEndpoint(ctx);

        if (_droneAddress == null)
        {
            // 새로운 IPEndPoint를 생성하여 드론의 주소로 설정 
            _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
        }
        else
        {
            // 스레드 안전하게 업데이트 (여러 스레드에서 동시에 업데이트가 발생할 수 있는 상황에서의 경합 조건을 방지)
            lock (_droneAddress)
            {
                _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
            }
        }
    }
    
    // IChannelHandlerContext에서 엔드포인트 정보를 가져오는 메서드, IPEndPoint 클래스는 IP 주소와 포트 번호를 나타낸다. 주로 소켓 프로그래밍에서 사용되며 특히 TCP 또는 UDP 통신에서 호스트와 포트를 식별하는데 유용하다.   
    private IPEndPoint GetChannelEndpoint(IChannelHandlerContext ctx)
    {
        // 현재 채널의 원격 주소를 IPEndPoint로 캐스팅해서 정보 가져오기
        var contextEp = (IPEndPoint)ctx.Channel.RemoteAddress;
        
        // 채널의 속성에서 "SenderAddress" 라는 키로 저장된 IPEndPoint 정보 가져오기
        return contextEp ?? ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Get()!;
    }
    
}

