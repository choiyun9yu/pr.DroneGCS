namespace kisa_gcs_service
{
    public class Program
    {
        // 애플리케이션 진입점, Main 메소드 정의
        public static void Main(string[] args)
        {
            // CreateHostBuilder 호출해서 애플리케이션 실행
            CreateHostBuilder(args).Build().Run();
        }

        // CreateHostBuilder 메소드 정의, 웹 애플리케이션을 구성하고 실행하기 위한 'IHostBuilder'를 생성하는 역할
        // 이 메소드에서 애플리케이션의 설정을 정의하고 웹 호스팅 환경을 설정
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) // 기본 호스팅 설정을 만드는 역할, (로깅, 구성, 서비스 공급자 및 환경설정 포함)
                .ConfigureWebHostDefaults(webBuilder => // 웹 호스팅을 구성하는 메소드, webBuilder.UseStartup<Startup>()을 호출해서
                {
                    webBuilder.UseStartup<Startup>();   // Startup 클래스를 사용해서 웹 애플리케이션을 설정
                });
    };    
}
