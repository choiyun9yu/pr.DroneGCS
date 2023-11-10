using System;
using System.Net;
using System.Net.Sockets;

namespace kisa_gcs_service
{

    public class Program
    {
        public static void Main(string[] args)      // 애플리케이션 진입점, Main 메소드 정의
        {
            CreateHostBuilder(args).Build().Run();  // CreateHostBuilder 호출해서 애플리케이션 실행
            ProcessMavLinkMessage(); // MAVLink 수신

        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>  // CreateHostBuilder 메소드 정의, 웹 애플리케이션을 구성하고 실행하기 위한 'IHostBuilder'를 생성하는 역할. 
            Host.CreateDefaultBuilder(args)             // IHostBuilder 인터페이스는 기본 호스팅 설정하고 실행할 수 있음, (로깅, 구성, 서비스 공급자 및 환경설정 포함)
                .ConfigureWebHostDefaults(webBuilder => // 웹 호스팅을 구성하는 메소드, webBuilder.UseStartup<Startup>()을 호출해서
                {
                    webBuilder.UseStartup<Startup>();   // Startup 클래스를 사용해서 웹 애플리케이션을 설정
                });
        
        static void ProcessMavLinkMessage()
        {
            UdpClient udpClient = new UdpClient(14556); // ArduPilot SITL에서 데이터 받을 포트 지정
            try
            {
                while (true)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpClient.Receive(ref remoteEP);
                    // data 배열에는 수신한 바이트 데이터가 들어있다.
                    // 여기서 MAVLink 메시지를 해석하고 처리하는 로직을 추가하면 된다.
                    // 예를 들면 MAVLink 라이브러리를 사용할 수 있다.
                    // https://github.com/mavlink/c_library_v2
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                udpClient.Close();
            }
        }
    };    
}
