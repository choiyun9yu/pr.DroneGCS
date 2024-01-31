using kisa_gcs_system.Services;
using kisa_gcs_system.Services.Helper;

namespace kisa_gcs_system;
public static class Program
{
    public static async Task Main(string[] args)	            
    {
        var host = CreateHostBuilder(args).Build();

        // To Do : 이렇게 미리 받을 포트를 열어두는 것 말고, 그때 그때 추가해서 사용할 수 있는 방법, DroneId에 따라 구분해서 명령을 전달할 수 있는 방법 
        // DroneUdpConnection(host, 14550);
        // DroneUdpConnection(host, 14553);
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
        var mavlinkNettyService = (MavlinkNetty)host.Services.GetService(typeof(MavlinkNetty))!;                
        await mavlinkNettyService.Bind(port); 
    }
    
}