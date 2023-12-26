using SignalR.Hubs;

namespace kisa_gcs_service.Service;

public class MavlinkNetty
{
  private readonly MultithreadEventLoopGroup _bossGroup = new MultithreadEventLoopGroup(2);   // 보스그룹: 네트워크 연결을 수신하는 작업은 I/O 작업이므로 CPU 사용량을 높일 수 있다. 이러한 작업을 효율적으로 처리하기 위해 Netty에서는 이벤트 루프 그룹을 사용한다. 이벤트 루프 그룹은 여러 개의 쓰레드를 사용하여 I/O 작업은 분산 처리한다.
  private readonly Bootstrap _bootstrap;    // 부트 스트랩: 네트워크 연결을 설정하고 시작하는 데 사용되는 도구이다. 부트 스트랩을 사용하여 채널을 생성하고, 채ㅇ널ㅔ 필요한 프로토콜을 추가하고, 채널을 바인딩할 수 있다. 
  private IChannel? _bootstrapChannel;      // 부트 스트랩 채널: 네트워크 연결을 수신하고 시작한다. 네트워크 연결을 통해 데이터를 송수신한다. 네트워크 연결에 대한 이벤트 처리를 한다. /이벤트 루프를 사용한다. /부트스트랩에 의해 설정된 프로토콜을 처리한다. /채널파이프라인을 지원한다. 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계이다.
  
  private readonly MavlinkDecoder _decoder;
  // private readonly MavlinkHandler _handler;

  // 생성자
  public MavlinkNetty()   // IHubContext를 주입받아 MavlinkUdpMessageDecoder 클래스에 전달
  {
    _decoder = new MavlinkDecoder();
    // _handler = new MavlinkHandler(gcsController);
      
    _bootstrap = new Bootstrap();   // _bootstrap: UDP 서버를 설정하고 시작하기 위한 부트스트랩 생성
    _bootstrap            
      .Group(_bossGroup)            // _bossGroup: Netty의 이벤트 루프 그룹으로, 여러 쓰레드에서 이벤트 루프를 공유하여 네트워크 이벤트를 처리 
      .ChannelFactory(() =>         // new SocketDatagramChannel()을 사용하여 새로운 채널을 생성 (Netty에서 제공하는 Datagram 소켓 채널이다.)
      {
        var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);  // Datagram 소켓 채널은 UDP 프로토콜을 사용하는 네트워크 연결을 제공한다.
        return channel;
      })
      .Handler(new ActionChannelInitializer<IChannel>(channel =>  // Netty의 ChannelInitializer를 사용하여 소켓 채널의 초기화 담당, ChannelInitializer는 새로운 채널이 생성될 때마다 호출되는 초기화 로직을 정의한다. 주로 이곳에 채널 파이프라인에 필요한 핸들러 및 디코더, 인코더 등을 추가한다. 
        {
          var pipeline = channel.Pipeline;  // Netty에서는 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계를 파이프라인이라고 한다. pipeline 객체는 이러한 파이프라인을 정의하고 구성하는데 사용된다.
          pipeline.AddLast("Mavlink Decoder", _decoder);  // 1차적으로 디코딩해서 핸들러에게 넘겨준다.
          // pipeline.AddLast("Mavlink Handler", _handler);  // 핸들러가 건네받아서 데이터를 처리한 후 자동으로 메모리에서 해제한다.
        }
      ));
  }
  
  public async Task StartAsync(int port)  // 지정된 호스트 및 포트로 UDP 클라이언트를 시작하고 데이터를 수신
  { // UDP 서버 시작 및 바인딩
    _bootstrapChannel = await _bootstrap.BindAsync(port);  // 입력 받은 port 바인딩,  _bootstrapChannel: 서버가 바인딩된 채널을 저장하는 변수
    Console.WriteLine("Started UDP server for Mavlink: " + port);
  }
}