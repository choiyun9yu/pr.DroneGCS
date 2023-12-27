using SignalR.Hubs;

namespace kisa_gcs_system.Services;

public class MavlinkNetty
{
  private readonly MultithreadEventLoopGroup _bossGroup = new (2); 
  private readonly Bootstrap _bootstrap;    
  private IChannel? _bootstrapChannel; 
  
  private readonly MavlinkDecoder _decoder;
  private readonly MavlinkHandler _handler;
  public MavlinkNetty(IHubContext<DroneHub> hubContext)  
  {
    _decoder = new MavlinkDecoder();
    
    _handler = new MavlinkHandler(hubContext);
      
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
  
  public async Task StartAsync(int port) 
  { 
    _bootstrapChannel = await _bootstrap.BindAsync(port);
    Console.WriteLine("Started UDP server for Mavlink: " + port);
  }
}