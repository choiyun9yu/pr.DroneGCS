#nullable enable

using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Converters;


using kisa_gcs_service.Hubs;


namespace kisa_gcs_service
{
    public class Startup : IStartup    // 애플리케이션의 초기 설정 및 구성을 담당, C#에서 클래스가 인터페이스를 구현하는 경우 콜론(:)을 통해 어떤 인터페이스를 구현하고 있는지 나타낸다.
    {                                  // IStartup은 Configure 메서드만 요구한다. 
        public void Configure(IApplicationBuilder app) // Configure 메소드는 HTTP 파이프라인 구성하는 역할, IApplicationBuilder는 ASP.NET Core 미들웨어 파이프라인을 구성하고 구축하는데 사용, 미들웨어 파이프라인은 HTTP 요청을 처리하고 응답을 생성하는데 사용
        {                                                                                                                          // 미들웨어(Middleware)는 소프트웨어 응용 프로그램 및 시스템 구성 요소 간의 통신과 상호 작용을 지원하기 위한 중간 소프트웨어 레이어 또는 서비스를 말한다.                                           
            // DefaultFilesOptions를 설정하여 기본 파일 이름을 "index.html"로 설정
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            
            // 파일 확장자에 따른 콘텐츠 유형을 설정
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings =
                {
                    [".m3u8"] = "application/x-mpegURL",
                    [".ts"] = "video/MP2T"
                }
            };
            
            // 디렉토리를 생성하고 해당 디렉토리를 파일 제공자로 사용
            Directory.CreateDirectory(".output");   // .output 디렉토리 생성, 이 디렉토리는 애플리케이션에서 정적 파일을 저장하는 위치로 사용
            app.UseStaticFiles(new StaticFileOptions    // ASP.NET Core 애플리케이션에서 정적 파일을 제공하기 위해 UseStaticFiles 미들웨어를 설정
            {
                ContentTypeProvider = provider,     // ContentTypeProvider는 파일 확장자와 해당 파일의 MIME유형을 매핑하는데 사용 
                ServeUnknownFileTypes = true,       // ServeUnknownFiletypes는 알려지지 않은 MIME 유형의 파일도 서빙하는 것을 허용
                FileProvider = new PhysicalFileProvider(    // FileProvider는 정적 파일이 저장된 디렉토리를 설정, PhysicalFileProvider를 사용하여 .output 디렉토리 내의 파일을 찾을 수 있도록 지정     
                    Path.Combine(Directory.GetCurrentDirectory(), ".output")),  // Path.Combine 메소드를 사용하여 두개의 문자열을 결함, Directory.GetCurrentDirectory()는 현재 실행 중인 애플리케이션의 작업 디렉토리를 가져옴
                RequestPath = "/video"  // 클라이언트에서 정적 파일을 요청할 때 사용되는 URL 경로를 설정, 예를 들어 클라이언트에서 "/video"경로로 파일을 요청하면 .output 디렉토리에서 해당 파일ㅇ르 찾아 응답으로 전
            });
            
            // 웹 애플리케이션을 위한 정적 파일 제공 설정
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "./WebApp/build"))),
                RequestPath = "" 
            });
            
            // CORS 설정
            app.UseCors(builder =>  // app은 IApplicationBuilder이다. UseCors 메서드를 호출하여 CORS 미들 웨어를 구성한다. 이 미들웨어는 HTTP 요청과 응답 사이에 CORS 정책을 적용한다.
            {   // CORS 정책의 원본 도메인 설정, 여러 도메인을 배열로 지정하고 배열 안에 있는 도메인에서의 요청을 허용하도록 설정
                builder.WithOrigins(new String[] { "http://localhost:3000", "http://localhost:3001", "http://localhost:3002",   
                                                   "http://localhost:5000", "http://localhost:5001",
                                                   "http://localhost:5050" })
                    .AllowAnyHeader()       // 모든 HTTP 요청 헤더를 허용하도록 설정
                    .AllowAnyMethod()       // 모든 HTTP 요청 메서드를 허용하도록 설정 
                    .AllowCredentials();    // 자격 증명을 포함한 요청을 허용하도록 설정, 요청에 사용자 인증 정보가 포함되는 경우 허용한다는 것을 의미
            });
            
            // 개발자 페이지, 개발 환경에서 오류 페이지 표시
            app.UseDeveloperExceptionPage();
            
            // 라우팅, URL 라우팅 활성화, 요청을 적절한 컨트롤러 액션으로 라우팅하는 데 사용
            app.UseRouting();                   

            // SignalR hub에 대한 엔드포인트 설정
            app.UseEndpoints(routes => // 엔드포인트 매핑, 엔드포인트는 클라이언트 요청을 처리하고 적절한 핸들러 또는 액션을 호출하는데 사용, 여기서 routes는 엔드포인트 라우팅을 구성하는 데 사용되는 델리게이트 
            {
                routes.MapHub<DroneHub>("/droneHub"); // SignalR 허브를 설정하고 특정경로에 매핑, 경로 /droneHub는 클라이언트 애플리케이션이 SignalR 허브에 연결할 때 사용할 URL 엔드포인
            });
         }
        
        public IServiceProvider ConfigureServices(IServiceCollection services) // IStartup 인터페이스에서 파생된 메서드인 ConfiguireService는 서비스 컨테이너를 설정하고 구성하기 위해 사용, 서비스 컨테이너는 의존성 주입을 지원하며 웹 애플리케이션서에 사용되는 서비스 및 종속성을 등록하고 구성하는데 사용 
        {
            services.AddConnections();
            services.AddCors();
            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
                options.PayloadSerializerSettings.Converters
                    .Add(new StringEnumConverter());
            });
            return services?.BuildServiceProvider();
        }
    }   
}
