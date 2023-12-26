// using MAVSDK;
// using kisa_gcs_service.Controller;
// using kisa_gcs_service.Model;
//
//
// namespace kisa_gcs_service.Service;
//
// // SimpleChannelInboundHandler: 수신한 데이터를 처리한 후 메모리에서 자동으로 해제하는 기능을 제공한다.
// public class MavlinkHandler : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>, IGcsControllerUnit
// {
//     private IChannelHandlerContext? _context;
//     private IPEndPoint? _droneIp;
//     private GcsController gcsController;
//     private readonly MAVLink.MavlinkParse _parser = new();
//     
//     public MavlinkHandler(GcsController gcsController)
//     {
//         this.gcsController = gcsController;
//     }
//
//     // 채널에서 수신한 데이터를 처리하는 역할을 한다. 이 메서드는 필수적으로 오버라이드 되어야 한다. 채널에서 읽은 데이터를 이형식으로 변환하여 받아온다.
//         /* IChannelHandlerContext는 Netty의 이벤트 파이프라인에서 현재 핸들러의 위치 및 채널파이프라인에 대한 상태를 관리하는 역할을 하는 인터페이스이다. 
//          * 여러 이벤트 핸들러가 등록되어 있을 때, 각 핸들러는 IChannelHandlerContext를 통해 현재의 위치를 파악하고 다음 핸들러로 이벤트를 전달할 수 있다. 이를 통해 이벤트 핸들러 간의 연결과 순서를 관리할 수 있따.
//          * - 이벤트 전파: 현재 핸들러에서 다음 핸들러로 이벤트를 전파할 때 사용된다. FireChannelRead와 같은 메서드를 통해 다음 핸들러로 이벤트를 전달할 수 있다.
//          * - 채널 파이프라인 조작: 채널 파이프라인에 새로운 핸들러를 추가하거나, 기존의 핸들러를 제거하고 이동할 수 있는 메서들를 제공한다.
//          * - 채널 및 이벤트 관련 정보 제공: 현재 채널의 상태나 이벤트에 대한 다양한 정보를 제공한다.
//          * - 채널 연결과 관련된 작업 수행: 채널이 활성화되거나 비활성화될 때 특정 작업을 숳애할 수 있도록 한다.
//          * 따라서 IChannelHandlerContext는 채널 파이프라인에서 이벤트 핸들러가 동작하는 컨텍스트를 나타내며, 다양한 기능을 수행하도록 도와준다.
//         */
//     protected override async void ChannelRead0(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
//     {
//         // 드론 주소 업데이트 및 채널 엔드포인트 가져오기
//         updateDroneIp(ctx);
//         var ep = getChannelEndpoint(ctx);
//         
//         // 드론 통신 객체 생성 및 초기화
//         var link = new DroneCommunication(ep.Address.MapToIPv4() + ":" + ep.Port);
//         _context = ctx;
//         
//         // 드론 메시지를 메니저를 통해 처리하고 결과 기다리기
//         await gcsController.HandleMavlinkMessage(msg, this, link);
//     }
//     
//     // 예외 처리
//     public override void ExceptionCaught(IChannelHandlerContext ctx, Exception exception)
//     {
//         Console.WriteLine("Exception: " + exception);
//         ctx.CloseAsync(); // 예외가 발생하면 채널을 닫을 수 있음
//     }
//
//     
//     ////////////////////////////////////// droneIp 관련 메소드 ////////////////////////////////////// 
//     public void SetDroneIp(IPEndPoint droneIp)
//     {
//         this._droneIp = droneIp;
//     }
//     
//     private void updateDroneIp(IChannelHandlerContext ctx)
//     {
//         var currentEp = getChannelEndpoint(ctx);
//
//         if (_droneIp == null)
//         {
//             _droneIp = new IPEndPoint(currentEp.Address.MapToIPv4(), currentEp.Port);
//         }
//         else
//         {
//             // 드론 주소가 이미 설정되어 있다면, 스레드 안전하게 업데이트 (여러 스레드에서 동시에 업데이트가 발생할 수 있는 상황에서는 경합 조건을 방지한다.)
//             lock (_droneIp)
//             {
//                 _droneIp = new IPEndPoint(currentEp.Address.MapToIPv4(), currentEp.Port);
//             }
//         }
//     }
//
//     private IPEndPoint getChannelEndpoint(IChannelHandlerContext ctx)
//     {
//         // 현재 채널의 원격 엔드포인트 정보 가져오기
//         var contextEp = (IPEndPoint)ctx.Channel.RemoteAddress;
//
//         // TCP인 경우
//         if (contextEp != null)
//         {
//             return contextEp;
//         }
//         // UDP인 경우
//         else
//         {
//             return ctx.Channel
//                 .GetAttribute<IPEndPoint>(AttributeKey<IPEndPoint>.ValueOf("SenderAddress"))
//                 .Get()!;
//         }
//
//     }
//
//     //////////////////////////////// IGcsControllerUnit 관련 메소드 //////////////////////////////// 
//     public Task SendMessageAsync(object msg)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DisconnectAsync()
//     {
//         throw new NotImplementedException();
//     }
// }