using MAVSDK;

namespace kisa_gcs_system.Services.Helper;

public class MavlinkDecoder944 : MessageToMessageDecoder<DatagramPacket>           
{                                                                              
  private readonly MAVLink.MavlinkParse _parser = new();

  protected override void Decode(IChannelHandlerContext ctx, DatagramPacket input, List<object?> output) 
  {
    ctx.Channel.GetAttribute(                                               
        AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Set((IPEndPoint)input.Sender);
    
    var decoded = Decode(input);
    output.Add(decoded);
    // output.Add(OutputMessage);
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
  
  public class OutputMessage
  {
    public int Port { get; set; }
    private MAVLink.MAVLinkMessage Message { get; set; }
  
    public OutputMessage(int port, MAVLink.MAVLinkMessage decoded)
    {
      Port = port;
      Message = decoded;
    }
  }
  
}