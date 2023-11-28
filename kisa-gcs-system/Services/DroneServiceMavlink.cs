#nullable enable

using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using SignalRChat.Hubs;


namespace kisa_gcs_service;

public class MavlinkUdpMessageDecoder : MessageToMessageDecoder<DatagramPacket> // MavlinkUdpMessageDecoder 클래스는 MessageToMessageDecoder<DatagramPacket>을 확장
{                                                                               // MessageToMessageDecoder 클래스는 dotNetty에서 사용자가 정의한 프로토콜로 인코딩된 메시지를 디코딩하는 데 사용, 이 클래스를 상속받아 용자 정의 디코딩 로직을 구현할 수 있음
  private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
  private readonly IHubContext<DroneHub> _hubContext;

  public MavlinkUdpMessageDecoder(IHubContext<DroneHub> hubContext)
  {
    _hubContext = hubContext;
  }

  protected override async void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) // Decode 메서드는 기본 클래스 메서드를 재정의하여 DatagramPacket을 MAVLink 메시지로 디코딩
  {
    context.Channel.GetAttribute( // 채널 컨텍스트에 발신자 주소를 설정
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    // DatagramPacket을 MAVLink 메시지로 디코딩
    var decoded = this.Decode(context, input);
    if (decoded != null)
    {
      // object obj = JsonSerializer.Serialize(decoded); // Json 형태로 변환
      // Console.WriteLine(decoded.GetType());
      string? obj = decoded.ToString();
      // Console.WriteLine(obj.GetType());
      
      // SendEventToClients 메서드 호출하여 클라이언트에게 이벤트 전송
      await _hubContext.Clients.All.SendAsync("ReceiveEvent", obj);
      Console.WriteLine(obj);
      output.Add(obj);
    }
  }
  
  
  protected virtual object? Decode(IChannelHandlerContext context, DatagramPacket input) // DatagramPacket에서 MAVLink 메시지를 추출하기 위한 가상 Decode 메서드
  {
    var stream = new ReadOnlyByteBufferStream(input.Content, false);  // DatagramPacket의 내용에서 스트림을 생성합니다.
    try
    {
      return this.parser.ReadPacket(stream);  // 스트림에서 MAVLink 패킷을 읽고 파싱합니다.
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);     // 디코딩 오류를 처리하고 오류 메시지를 출력합니다.
      return null;
    }
  }
}


public class DroneMonitorServiceMavUdpNetty // UDP 서버 구성하는 클래스
{
  private readonly MultithreadEventLoopGroup _bossGroup = new(2);
  private readonly Bootstrap _bootstrap;
  private IChannel? _bootstrapChannel;

  
  public DroneMonitorServiceMavUdpNetty(IHubContext<DroneHub> hubContext)
  {
    // 부트스트랩 초기화 (_언더스코어를 변수 앞에 붙이면 해당 클래스 내에서만 사용된다는 것을 의미)
    _bootstrap = new Bootstrap();   // _bootstrap: UDP 서버를 설정하고 시작하기 위한 부트스트랩 
    _bootstrap            
      .Group(_bossGroup)            // _bossGroup: Netty의 이벤트 루프 그룹으로, 여러 쓰레드에서 이벤트 루프를 공유하여 네트워크 이벤트를 처리 
      .ChannelFactory(() =>
      {
        var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);  // Netty에서 제공하는 Datagram 소켓 채널, AddressFamily.InterNetwork를 사용하여 IPv4 주소 체계를 사용하는 채널 생성
        return channel;
      })
      .Handler(new ActionChannelInitializer<IChannel>(channel =>  // Netty의 ChannelInitializer를 사용하여 소켓 채널의 초기화 담당, ChannelInitializer는 새로운 채널이 생성될 때마다 호출되는 초기화 로직을 정의한다. 주로 이곳에 채널 파이프라인에 필요한 핸들러 및 디코더, 인코더 등을 추가한다. 
        {
          var pipeline = channel.Pipeline;  // Netty에서는 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계를 파이프라인이라고 한다. pipeline 객체는 이러한 파이프라인을 정의하고 구성하는데 사용된다.
          pipeline.AddLast("MavLink udp decoder", new MavlinkUdpMessageDecoder(hubContext));  // 패킷 디코더 및 핸들러 추가(위에서 정의한)
        }
      ));
  }
  
  
  public async Task StartAsync(int port)  // 지정된 호스트 및 포트로 UDP 클라이언트를 시작하고 데이터를 수신
  { // UDP 서버 시작 및 바인딩
    _bootstrapChannel = await _bootstrap.BindAsync(port);   // _bootstrapChannel: 서버가 바인딩된 채널을 저장하는 변수
    Console.WriteLine("Started UDP server for Mavlink: " + port);
  }
  
  
  public async Task StopAsync()   // 클라이언트 중지
  { // UDP 서버 중지
    if (_bootstrapChannel != null)
      await _bootstrapChannel.CloseAsync();
  }
}