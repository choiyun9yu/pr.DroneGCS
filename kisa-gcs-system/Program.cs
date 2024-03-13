using kisa_gcs_system.Services;
using kisa_gcs_system.Services.Helper;

namespace kisa_gcs_system;
public static class Program
{
    public static async Task Main(string[] args)	            
    {
        var host = CreateHostBuilder(args).Build();
        
        await DroneUdpConnection(host, 14556);
        
        await host.RunAsync();

        await DroneUdpDisconnection(host);
    }
	
    private static IHostBuilder CreateHostBuilder(string[] args) => 
        Host.CreateDefaultBuilder(args)                            
            .ConfigureWebHostDefaults(webBuilder =>                
            {
                webBuilder.UseStartup<Startup>();                 
                webBuilder.UseUrls("http://0.0.0.0:5000");
            });
    
    public static async Task DroneUdpConnection(IHost host, int port)
    {
        var mavlinkUdpNettyService = (MavlinkUdpNetty)host.Services.GetService(typeof(MavlinkUdpNetty))!;                
        await mavlinkUdpNettyService.Bind(port); 
    }

    public static async Task DroneUdpDisconnection(IHost host)
    {
        var mavlinkUdpNettyService = (MavlinkUdpNetty)host.Services.GetService(typeof(MavlinkUdpNetty))!;
        await mavlinkUdpNettyService.Close();
    }

}