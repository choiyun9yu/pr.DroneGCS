// using Exception = System.Exception;
// using MAVSDK;
// using SignalR.Hubs;
//
// namespace kisa_gcs_service.Service2;
//
// /*
//   UDP 소켓을 통해 데이터가 수신됩니다.
//   데이터가 MavlinkUdpMessageDecoder 클래스의 Decode 메서드로 전달됩니다.
//   발신자 주소가 채널 속성으로 저장됩니다.
//   데이터가 ReadOnlyByteBufferStream 형식으로 변환됩니다.
//   MAVLink.MavlinkParse 클래스의 ReadPacket 메서드를 사용하여 데이터를 파싱합니다.
//   파싱된 메시지가 출력 리스트에 추가됩니다.
//   오류가 발생하면 오류 메시지를 출력하고 null을 반환합니다. 
//  */
//
// //////////////////////////////////////// DroneMavLinkMonitorUnit.cs ////////////////////////////////////////
// public class MavlinkUdpMessageDecoder : MessageToMessageDecoder<DatagramPacket> // MavlinkUdpMessageDecoder 클래스는 MessageToMessageDecoder<DatagramPacket>을 상속받아 UDP 패킷을 MAVLink 메시지로 디코딩 한다.
// {                                                                               // MessageToMessageDecoder 클래스는 dotNetty에서 사용자가 정의한 프로토콜로 인코딩된 메시지를 디코딩하는 데 사용, 이 클래스를 상속받아 사용자 정의 디코딩 로직을 구현할 수 있다.
//   private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();    // MAVLink 라이브러리의 MavlinkParse 클래스 생성(MavlinkParse 클래스는 MAVLink 메시지를 파싱하고 생성하는데 사용되는 클래스)
//   private readonly IHubContext<DroneHub> _hubContext;                           // IHubContext를 주입 받아 SignalR Hub와 통신
//   public MavlinkUdpMessageDecoder(IHubContext<DroneHub> hubContext)
//   {
//     _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));  // ?? 이하는 null인경우 예외처리 코드
//   }
//
//   protected override async void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) // Decode 메서드는 기본 클래스 메서드를 재정의하여 DatagramPacket을 MAVLink 메시지로 디코딩하고, 디코딩 된 메시지를 SignalR Hub로 전송
//   {
//     context.Channel.GetAttribute(                    // 채널 컨텍스트에 발신자 주소를 속성으로 저장
//         AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
//     
//     var decoded = Decode(context, input);     // 자식 클래스의 Decode 메서드를 호출하여 실제 디코딩 작업을 수행
//     if (decoded != null)      //  디코딩 데이터가 null값이 아니라면 output 리스트에 추가
//     {
//       
//       // !중요! //
//       // SendEventToClients 메서드 호출하여 클라이언트에게 이벤트 전송 (이 핸들러가 있어야 Hub에 정의된 메서드를 실행해서 클라이언트로 넘겨준다.)
//       // await _hubContext.Clients.All.SendAsync("DroneStateUpdate", decoded);       
//       
//       output.Add(decoded);    // IHubContext<DroneHub> 인터페이스에 처리된 결과를 전달, 이 메서드는 WebSocket을 통해 클라이언트에게 메시지를 전송하는데 사용된다.
//     }
//   }
//   
//   protected virtual object? Decode(IChannelHandlerContext context, DatagramPacket input)  // DatagramPacket에서 MAVLink 메시지를 추출하기 위한 가상 Decode 메서드 (가상 메서드, 상속된 파생 클래스에서 메서드 재정의 가능)
//   {
//     var stream = new ReadOnlyByteBufferStream(input.Content, false);  // 이 코드는 입력 데이터인 DatagramPacket의 컨텐츠를 ReadOnlyByteBufferStream 형식으로 변환
//     try
//     {
//       return this.parser.ReadPacket(stream);  // 스트림에서 MAVLink 패킷을 읽고 파싱합니다.
//     }
//     catch (Exception e)
//     {
//       Console.Error.WriteLine(e.Message);     // 디코딩 오류를 처리하고 오류 메시지를 출력합니다.
//       return null;
//     }
//   }
// }
//
// //////////////////////////////////////// DroneMonitorServiceMavNetty.cs ////////////////////////////////////////
// public class DroneMonitorServiceMavUdpNetty // UDP 서버 구성하는 클래스
// {
//   private readonly MultithreadEventLoopGroup _bossGroup = new MultithreadEventLoopGroup(2);   // 보스그룹: 네트워크 연결을 수신하는 작업은 I/O 작업이므로 CPU 사용량을 높일 수 있다. 이러한 작업을 효율적으로 처리하기 위해 Netty에서는 이벤트 루프 그룹을 사용한다. 이벤트 루프 그룹은 여러 개의 쓰레드를 사용하여 I/O 작업은 분산 처리한다.
//   private readonly Bootstrap _bootstrap;    // 부트 스트랩: 네트워크 연결을 설정하고 시작하는 데 사용되는 도구이다. 부트 스트랩을 사용하여 채널을 생성하고, 채ㅇ널ㅔ 필요한 프로토콜을 추가하고, 채널을 바인딩할 수 있다. 
//   private IChannel? _bootstrapChannel;      // 부트 스트랩 채널: 네트워크 연결을 수신하고 시작한다. 네트워크 연결을 통해 데이터를 송수신한다. 네트워크 연결에 대한 이벤트 처리를 한다. /이벤트 루프를 사용한다. /부트스트랩에 의해 설정된 프로토콜을 처리한다. /채널파이프라인을 지원한다. 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계이다.
//   
//   // 생성자
//   public DroneMonitorServiceMavUdpNetty(IHubContext<DroneHub> hubContext)   // IHubContext를 주입받아 MavlinkUdpMessageDecoder 클래스에 전달
//   {
//     _bootstrap = new Bootstrap();   // _bootstrap: UDP 서버를 설정하고 시작하기 위한 부트스트랩 생성
//     _bootstrap            
//       .Group(_bossGroup)            // _bossGroup: Netty의 이벤트 루프 그룹으로, 여러 쓰레드에서 이벤트 루프를 공유하여 네트워크 이벤트를 처리 
//       .ChannelFactory(() =>         // new SocketDatagramChannel()을 사용하여 새로운 채널을 생성 (Netty에서 제공하는 Datagram 소켓 채널이다.)
//       {
//         var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);  // Datagram 소켓 채널은 UDP 프로토콜을 사용하는 네트워크 연결을 제공한다.
//         return channel;
//       })
//       .Handler(new ActionChannelInitializer<IChannel>(channel =>  // Netty의 ChannelInitializer를 사용하여 소켓 채널의 초기화 담당, ChannelInitializer는 새로운 채널이 생성될 때마다 호출되는 초기화 로직을 정의한다. 주로 이곳에 채널 파이프라인에 필요한 핸들러 및 디코더, 인코더 등을 추가한다. 
//         {
//           var pipeline = channel.Pipeline;  // Netty에서는 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계를 파이프라인이라고 한다. pipeline 객체는 이러한 파이프라인을 정의하고 구성하는데 사용된다.
//           pipeline.AddLast("MavLink udp decoder", new MavlinkUdpMessageDecoder(hubContext));  // 패킷 디코더 및 핸들러 추가, 여기서 디코더 적용
//         }
//       ));
//   }
//   
//   public async Task StartAsync(int port)  // 지정된 호스트 및 포트로 UDP 클라이언트를 시작하고 데이터를 수신
//   { // UDP 서버 시작 및 바인딩
//     _bootstrapChannel = await _bootstrap.BindAsync(port);  // 입력 받은 port 바인딩,  _bootstrapChannel: 서버가 바인딩된 채널을 저장하는 변수
//     Console.WriteLine("Started UDP server for Mavlink: " + port);
//   }
// }
//
// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// // using Exception = System.Exception;
// // using MAVSDK;
// //
// // namespace kisa_gcs_service.Service;
// //
// // //////////////////////////////////////////// DroneMonitorServiceMavNetty.cs ////////////////////////////////////////////
// // // public class DroneMavNetty
// // // {
// // //   private readonly MultithreadEventLoopGroup _bossGroup = new(1);
// // //   private readonly MultithreadEventLoopGroup _workerGroup = new();
// // //   private readonly ServerBootstrap _bootstrap;
// // //   private IChannel? _bootstrapChannel;
// // //
// // //   public DroneMavNetty()
// // //   {
// // //     _bootstrap = new ServerBootstrap();
// // //     _bootstrap
// // //       .Group(_bossGroup, _workerGroup)
// // //       .Channel<TcpServerSocketChannel>()
// // //       .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
// // //         {
// // //           IChannelPipeline pipeline = channel.Pipeline;
// // //           pipeline.AddLast("MavLink decoder", new DroneMavLinkDecoder());
// // //         }
// // //       ));
// // //   }
// // //   public async Task StartAsync(int port)
// // //   {
// // //     _bootstrapChannel = await _bootstrap.BindAsync(port);
// // //     Console.WriteLine("Started TCP server for Mavlink: " + port);
// // //   }
// // // }
// //
// // // UDP
// // public class DroneMavUdpNetty
// // {
// //   private readonly MultithreadEventLoopGroup _bossGroup = new (2);   
// //   private readonly Bootstrap _bootstrap;    
// //   private IChannel? _bootstrapChannel;     
// //   
// //   // 0. 생성자는 Startup.cs 파일에서 service 컨테이너에 추가될 때 이미 생성
// //   public DroneMavUdpNetty() 
// //   {
// //     _bootstrap = new Bootstrap();   
// //     _bootstrap            
// //       .Group(_bossGroup)           
// //       .ChannelFactory(() =>        
// //       {
// //         var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);  
// //         return channel;
// //       })
// //       .Handler(new ActionChannelInitializer<IChannel>(channel =>  
// //         {
// //           var pipeline = channel.Pipeline; 
// //           pipeline.AddLast("MavLink udp decoder", new MavlinkUdpDecoder());  
// //         }
// //       ));
// //   }
// //   
// //   public async Task StartAsync(int port)  // 1. Program.cs에서 호출되면 
// //   { // _booststrapChannel은 UDP 서버의 채널을 가리키는 변수, _boostrap.BindAsync() 메서드는 이 채널을 특정 포트에 바인딩하는 메서드
// //     _bootstrapChannel = await _bootstrap.BindAsync(port);  // 입력받은 포트로 UDP 서버 시작
// //     Console.WriteLine("Started UDP server for Mavlink: " + port);
// //   }
// // }
// //
// // // public class DroneMavUdpCiNetty
// // // {
// // //   private readonly MultithreadEventLoopGroup _bossGroup = new(2);
// // //   private readonly Bootstrap _bootstrap;
// // //   private readonly DroneMavlinkHandler _handler;
// // //   private IChannel? _bootstrapChannel;
// // //   
// // //   // 생성자
// // // }
// //
// // ////////////////////////////////////////////// DroneMavLinkMonitorUnit.cs //////////////////////////////////////////////
// //
// // // 디코더
// // public class MavlinkUdpDecoder : MessageToMessageDecoder<DatagramPacket> // DatagramPacket 메시지를 디코딩
// // {                                                                               
// //   private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();   
// //   
// //   protected override async void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) 
// //   {
// //     context.Channel.GetAttribute(                   
// //         AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
// //     
// //     var decoded = Decode(context, input);    
// //     if (decoded != null)    
// //     {
// //       output.Add(decoded);    
// //     }
// //   }
// //   
// //   protected virtual object? Decode(IChannelHandlerContext context, DatagramPacket input)  
// //   {
// //     var stream = new ReadOnlyByteBufferStream(input.Content, false); 
// //     try
// //     {
// //       return this.parser.ReadPacket(stream);  
// //     }
// //     catch (Exception e)
// //     {
// //       Console.Error.WriteLine(e.Message);    
// //       return null;
// //     }
// //   }
// // }
// //
// // public class DroneMavLinkDecoder : ByteToMessageDecoder  // IByteBuffer 타입 메시지 디코딩
// // {
// //   private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
// //   protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
// //   {
// //     var decoded = this.Decode(context, input);
// //     if (decoded != null)
// //     {
// //       output.Add(decoded);
// //     }
// //   }
// //
// //   protected virtual object? Decode(IChannelHandlerContext context, IByteBuffer input)
// //   {
// //     var stream = new ReadOnlyByteBufferStream(input, false);
// //     try
// //     {
// //       return this.parser.ReadPacket(stream);
// //     }
// //     catch (Exception e)
// //     {
// //       Console.Error.WriteLine(e.Message);
// //       return null;
// //     }
// //   }
// // }
// //
// // // 인코더
// //
// //
// // // 핸들러
// // public class DroneMavlinkHandler : SimpleChannelInboundHandler<MAVLink.MAVLinkMessage>
// // {
// //   private IChannelHandlerContext? context;  // Netty의 채널 컨텍스트를 저장하는 변수로, 드론과의 통신 채널에 대한 정보를 관리
// //   private IPEndPoint? droneAddress;         // 드론의 IP 주소와 포트 번호를 저장하는 IPEndPoint 객체로 드론과의 통신 주소를 관리
// //   
// //   private DroneMonitorManager manger;
// //   
// //   private readonly MAVLink.MavlinkParse _parser = new();
// //
// //   // 생성자
// //   public DroneMavLinkMessageHandler(DroneMonitorManager manager)
// //   {
// //     this.manager = manager;
// //   }
// //
// //
// //   // MAVLink 메시지를 수신하고 처리하는 역할, MAVLink 메시지가 수신되면 이 메서드가 호출된다.
// //   protected override void ChannelRead0(IChannelHandlerContext context, MAVLink.MAVLinkMessage droneMessage)  // Netty의 'SimpleChannelInboundHandler 클래스에서 상속한 메서드로, 드론으로부터 수신된 MAVLink 메시지 처리
// //   {
// //     await manager.HandleMavlinkMessage(droneMessage, this);
// //   }
// //   
// //   private void updateDroneAddress(IChannelHandlerContext context)
// //   {
// //     var senderEp = getChannelEndpoint(context);
// //     if (droneAddress == null)
// //     {
// //       droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
// //     }
// //     else
// //     {
// //       lock (droneAddress)
// //       {
// //         droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
// //       }
// //     }
// //   }
// //
// //   private IPEndPoint getChannelEndpoint(IChannelHandlerContext context)
// //   {
// //     var contextEp = (IPEndPoint)context.Channel.RemoteAddress;
// //     if (contextEp != null)
// //     {
// //       return contextEp;
// //     }
// //
// //     return context.Channel.GetAttribute(
// //       AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Get()!;
// //   }
// //   
// //   public void SetDroneAddress(IPEndPoint droneAddress)
// //   {
// //     this.droneAddress = droneAddress;
// //   }
// //   
// // }
//
//
//
// //////////////////////////////////////////////// DroneMonitorManager.cs ////////////////////////////////////////////////
//
//
//
//
//
//
// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
