using kisa_gcs_system.Models;

namespace kisa_gcs_system.Services.Helper;

public class MavlinkUdpNetty
{
  private readonly MultithreadEventLoopGroup _bossGroup = new (2); 
  private readonly Bootstrap _bootstrap;    
  private IChannel? _bootstrapChannel; 
  
  private readonly MavlinkDecoder _decoder;
  private readonly MavlinkHandler _handler;
  private int _port;

  public MavlinkUdpNetty(ArduCopterService controlService)
  {
    _decoder = new MavlinkDecoder();
    _handler = new MavlinkHandler(controlService);
      
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
          pipeline.AddLast("Mavlink Decoder", _decoder);
          pipeline.AddLast("Mavlink Handler", _handler);
        }
      ));
  }
  
  public async Task Bind(int port)
  {
    _port = port;
    _bootstrapChannel = await _bootstrap.BindAsync(port);
    Console.WriteLine("Started UDP server for Mavlink: " + _port);
  }
  
  public async Task Close()
  {
    Console.WriteLine("Close UDP Server for Mavlink " + _port);
    await _bootstrapChannel.CloseAsync();
  }

}

public class MavlinkTcpNetty
{
  private readonly MultithreadEventLoopGroup _bossGroup = new (2); 
  private readonly Bootstrap _bootstrap;    
  private IChannel? _bootstrapChannel; 
  

  public MavlinkTcpNetty(ArduCopterService controlService)
  {
    
  }
  
}
