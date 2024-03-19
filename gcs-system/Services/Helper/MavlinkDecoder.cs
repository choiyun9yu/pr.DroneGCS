using System.Net;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

public class MavlinkTcpDecoder : ByteToMessageDecoder
{
  private readonly MAVLink.MavlinkParse _parser = new();
    
  protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
  {
    var decoded = Decode(context, input);
    if (decoded != null)
    {
      output.Add(decoded);
    }
  }

  protected virtual object? Decode(IChannelHandlerContext ctx, IByteBuffer input)
  {
    var stream = new ReadOnlyByteBufferStream(input, false);
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

public class MavlinkUdpDecoder : MessageToMessageDecoder<DatagramPacket>
{
  private readonly MAVLink.MavlinkParse _parser = new();

  protected override void Decode(IChannelHandlerContext ctx, DatagramPacket input, List<object?> output)
  {
    ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>
        .ValueOf("SenderAddress"))
      .Set((IPEndPoint)input.Sender);

    var decoded = Decode(input);
    // Console.WriteLine(decoded);
    output.Add(decoded);
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

  // public class OutputMessage
  // {
  //     public int Port { get; set; }
  //     private MAVLink.MAVLinkMessage Message { get; set; }
  //
  //     public OutputMessage(int port, MAVLink.MAVLinkMessage decoded)
  //     {
  //         Port = port;
  //         Message = decoded;
  //     }
  // }
}