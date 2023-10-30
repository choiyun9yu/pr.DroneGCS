#nullable disable   // null 체크 무시 (#을 사용해서 전처리기 지시문을 사용)
    
using System;   // 기본 데이터 형식 및 기능을 제공하는 .Net의 기본 네임스페이스                          
using System.IO;    // 파일 시스템 및 스트림과 관련된 작업을 수행할 때 사용하는 네임스페이스 (입출력 작업을 수행하는 클래스와 메서드를 제공)
using System.Reflection;    // .NET 어셈블리 및 형식 정보에 엑세스하고 조작하는데 사용되는 클래스와 메서드를 제공하는 네임스페이스
using System.Threading.Tasks;   // 비동기 작업을 처리하기 위한 클래스와 인터페이스를 제공하는 네임스페이스 (Task 및 Task<T>와 같은 비동기 프로그래밍을 위한 클래스 제공)
using Microsoft.AspNetCore;     // ASP.NET Core 프레임워크를 사용하는 웹 응용 프로그램을 개발하기 위한 클래스와 라이브러리를 포함하는 네임스페이스 
using Microsoft.AspNetCore.Hosting; // ASP.NET Core 응용 프로그램을 호스팅하고 관리하는데 사용되는 클래스와 인터페이스를 제공하는 네임스페이스 (WebHost 클래스를 포함하고 있어 웹 앱의 호스팅을 담당 가능)

namespace kisa_gcs_service
{

    public class Program
    {
        public static async Task Main(string[] args)    // 반환 형식에 void 대신 Task 쓰는 이유는 비동기 작업을 하기 위함이다.
        {
            Console.WriteLine($"Version: {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");  // 어셈블리 버전 정보 출력
            
            // 드론 설정 초기화, ConfigurationHelper.cs, ./config.json으로 초기화, 없는 경우 빈 객체로 초기화 
            DroneConfiguration.Initialization();  
            
            // 호스트 빌드
            var host = CreateHostBuilder(args).Build(); // CreateHostBuilder 호출해서 호스트 빌드   // var를 사용하면 컴파일러가 변수의 데이터 형식을 자동으로 결정
            
            // 드론 소켓 서비스 
            
            // 조이스틱 서비스
            
            // 드론 UDP 서비스
            
            // 시리얼 드론 모니터 서비스
            
            // 드론 모니터 관리자
            
            // 호스트 실행
            await host.RunAsync();    
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            // 웹 호스팅 설정 및 루트 디렉토리 출력
            Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), "./WebApp/build"));
            var port = DroneConfiguration.Environment.GetValue("PORT", 5000);   // 동적으로 DroneConfiguration 설정
            return Host.CreateDefaultBuilder(args)
                .UseUrls($"http://*:{port}")
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "./WebApp/build"))
                .UseStartup<Startup>();
        }
        
        // HTTP API 사용할 당시의 Builder
        // public static IHostBuilder CreateHostBuilder(string[] args) =>  // CreateHostBuilder 메소드 정의, 웹 애플리케이션을 구성하고 실행하기 위한 'IHostBuilder'를 생성하는 역할. 
        //     Host.CreateDefaultBuilder(args)             // IHostBuilder 인터페이스는 기본 호스팅 설정하고 실행할 수 있음, (로깅, 구성, 서비스 공급자 및 환경설정 포함)
        //         .ConfigureWebHostDefaults(webBuilder => // 웹 호스팅을 구성하는 메소드, webBuilder.UseStartup<Startup>()을 호출해서
        //         {
        //             webBuilder.UseStartup<Startup>();   // Startup 클래스를 사용해서 웹 애플리케이션을 설정
        //         });
    };    
}
