using kisa_gcs_service.Service;
using kisa_gcs_service.Model;

namespace kisa_gcs_service;
public static class Program
{
    public static async Task Main(string[] args)	// 어프리케이션 진입점. Main 메소드 정의
    {
        // DroneConfiguration.Initialization();
		
        // 호스트 빌드
        var host = CreateHostBuilder(args).Build();
		
        // MAVLink 수신
        MAVLinkReceiver(host, 14556);
		
        // 호스트 실행
        await host.RunAsync();
    }
	
    public static IHostBuilder CreateHostBuilder(string[] args) => // 어플리케이션 구성 및 실행을 위한 IHostBuilder 정의
        Host.CreateDefaultBuilder(args) // IHostBUilder 인터페이스는 기본 호스팅 설정하고 실행할 수 있음
            .ConfigureWebHostDefaults(webBuilder => // webBuilder.UseStartup<Startup>()을 호출해서 웹 호스팅을 구성하는 메소드  
            {
                webBuilder.UseStartup<Startup>(); // Startup 클래스를 사용해서 웹 애플리케이션을 설정 
                webBuilder.UseUrls("http://0.0.0.0:5000");
            });
	
    public static async Task MAVLinkReceiver(IHost host, int port)
    {
        var MavlinkNettyService =
            (MavlinkNetty)host 
                .Services // ASP.NET Core에서 Host 또는 WebHost를 생성하면 'IServiceProvider 인터페이스를 구현한 컨테이너가 생성된다. 이 서비스 컨테이너는 애플리케이션 전체에서 사용가능한 서비스를 관리하고 제공한다. .Services는 이 서비스 컨테이너에서 서비스를 검색하는데 사용된다. 주로 의존성 주입을 통해 서비스를 사용할 때 쓰인다. 
                .GetService(typeof(MavlinkNetty))!; // 서비스 프로바이더로부터 DroneMonitorServiceMavUdpNetty 서비스를 가져온다.
        await MavlinkNettyService.StartAsync(port); // 가져온 서비스의 StartAsync 메서드를 호출해서 시작(포트 번호 14556을 전달)
    }
}