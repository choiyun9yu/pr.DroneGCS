using gcs_system.Services.Helper;

namespace gcs_system;
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
                webBuilder.UseUrls("http://localhost:5000");
            });

    private static async Task DroneUdpConnection(IHost host, int port)
    {
        var mavlinkUdpNettyService = (MavlinkUdpNetty)host.Services.GetService(typeof(MavlinkUdpNetty))!;                
        await mavlinkUdpNettyService.Bind(port); 
    }

    private static async Task DroneUdpDisconnection(IHost host)
    {
        var mavlinkUdpNettyService = (MavlinkUdpNetty)host.Services.GetService(typeof(MavlinkUdpNetty))!;
        await mavlinkUdpNettyService.Close();
    }

}

/* 
 * C# 언어 지침 
 * - 클래스명, 인스턴스명은 명사로 작성되어야 한다.     PascalCase
 * - 메소드명은 동사 또는 동사구로 작성되어야 한다.     PascalCase
 *   (단, 단순히 boolean 상태를 반환하는 메서드의 동사는 Is, Can, Has, Should 를 사용하되
 *                        부자연스럽다면 상태를 나타내는 3인칭 단수 동사를 사용할 수도 있다.)
 * - 필드명은 설명적이고 명확한 명사로 작성한다.
 *   - 공용 필드(public)는 파스칼 케이스를 사용한다.  PascalCase
 *   - 비공개 필드(private)는 카멜 케이스를 사용한다. carmelCase 
 *   - 비공개 프로퍼티인 경우 앞에 _ 를 붙여준다.
 *     ( 프로퍼티: private int score { get; set; } = 0 // 자동 구현 프로퍼티 + 기본값 설정 )
 */
 