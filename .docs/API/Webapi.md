# Web API

## 1. webapi project
.NET CLI 로 생성할 수 있는 레거시 프로젝트이다. 아래 명령어로 생성할 수 있다.

    % dotnet new webapi -f net8.0 -o proejct-name 
    % dotnet run    

Program.cs와 project-name.csproj, appsettings.json 등이 생성된다. 

## 2. Program.cs
legacy를 따르지 않고 IHostBuilder 인터페이스 구현체와 Starup.cs 파일을 만들어서 호스트를 생성할 수 있다.

### 2-1. IHostBuilder 인터페이스
IHostBUilder는 기본 호스팅을 설정하고 실행할 수 있게 해주는 인터페이스이다.   

이 인터페이스의 ConfigureWebHostDefaults 메소드를 사용하면 Startup 클래스를 사용해서 웹 애플리케이션을 설정할 수 있다.

    public static IHostBuilder CreateHostBuilder(string[] args) => 
        Host.CreateDefaultBuilder(args)                            
            .ConfigureWebHostDefaults(webBuilder =>                
            {
                webBuilder.UseStartup<Startup>();                 
                webBuilder.UseUrls("http://0.0.0.0:5000");
            });

## 3. Startup.cs
### 3-1. IConfiguration 인터페이스 
Startup 클래스의 생성자는 IConfiguration 인터페이스의 객체를 매개변수로 받는다.  
이 인터페이스는 appsettings.json에 설정 되어 있는 어플리케이션의 구성 정보를 로드하고 읽는데 사용된다.

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

### 3-2. IServiceCollection 인터페이스 
ConfigureServices 메소드를 하나 만들어서 IServiceCollection 인터페이스를 삽입해준다.  
이 인터페이스를 삽입 받은 메서드는 서비스 컨테이너를 등록하고 컨테이너에 서비스를 추가하는 역할을 할 수 있다.

    public void ConfigureServices(IServiceCollection services
    {
        services.AddControllers();  // 컨트롤러 서비스 등록, 컨트롤러는 API의 엔드포인트를 구성하는데 사용됨
        // services.AddScoped<>();         
        services.AddCors(options => // CORS 정책 추가
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000", "http://localhost:3001")// 특정 오리진 허용
                    .AllowAnyMethod()   // 모든 HTTP 메서드 허용
                    .AllowAnyHeader()   // 모든 헤더 허용
                    .AllowCredentials();// Credentails 모드 제거 (보안 상의 이유로 모든 오리진 허용 옵션과 동시 사용 불가)
            });
        });
        // services.AddSingleton<GcsController>(); // GcsController
        services.AddSingleton<MavlinkNetty>();  // UDP 
        services.AddSignalR();      // SignalR 추가
    }

ASP.NET Core에서 Host 또는 WebHost를 생성하면 'IServiceProvider 인터페이스를 구현한 컨테이너가 생성된다.  
이 서비스 컨테이너는 애플리케이션 전체에서 사용가능한 서비스를 관리하고 제공한다.  
.Services는 이 서비스 컨테이너에서 서비스를 검색하는데 사용된다. 주로 의존성 주입을 통해 서비스를 사용할 때 쓰인다.

    public static async Task DroneConnection(IHost host, int port)
    {
        var MavlinkNettyService =
            (MavlinkNetty)host 
                .Services                                          
                .GetService(typeof(MavlinkNetty))!;                // 서비스 프로바이더로부터 DroneMonitorServiceMavUdpNetty 서비스를 가져온다.
        await MavlinkNettyService.StartAsync(port);                // 가져온 서비스의 StartAsync 메서드를 호출해서 시작(포트 번호 14556을 전달)
    }

### 3-3. IApplicationBuilder 인터페이스
Configure라는 메소드를 만들고 IApplicationBuilder 인터페이스를 삽입해준다.  
이 인터페이스를 삽입 받은 메서드는 요청 처리 파이프라인을 구성할 수 있다.  
IApplicationBuilder는 ASP.NET Core 미들웨어 파이프라인을 구성하고 구축하는데 사용된다.  
(미들웨어 파이프라인은 HTTP 요청을 처리하고 응답을 생성하는데 사용한다.) 


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();               // 라우팅, URL 라우팅 활성화, 요청을 적절한 컨트롤러 액션으로 라우팅하는데 사용
        app.UseAuthorization();         // 인증 및 권한 부여 미들웨어 추가
        app.UseCors("CorsPolicy");      // CORS
        app.UseWebSockets();            // 웹 소켓 사용
        app.UseMiddleware<WebSocketHandler>();
        app.UseEndpoints(endpoints =>   // 엔드포인트 매핑, 컨트롤러 엔드 포인트를 애플리케이션에 매핑, API 요청을 처리하고 컨트롤러 액션을 실행하는데 사용 
        {
            endpoints.MapControllers();
            // endpoints.MapHub<ChatHub>("/chatHub");   // SignalR
            endpoints.MapHub<DroneHub>("/droneHub");    // droneHub
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }

### 3-4. IWebHostEnvironment 인터페이스
IWebHostEnvironment는 현재 호스팅 환경 정보를 제공하는 인터페이스이다.   
이를 통해 실행중인 환경을 판별하고 해당 환경에 맞는 동작을 설정할 수 있다.

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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

### 4. Model
[DataContract]와 [DataMember]는 .NET에서 데이터 직렬화를 위해 사용된다.  
- **[DataContract] 어트리뷰트**  
    이 어트리뷰트는 클래스가 데이터 컨트랙트의 일부임을 나타낸다. 데이터 컨트랙트는 직렬화된 데이터 형식을 정의하는데 사용된다.    
    클래스에 이 어트리뷰트를 지정하면 해당 클래스의 인스턴스를 직렬화 및 역직렬화할 때 사용할 데이터 구조를 정의한다.    
- **[DataMember] 어트리뷰트**  
    이 어트리뷰트는 데이터 컨트랙트 내에서 직렬화할 멤버(변수 또는 속성)를 나타낸다.  
    멤버에 이 어트리뷰트를 지정하면 멤버는 직렬화 프로세스에서 고려며되, 이를 통해 해당 멤버의 데이터가 저장 및 전송 된다.  

즉, 이 어트리뷰트들은 클래스의 특정 멤버가 데이터를 표현하고 이를 직렬화할 때 사용되는 규칙을 제공한다.   
이것은 주로 웹 서비스나 데이터베이스와 같은 곳에서 객체의 상태를 전송하거나 저장할 때 유용하다.  

[DataConstract]는 데이터 계약을 나타내는 속성이다. 이 속성을 붙이면 클래스가 WCF(Windows Communication Foundation)에서 사용 가능한 데이터 형식으로 표시된다. 클래스의 속성과 메서드가 WCF에서 자동으로 시리얼화 및 역직렬화 된다.

WCF: Microsoft에서 개발한 분산 통신 프레임워크이다. WCF는 다양한 프로토콜과 형식을 지원하며, 이를 통해 다양한 환경에서 분산 애플리케이션을 개발할 수 있다.