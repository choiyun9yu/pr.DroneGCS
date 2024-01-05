// using MAVSDK;
//
// namespace kisa_gcs_system.Services;
//
// public class MavlinkEncoder : MessageToByteEncoder<MAVLink.MAVLinkMessage>
// {
//     private readonly MAVLink.MavlinkParse _parse = new ();
//     private readonly IHubContext<DroneHub> _hubContext;
//     
//     public MavlinkEncoder(IHubContext<DroneHub> hubContext)
//     {
//         _hubContext = hubContext;
//     }
//
//     protected override void Encode(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg, IByteBuffer output)
//     {
//         var encoded = _parse.GenerateMAVLinkPacket20(
//             (MAVLink.MAVLINK_MSG_ID)
//                 msg.msgid,
//                 msg.data,
//                 sign: false,
//                 msg.sysid,
//                 msg.compid,
//                 msg.seq
//         );
//         output.WriteBytes(encoded);
//     }
// }
//
// public async Task HandleDroneFlightMode(CustomMode flightMode)
// {
//     Console.WriteLine("Acting HandleDroneFlightMode");
//     // MAVLink 프로토콜에서 사용되는 메시지 및 명령 생성
//     var commandBody = new MAVLink.mavlink_set_mode_t()
//     {
//         // 시뮬레이터는 SYS ID 가 1 이어서? 
//         target_system = (byte)1,
//         custom_mode = (uint)flightMode,
//         base_mode = 1,
//     };
//         
//     // 생성된 명령을 이용하여 MAVLink 메시지 생성 
//     var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
//         MAVLink.MAVLINK_MSG_ID.SET_MODE, commandBody));
//         
//     // 생성된 MAVLink 메시지를 이용하여 드론에 비행 모드 변경 명령을 비동기적으로 전송 
//     await SetCustomModeAsync(msg);
// }
//     
// private async Task SetCustomModeAsync(MAVLink.MAVLinkMessage msg)
// {
//     // 컨텍스트가 null 이거나 채널이 활성화되어 있지 않은 경우에는 메시지를 전송하지 않고 종료
//     if (_context is null || !_context.Channel.Active)
//     {
//         return;
//     }
//
//     try
//     {
//         await _context.Channel.WriteAndFlushAsync(this.EncodeUdpDroneMessage(msg));
//     } catch (Exception e) {
//         Console.WriteLine(e.Message);
//     }
// }
//
// private DatagramPacket EncodeUdpDroneMessage(MAVLink.MAVLinkMessage msg)
// {
//     // MAVLink 메시지를 MAVLink 2.0 패킷으로 인코딩하여 바이트 배열로 만듬 (Netty 라이브러리의 Unpooled.WrappedBuffer를 사용하여 바이트 배열을 Netty의 버퍼로 래핑)
//     var encodeMsg = Unpooled.WrappedBuffer(_parser.GenerateMAVLinkPacket20(
//         (MAVLink.MAVLINK_MSG_ID)msg.msgid,  
//         msg.data,                           
//         sign: false,
//         msg.sysid,
//         msg.compid,
//         msg.seq
//     ));
//     // 래핑된 버퍼와 드론 주소를 사용하여 새로운 DatagramPacket을 생성하고 반환, DatagramPacket은 네트워크 패킷을 나타내는 Netty 라이브러리의 클래스
//     return new DatagramPacket(encodeMsg, _droneAddress);
// }