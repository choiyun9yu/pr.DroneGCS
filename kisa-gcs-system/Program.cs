using kisa_gcs_system.Services;

namespace kisa_gcs_system;
public static class Program
{
    public static async Task Main(string[] args)	            
    {
        var host = CreateHostBuilder(args).Build();

        DroneUdpConnection(host, 14556);
        
        await host.RunAsync();
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
        var mavlinkNettyService =
            (MavlinkNetty)host 
                .Services.GetService(typeof(MavlinkNetty))!;                
        await mavlinkNettyService.Bind(port); 
    }
}