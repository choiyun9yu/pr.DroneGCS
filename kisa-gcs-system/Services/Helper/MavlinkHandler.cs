using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models;
using MAVSDK;

namespace kisa_gcs_system.Services.Helper;

public class MavlinkHandler(ArduCopterService droneService) : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>
{

    private IChannelHandlerContext? _context;
    
    private string? _droneId;
    private IPEndPoint? _droneAddress;
    private Dictionary<string, IPEndPoint> _droneAddressMap;
    private DroneConnectionProtocol _protocol = DroneConnectionProtocol.UDP;    

    private ArduCopterService _droneService = droneService;
    protected override async void ChannelRead0(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
    {
        UpdateDroneAddress(ctx);
        
        // var ep = GetChannelEndpoint(ctx);
        // DroneCommunication link = new (_protocol, ep.Address.MapToIPv4() + ":" + ep.Port);
        // Console.WriteLine($"Protocol: {link.getProtocol()}, Address: {link.getAddress()}, Port: {link.getPort()}");
        
        _droneId = msg.sysid.ToString();
        
        await _droneService.HandleMavlinkMessage(msg, ctx, _droneAddress, _droneId);
    }
    
    private void UpdateDroneAddress(IChannelHandlerContext ctx)
    {
        var senderEp = GetChannelEndpoint(ctx);

        if (_droneAddress == null)  // 조건을 바꿔야할 듯 TO DO
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
        var contextEp = (IPEndPoint)ctx.Channel.RemoteAddress;
        
        return contextEp ?? ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Get()!;
    }
    
    // public override void ChannelActive(IChannelHandlerContext ctx)
    // {
    //     try
    //     {
    //         lock
    //             (this) // 공유 리소스와 관련된 경쟁 조건 및 스레드 안전성을 보장한다. lock 문을 사용하면 여러 스레드가 동시에 _context 필드에 엑세스하고 업데이트하려고 할 때 하나의 스레드만 언제든지 그 작업을 수행할 수 있다. 이렇게 함으로써 여러 스레드가 관련된 경우 발생할 수 있는 경쟁 조건이나 데이터 손상과 관련된 문제를 방지할 수 있다.
    //         {
    //             _context = ctx;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //     }
    // }
    //
    // public override void ChannelInactive(IChannelHandlerContext ctx)
    // {
    //     Console.WriteLine("Channel Inactive triggered.");
    //     try
    //     {
    //         if (_droneAddress != null)
    //         {
    //             // 드론의 상태를 오프라인으로 설정
    //         }
    //         // 드론이 연결 해제 했을 때 로그 남김 
    //         Console.WriteLine($"Drone {_droneId} has disconnected!");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //     }
    // }
}

