// using MAVSDK;
//
// namespace kisa_gcs_system.Services;
//
// public class MavlinkDecoder3 : MessageToMessageDecoder<DatagramPacket>
// {
//     private readonly MAVLink.MavlinkParse _parser;
//     private readonly ILogger<MavlinkDecoder3> _logger;
//
//     public MavlinkDecoder3(MAVLink.MavlinkParse parser, ILogger<MavlinkDecoder3> logger)
//     {
//         _parser = parser ?? throw new ArgumentNullException(nameof(parser));
//         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//     }
//
//     public MavlinkDecoder3()
//     {
//         throw new NotImplementedException();
//     }
//
//     // UDP 패킷을 MAVLink 메시지로 디코딩하는 메서드
//     protected override async void Decode(IChannelHandlerContext ctx, DatagramPacket msg, List<object> output)
//     {
//         // 발신자 주소를 채널 컨텍스트의 속성으로 저장
//         ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)msg.Sender);
//         
//         // 자식 클래스의 DecodeAsync 메서드를 호출하여 실제 디코딩 작업 수행
//         var decoded = await DecodeAsync(ctx, msg);
//
//         if (decoded != null)
//         {
//             MAVLink.MAVLinkMessage mavLinkMessage = (MAVLink.MAVLinkMessage)decoded;
//             object data = mavLinkMessage.data;
//             if (data is MAVLink.mavlink_attitude_t innerdata)
//             {
//                 Console.WriteLine(innerdata.roll);
//                 // output.Add(innerdata);
//             }
//         }
//     }
//
//     // DatagramPacket에서 MAVLink 메시지를 추출하는 가성 DecodeAsync 메서드
//     protected virtual async Task<object?> DecodeAsync(IChannelHandlerContext ctx, DatagramPacket msg)
//     {
//         var stream = new ReadOnlyByteBufferStream(msg.Content, false);
//
//         try
//         {
//             // 스트림에서 MAVLink 패킷 읽고 파싱
//             return await Task.Run(() => _parser.ReadPacket(stream));
//         }
//         catch (Exception e)
//         {
//             _logger.LogError($"MAVlink Decoding Error: {e}");
//             return null;
//         }
//     }
// }