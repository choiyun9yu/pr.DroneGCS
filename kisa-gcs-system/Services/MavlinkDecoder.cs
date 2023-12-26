using kisa_gcs_system.Services;
using MAVSDK;

namespace kisa_gcs_service.Service;

public class MavlinkDecoder : MessageToMessageDecoder<DatagramPacket>           
{                                                                              
  private readonly MAVLink.MavlinkParse _parser = new();                      
  private MavlinkMapper _mapper = new();
  protected override void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) 
  {
    // 채널 컨텍스트에 발신자 주소를 속성으로 저장 ?
    context.Channel.GetAttribute(                                               
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    var decoded = Decode(input);                                  
    if ((decoded != null))                                                      
    {
      MAVLink.MAVLinkMessage mavlinkMessage = decoded;
      object data = mavlinkMessage.data;
      _mapper.predictionMapping(data);
      _mapper.gcsMapping(data);
      string predictionJson = _mapper.predictionToJson();
      string gcsJson = _mapper.gcsToJson();
      output.Add(decoded);                                                    
    }
  }
  
  protected MAVLink.MAVLinkMessage? Decode(DatagramPacket input) 
  {
    var stream = new ReadOnlyByteBufferStream(input.Content, false);
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
}