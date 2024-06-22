using System.Net;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

public class MavlinkEncoder
{
    private IChannelHandlerContext? _context;
    private IPEndPoint? _droneAddress;
    private readonly MAVLink.MavlinkParse _parser = new();
    
    public async Task SendCommandAsync(IChannelHandlerContext ctx, IPEndPoint addr, MAVLink.MAVLinkMessage msg)
    {
        _context = ctx;
        _droneAddress = addr;
        
        // _context가 null 이거나 Channel이 활성화되지 않았을 경우 드론 통신을 위한 적절한 환경이 설정되어 있지 않다고 판단하고 메소드 중단
        if (_context is null || !_context.Channel.Active) return;
     
        try 
        {
            // WriteAndFlushAsync 메서드는 비동기적으로 작동하며, 메시지를 채널에 기록하고 즉시 채널을 플러시하여 즉시 전송한다.
            await _context.Channel.WriteAndFlushAsync(EncodeUdpDroneMessage(msg));
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
    
    private DatagramPacket EncodeUdpDroneMessage(MAVLink.MAVLinkMessage msg)
    {
        if (_droneAddress != null)
        {
            // MAVLink 메시지를 MAVLink 2.0 패킷으로 인코딩하여 바이트 배열로 만듬 (Netty 라이브러리의 Unpooled.WrappedBuffer를 사용하여 바이트 배열을 Netty의 버퍼로 래핑)
            var encodeMsg = Unpooled.WrappedBuffer(_parser.GenerateMAVLinkPacket20(
                (MAVLink.MAVLINK_MSG_ID)
                msg.msgid,
                msg.data,
                sign: false,
                msg.sysid,
                msg.compid,
                msg.seq)
            );

            // 래핑된 버퍼와 드론 주소를 사용하여 새로운 DatagramPacket을 생성하고 반환, DatagramPacket은 네트워크 패킷을 나타내는 Netty 라이브러리의 클래스
            return new DatagramPacket(encodeMsg, _droneAddress);
        }
        throw new InvalidOperationException("_droneAddress is null.");
    }
}

// public class MavlinkEncoder : MessageToByteEncoder<MAVLink.MAVLinkMessage>
// {
//     private readonly MAVLink.MavlinkParse _parser = new();
//
//     // 해당 채널이 활성화 되어 있으면 데이터가 입려될 때마다 Encode 메서드가 자동으로 호출된다(?)
//     protected override void Encode(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg, IByteBuffer output)
//     {
//         try
//         {
//             var encodeMsg = _parser.GenerateMAVLinkPacket20(
//                 (MAVLink.MAVLINK_MSG_ID)
//                 msg.msgid,
//                 msg.data,
//                 sign: false,
//                 msg.sysid,
//                 msg.compid,
//                 msg.seq
//             );
//             output.WriteBytes(encodeMsg);
//             
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e.Message);
//         }
//     }
//
// }