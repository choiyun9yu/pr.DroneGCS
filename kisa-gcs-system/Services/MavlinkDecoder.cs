using MAVSDK;

namespace kisa_gcs_service.Service;

public class MavlinkDecoder : MessageToMessageDecoder<DatagramPacket> // MavlinkUdpMessageDecoder 클래스는 MessageToMessageDecoder<DatagramPacket>을 상속받아 UDP 패킷을 MAVLink 메시지로 디코딩 한다.
{                                                                               // MessageToMessageDecoder 클래스는 dotNetty에서 사용자가 정의한 프로토콜로 인코딩된 메시지를 디코딩하는 데 사용, 이 클래스를 상속받아 사용자 정의 디코딩 로직을 구현할 수 있다.
  private readonly MAVLink.MavlinkParse parser = new();    // MAVLink 라이브러리의 MavlinkParse 클래스 생성(MavlinkParse 클래스는 MAVLink 메시지를 파싱하고 생성하는데 사용되는 클래스)
  
  protected override async void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) // Decode 메서드는 기본 클래스 메서드를 재정의하여 DatagramPacket을 MAVLink 메시지로 디코딩하고, 디코딩 된 메시지를 SignalR Hub로 전송
  {
    context.Channel.GetAttribute(                    // 채널 컨텍스트에 발신자 주소를 속성으로 저장
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    var decoded = Decode(context, input);     // 자식 클래스의 Decode 메서드를 호출하여 실제 디코딩 작업을 수행
    if ((decoded != null))      //  디코딩 데이터가 null값이 아니라면 output 리스트에 추가
    {
      MAVLink.MAVLinkMessage mavlinkMessage = (MAVLink.MAVLinkMessage)decoded;
      // Console.WriteLine($"{mavlinkMessage.msgtypename}: {mavlinkMessage.data}");
      object data = mavlinkMessage.data;
      // output.Add(decoded);    // output.Add() 메서드는 디코딩된 메시지를 처리된 메시지 목록에 추가한다. 이 목록은 다음으로 연결된 핸들러로 전달된다. Netty의 핸들러 파이프라인에서 다음 단계의 핸들러는 이 목록에서 메시지를 꺼내어 추가적은 처리를 수행할 수 있다.
      if (data is MAVLink.mavlink_attitude_t innerdata)
      {
        Console.WriteLine(innerdata.roll);
      }
    }
  }
  
  protected virtual object? Decode(IChannelHandlerContext context, DatagramPacket input)  // DatagramPacket에서 MAVLink 메시지를 추출하기 위한 가상 Decode 메서드 (가상 메서드, 상속된 파생 클래스에서 메서드 재정의 가능)
  {
    var stream = new ReadOnlyByteBufferStream(input.Content, false);  // 이 코드는 입력 데이터인 DatagramPacket의 컨텐츠를 ReadOnlyByteBufferStream 형식으로 변환
    try
    {
      return parser.ReadPacket(stream);  // 스트림에서 MAVLink 패킷을 읽고 파싱합니다.
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);     // 디코딩 오류를 처리하고 오류 메시지를 출력합니다.
      return null;
    }
  }
}