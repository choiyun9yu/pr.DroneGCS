using MAVSDK;

namespace kisa_gcs_system.Services;

public class MavlinkDecoder : MessageToMessageDecoder<DatagramPacket>           
{                                                                              
  private readonly MAVLink.MavlinkParse _parser = new();   
  
  protected override void Decode(IChannelHandlerContext context, DatagramPacket input, List<object?> output) 
  {
    context.Channel.GetAttribute(                                               
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    var decoded = Decode(input);           
    output.Add(decoded);    
    if ((decoded != null))                                                      
    {
      MAVLink.MAVLinkMessage mavlinkMessage = decoded;
      object data = mavlinkMessage.data;
      output.Add(data);                                                    
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