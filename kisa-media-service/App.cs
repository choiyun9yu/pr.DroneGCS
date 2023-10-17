using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace kisa_gcs_service
{
    public class App
    {
        public static void Main(string[] args)
        {
            // IHostBuilder 생성 및 웹 호스팅 구성
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // 기본 호스트 빌더 생성
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Startup 클래스를 사용하여 웹 애플리케이션 구성
                    webBuilder.UseStartup<Startup>();
                });
    }
}
