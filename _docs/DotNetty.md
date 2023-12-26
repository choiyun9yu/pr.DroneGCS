# DotNetty

## 1. What is the DotNetty?
DotNetty는 .NET 기반의 비동기 네트워크 라이브러리로, 고성능 네트워크 애플리케이션을 개발하는 데 사용되는 오픈 소스 프로젝트이다. 이 라이브러리는 C# 및 .NET 플랫폼에서 TCP/UDP 프로토콜을 처리하고 다른 네트워크 프로그래밍 작업을 수행하기 위한 기능을 제공한다.  
(DotNetty는 Netty라고 불리는 자바에서 유명한 네트워크 프레임워크의 .NET 버전이라고 할 수 있다.)

## 2. What is benefit using the DotNetty?
비동기 및 이벤트 기반: DotNetty는 비동기 및 이벤트 기반의 프로그래밍 모델을 채택하여 다중 클라이언트와 서버 연결을 효율적으로 처리할 수 있다.
고성능: DotNetty는 높은 처리량과 낮은 대기 시간을 제공하는 고성능 네트워크 라이브러리로 알려져 있다.
다양한 프로토콜 지원: TCP 및 UDP와 같은 다양한 네트워크 프로토콜을 처리할 수 있어 다양한 종류의 애플리케이션을 지원한다.
유연성: DotNetty는 커스터마이징 및 확장이 가능하며, 다양한 네트워크 요구 사항에 맞게 조정할 수 있다.
오픈 소스: DotNetty는 오픈 소스 프로젝트로 개발 및 사용이 무료이며, 커뮤니티의 지원을 받고 있다.

## 3. What is disadvantage about DotNetty?

## 4. BootStrap
DotNetty에서 Bootstrap은 네트워크 어플리케이션을 설정하고 시작하는 데 사용되는 클래스이다.  
Bootstrap 클래스는 주로 클라이언트 및 서버 측의 부트스트래핑을 담당한다.  
(부트 스트랩핑은 네트워크 응용 프로그램을 초기화하고 시작하는 프로세스를 말한다.)


### 4-1. 네트워크 프로그램
일반적으로 네트워크 프로그램은 아래 단계로 이루어진다. 
- **부트스트랩 객체 생성**  
    부트스트랩은 네트워크 어플리케이션의 시작점이며, 이를 통해 초기 구성을 수행할 수 있다.  
    dotNetty에서는 ServerBootstrap 또는 Bootstrap 클래스를 사용해 부트스트랩을 생성할 수 있다. 
- **채널 초기화**
    channelInitializer 또는 handler를 설정해서 채널 초기화럴 정의한다.  
    이 단계에서는 채널 파이프라인에 핸들러를 추가하거나 설정하여 요청을 처리하고 응답을 생성하는데 필요한 모든 구성을 수행한다.
- **옵션 및 속성 설정**
    네트워크 옵션 및 속성을 설정하여 부트스트랩이 어떻게 동작할지를 정의한다.  
    이 단계에서는 서버 소켓의 옵션, 클라이언트 소켓의 옵션 등을 설정할 수 있다.
- **바인딩 또는 연결**  
    서버 부트스트랩인 경우, 서버 소켓을 특정 주소와 포트에 바인딩하여 클라이언트의 연결을 수락한다.  
    클라이언트 부스트스트랩인 경우 특정 서버에 연결을 시도한다.  
- **이벤트 루프 시작**
    이제 네트워크 어플리케이션이 작동하기 시작하고, 이벤트 루프가 시작된다.  
    이벤트 루프는 네트워크 이벤트를 수신하고 핸들링하며, 채널 파이프라인의 핸들러들이 호출되어 작업을 수행한다. 

### 4-2 용어 설명 
- **Channel**  
    채널은 네트워크 통신의 입출력이나 이벤트를 다루는 객체이다.  
    네트워크에서 데이터를 읽고 쓰는데 사용되며, 채널 파이프라인에 속한 한들러에 의해 처리된다.  
    채널 파이프라인으로 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계를 설정한다.
- **Handler**  
    핸들러는 네트워크 이벤트를 처리하고, 데이터를 변환하거나 가공하는 역할을 수행한다.  
    채널 파이프라인에 여러 핸들러를 추가하여 데이터를 처리하는 흐름을 정의할 수 있다.  
    인바운드 핸들러는 데이터를 읽고 처리하고, 아웃바운드 핸들러는 데이터를 쓰기 전에 가공한다.
- **Group**  
    그룹은 채널의 집합을 나타낸다. 서버에서 여러 클라이언트 연결을 관리하거나, 클라이언트에서 여러 서버에 연결할 때 사용된다.  
    채널 그룹은 채널이 생성되거나 닫힐 때 자동으로 관리된다.
  - 보스 그룹  
      네트워크 연결을 수신하는 작업은 I/O 작업이므로 CPU 사용량을 높일 수 있다.  
      이러한 작업을 효율 적으로 처리하기 위해 Netty에서는 이벤트 루프 그룹을 사용한다.  
      이벤트 루프 그룹은 여러 개의 쓰레드를 사용하여 I/O 작업을 분산 처리한다.
- **이벤트 루프**  
    이벤트 루프는 이벤트 기반의 프로그래밍에서 비동기적으로 발생하는 이벤트들을 관리하고 처리하는 메커니즘을 말한다.  
    이벤트 루프는 이벤트 큐를 주시하면서 큐에 새로운 이벤트가 들어오면 이를 처리한다.  
    이는 주로 이벤트 핸들러나 콜백 함수를 실행함으로써 이뤄진다.
  - 이벤트 큐
      비동기적으로 발생하는 이벤트들이 큐에 들어가게 된다.  
      이벤트는 주로 사용자 입력, 네트워크 요청, 타이머 등 다양한 비동기적인 작업을 나타낸다.

### 4-3. SimpleChannelInboundHandler
SimpleChannelInboundHandler는 Netty에서 제공하는 ChannelInboundHandlerAdapter 클래스를 상속받은 구현체 중 하나이다.    
이 핸들러는 주로 인바운드 데이터를 처리하는 데 사용되며, 특히 수신한 데이터를 처리하고 그에 따른 작업을 수행하는데 특화되어 있다.
- 자동 메모리 관리: 데이터를 처리한 후 자동으로 메모리를 해제할 수 있다 (메모리 누수 방지)
- 메시지 수신 시 호출되는 메서드: channelRead0이라는 메서드를 오버라이드하여 수신한 메시지를 처리하고 다음 이벤트 핸들러로 전달한다.
- 자동으로 데이터 변환: 수신된 데이터를 핸들러에 전달하기 전에 자동으로 변환해준다. 변환 작업은 channelRead0에서 처리된다.
- Exception 핸들링: 예외가 발생했을 때 exceptionCaught 메서드를 통해 예외를 처리할 수 있다.

<br>
<br>

## 5. How to use?

### 5-1. 부트 스트랩 생성 및 설정

    public MavlinkNetty() 
    {
            _bootstrap = new Bootstrap();   // UDP 서버를 설정하고 시작하기 위한 부트스트랩 생성
            _bootstrap            
              .Group(_bossGroup)            // 이벤트 루프 그룹을 _bossGroup으로 설정, 여러 쓰레드에서 이벤트 루프를 공유하여 네트워크 이벤트를 처리하기 위함
              .ChannelFactory(() =>         // new SocketDatagramChannel()을 사용하여 새로운 채널을 생성 (Netty에서 제공하는 Datagram 소켓 채널)
              {                                                           
                var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);  // Datagram 소켓 채널은 UDP 프로토콜을 사용하는 네트워크 연결을 제공한다.
                return channel;
              })
              .Handler(new ActionChannelInitializer<IChannel>(channel =>              // Netty의 ChannelInitializer를 사용하여 소켓 채널의 초기화, ChannelInitializer는 새로운 채널이 생성될 때마다 호출되는 초기화 로직을 정의한다. 주로 이곳에 채널 파이프라인에 필요한 핸들러 및 디코더, 인코더 등을 추가한다. 
                {
                  var pipeline = channel.Pipeline;                                    // Netty에서는 데이터가 채널을 통과하는 동안 적용되는 일련의 처리 단계를 파이프라인이라고 한다. pipeline 객체는 이러한 파이프라인을 정의하고 구성하는데 사용된다.
                  pipeline.AddLast("Mavlink Decoder", _decoder);
                }
              ));
    }

### 5-2. 부트스트랩과 특정 포트 바인딩 
BindAsync() 메소드를 사용해서 바인딩 

    public async Task StartAsync(int port) 
    {
        _bootstrapChannel = await _bootstrap.BindAsync(port); 
        Console.WriteLine("Started UDP server for Mavlink: " + port);
    }

### 5-3. Datagram MessageToMessageDecoder<DatagramPacket> 클래스
MessageToMessageDecoder 클래스는 dotNetty에서 사용자가 정의한 프로토콜로 인코딩된 메시지를 디코딩하는 데 사용한다. 이 클래스를 상속받으면 사용자 정의 디코딩 로직을 구현할 수 있다.  

이 클래스의 상속자는 아래 메소드를 필수적으로 구현해야한다.

    protected override void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output)
    {
        // output.Add() 메서드는 디코딩된 메시지를 처리된 메시지 목록에 추가한다. 이 목록은 다음으로 연결된 핸들러로 전달된다. Netty의 핸들러 파이프라인에서 다음 단계의 핸들러는 이 목록에서 메시지를 꺼내어 추가적은 처리를 수행할 수 있다.
    }

     /*
      * 가상 메서드
      *  가상 메서드는 부모 클래스에서 정의되어 있지만, 하위 클래스에서 필요에 따라 재정의(override)될 수 있는 메서드이다.
      *  C#에서 'virtual' 키워드를 사용하여 재정의될 수 있다. 재정의된 메서드는 부모 클래스의 메서드 대신 호출되며, 이를 통해 다형성을 구현할 수 있다.
      *  여기서 DecodeAsync가 가성 메소드로 선언했던 이유는 이 클래스를 생속받는 다른 클래스에서 필요에 따라 이 메서드를 재정의하기 위함이다.
      *  지금은 딱히 필요해보이지 않아서 virtual 키워드를 제거했다.
      */

    protected virtual MAVLink.MAVLinkMessage? Decode(DatagramPacket input) 
    {
        var stream = new ReadOnlyByteBufferStream(input.Content, false);        // 이 코드는 입력 데이터인 input를 ReadOnlyByteBufferStream 형식으로 변환
        try
        {
            return _parser.ReadPacket(stream);                                                      
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);                                                     
            return null;
        }
    }





