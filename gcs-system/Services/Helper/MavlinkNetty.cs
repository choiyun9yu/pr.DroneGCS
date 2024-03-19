using System.Net.Sockets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace gcs_system.Services.Helper;

public class MavlinkUdpNetty
{
  private readonly MultithreadEventLoopGroup _bossGroup = new (2); 
  private readonly Bootstrap _bootstrap;    
  private IChannel? _bootstrapChannel; 
  
  public MavlinkUdpNetty(DroneControlService droneControlService)
  {
      
    _bootstrap = new Bootstrap();   
    _bootstrap            
      .Group(_bossGroup)           
      .ChannelFactory(() =>         
      {
        var channel = new SocketDatagramChannel(AddressFamily.InterNetwork); 
        return channel;
      })
      .Handler(new ActionChannelInitializer<IChannel>(channel => 
        {
          var pipeline = channel.Pipeline; 
          pipeline.AddFirst("Mavlink Decoder", new MavlinkUdpDecoder());
          pipeline.AddLast("Mavlink Handler", new MavlinkHandler(droneControlService));
          pipeline.AddLast("Mavlink Encoder", new MavlinkEncoder());
        }
      ));
  }
  
  public async Task Bind(int port)
  {
    _bootstrapChannel = await _bootstrap.BindAsync(port);
    Console.WriteLine("Started UDP server for Mavlink: " + port);
  }
  
  public async Task Close()
  {
    await _bootstrapChannel!.CloseAsync();
  }

}

// public class MavlinkTcpNetty
// {
//   private readonly MultithreadEventLoopGroup _bossGroup = new(2);
//   private readonly Bootstrap _bootstrap;
//   private IChannel? _bootstrapChannel;
//   private int _port;
//
//   public MavlinkTcpNetty(DroneControlService droneControlService)
//   {
//     _bootstrap = new Bootstrap();
//     _bootstrap
//       .Group(_bossGroup)
//       .ChannelFactory(() =>
//       {
//         var channel = new SocketDatagramChannel(AddressFamily.InterNetwork);
//         return channel;
//       })
//       .Handler(new ActionChannelInitializer<IChannel>(channel =>
//       {
//         var pipeline = channel.Pipeline; 
//         pipeline.AddFirst("Mavlink Decoder", new MavlinkTcpDecoder());
//         pipeline.AddLast("Mavlink Handler", new MavlinkHandler(droneControlService));
//       }));
//   }
//     
//   public async Task Bind(int port)
//   {
//     _port = port;
//     _bootstrapChannel = await _bootstrap.BindAsync(_port);
//     Console.WriteLine("Open TCP Channel for Mavlink: " + _port);
//   }
//     
//   public async Task Close()
//   {
//     Console.WriteLine("Close UDP Server for Mavlink " + _port);
//     await _bootstrapChannel.CloseAsync();
//   }
//
// }