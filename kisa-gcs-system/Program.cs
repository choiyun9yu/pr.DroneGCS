using kisa_gcs_system.Services;

namespace kisa_gcs_system;
public static class Program
{
    public static async Task Main(string[] args)	            
    {
        var host = CreateHostBuilder(args).Build();
        
        DroneConnection(host, 14556);

        await host.RunAsync();
    }
	
    public static IHostBuilder CreateHostBuilder(string[] args) => 
        Host.CreateDefaultBuilder(args)                            
            .ConfigureWebHostDefaults(webBuilder =>                
            {
                webBuilder.UseStartup<Startup>();                 
                webBuilder.UseUrls("http://0.0.0.0:5000");
            });
	
    public static async Task DroneConnection(IHost host, int port)
    {
        var MavlinkNettyService =
            (MavlinkNetty)host 
                .Services.GetService(typeof(MavlinkNetty))!;                
        await MavlinkNettyService.StartAsync(port); 
    }
}