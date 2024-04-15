using DotNetty.Transport.Channels;
using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

public class MavlinkHandler(ArduCopterManager _manager) : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>
{
    private IChannelHandlerContext? _context;
    
    public override void ChannelActive(IChannelHandlerContext ctx)
    {
        try
        {
            /*
             * 공유 리소스와 관련된 경쟁 조건 및 스레드 안전성을 보장한다.
             * lock 문을 사용하면 여러 스레드가 동시에 _context 필드에 엑세스하고 업데이트하려고 할 때 하나의 스레드만 언제든지 그 작업을 수행할 수 있다.
             * 이렇게 함으로써 여러 스레드가 관련된 경우 발생할 수 있는 경쟁 조건이나 데이터 손상과 관련된 문제를 방지할 수 있다.
             */
            lock (this) 
            {
                _context = ctx;
            }
        }
        catch (Exception e) { Console.WriteLine(e); }

    }

    protected override async void ChannelRead0(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
    {
        _context = ctx;
        await _manager.HandleMavlinkMessage(ctx, msg);
    }
    
    public override void ChannelInactive(IChannelHandlerContext ctx)
    {
        Console.WriteLine("Channel Inactive triggered.");
    }
    
}

