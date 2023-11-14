#nullable enable

using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace kisa_gcs_service;

public class MavlinkUdpMessageDecoder : MessageToMessageDecoder<DatagramPacket> // MavlinkUdpMessageDecoder 클래스는 MessageToMessageDecoder<DatagramPacket>을 확장
{                                                                                                                  // MessageToMessageDecoder 클래스는 dotNetty에서 사용자가 정의한 프로토콜로 인코딩된 메시지를 디코딩하는 데 사용, 이 클래스를 상속받아 용자 정의 디코딩 로직을 구현할 수 있음
  private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
  
  protected override void Decode(IChannelHandlerContext context, DatagramPacket input, List<object> output) // Decode 메서드는 기본 클래스 메서드를 재정의하여 DatagramPacket을 MAVLink 메시지로 디코딩
  {
    context.Channel.GetAttribute( // 채널 컨텍스트에 발신자 주소를 설정
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    // DatagramPacket을 MAVLink 메시지로 디코딩
    var decoded = this.Decode(context, input);
    if (decoded != null)
    {
      output.Add(decoded);
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
      Console.Error.WriteLine(e.Message); // 디코딩 오류를 처리하고 오류 메시지를 출력합니다.
      return null;
    }
  }
}
public class DroneMavLinkMessageDecoder : ByteToMessageDecoder  // DroneMavLinkMessageDecoder 클래스는 ByteToMessageDecoder를 확장
{                                                                                                   // 
  private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
  protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)   // Decode 메서드는 기본 클래스 메서드를 재정의하여 IByteBuffer을 MAVLink 메시지로 디코딩
  {
    var decoded = this.Decode(context, input);   // IByteBuffer을 MAVLink 메시지로 디코딩
    if (decoded != null)
    {
      output.Add(decoded);
    }
  }

  protected virtual object? Decode(IChannelHandlerContext context, IByteBuffer input)  // IByteBuffer에서 MAVLink 메시지를 추출하기 위한 가상 Decode 메서드
  {
    var stream = new ReadOnlyByteBufferStream(input, false);   // IByteBuffer의 내용에서 스트림을 생성
    try
    {
      return this.parser.ReadPacket(stream);  // 스트림에서 MAVLink 패킷을 읽고 파싱
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message); // 디코딩 오류를 처리하고 오류 메시지를 출력
      return null;
    }
  }
}

public class DroneServiceMavlink
{
    private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
    
    public static void MavLinkMessage()
    {
        UdpClient udpClient = new UdpClient(14556); // ArduPilot SITL에서 데이터 받을 포트 지정
        try
        {
            while (true)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref remoteEp);  // data 배열에는 수신한 바이트 데이터가 들어있다.
                // 여기서 MAVLink 메시지를 해석하고 처리하는 로직을 추가하면 된다.
                MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
                Console.WriteLine(data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            udpClient.Close();
        }
    }
}

