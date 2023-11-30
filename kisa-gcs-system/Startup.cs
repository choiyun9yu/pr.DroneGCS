using SignalRChat.Hubs;
using kisa_gcs_service.Service;

namespace kisa_gcs_service;

public class Startup
{
    public Startup(IConfiguration configuration) // Startup 클래스의 생성자, IConfiguration 인터페이스의 객체를 매개변수로 받음
    {
        Configuration = configuration; // IConfiguration 인터페이스는 어플리케이션의 구성 정보를 로드하고 읽는데 사용됨 이렇게 하면 Configuration을 통해 appsettings.json의 설정 값을 읽을 수 있음
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) // ConfigureServices 메소드, 서비스 컨테이너에 서비스를 추가하는 메소드
    {
        services.AddControllers();  // 컨트롤러 서비스 등록, 컨트롤러는 API의 엔드포인트를 구성하는데 사용됨
        // services.AddScoped<>();         
        services.AddCors(options => // CORS 정책 추가
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000")// 특정 오리진 허용
                    .AllowAnyMethod()   // 모든 HTTP 메서드 허용
                    .AllowAnyHeader()   // 모든 헤더 허용
                    .AllowCredentials();// Credentails 모드 제거 (보안 상의 이유로 모든 오리진 허용 옵션과 동시 사용 불가)
            });
        });
        services.AddSingleton<DroneMonitorServiceMavUdpNetty>();  // UDP 
        services.AddSignalR();      // SignalR 추가
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Configure 메소드는 요청 처리 파이프라인 구성하는 메소드
    {   // IApplicationBuilder는 ASP.NET Core 미들웨어 파이프라인을 구성하고 구축하는데 사용, 미들웨어 파이프라인은 HTTP 요청을 처리하고 응답을 생성하는데 사용
        // IWebHostEnvironment는 현재 호스팅 환경 정보를 제공하는 인터페이스, 이를 통해 실행중인 환경을 판별하고 해당 환경에 맞는 동작을 설정
        if (env.IsDevelopment()) // 개발환경인지 확인
        {
            // 개발환경이면 개발자 예외 페이지 사용
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // 개발자 환경이 아니라면 예외처리 및 HSTS(HTTP Strict Transport Security) 사용
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseRouting();               // 라우팅, URL 라우팅 활성화, 요청을 적절한 컨트롤러 액션으로 라우팅하는데 사용
        app.UseAuthorization();         // 인증 및 권한 부여 미들웨어 추가
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints => // 엔드포인트 매핑, 컨트롤러 엔드 포인트를 애플리케이션에 매핑, API 요청을 처리하고 컨트롤러 액션을 실행하는데 사용 
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChatHub>("/chatHub");   // SignalR
            endpoints.MapHub<DroneHub>("/droneHub");    
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }
}