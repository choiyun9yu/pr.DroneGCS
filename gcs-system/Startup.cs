using gcs_system.Services;
using gcs_system.Services.Helper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection.Extensions;

// python sim_vehicle.py -v ArduCopter -I 0 -n 3 --auto-sysid --out=udp:127.0.0.1:14556
// python sim_vehicle.py -L ETRI -v ArduCopter -M --out=udp:127.0.0.1:14556

namespace gcs_system;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // GrpcChannel DI 컨테이너에 등록 
        var grpcClient = GrpcChannel.ForAddress("http://localhost:50051");
        services.AddSingleton(grpcClient);
        
        services.AddControllers();
        services.AddScoped<AnomalyDetectionApiService>();
        services.AddScoped<GcsApiService>();
        services.AddScoped<DashboardApiService>();
        services.AddCors(options => 
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
                    .AllowAnyMethod()   
                    .AllowAnyHeader()   
                    .AllowCredentials();
            });
        });
        services.AddSingleton<MavlinkUdpNetty>();
        services.AddSingleton<ArduCopterManager>();
        services.AddSignalR();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {  
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseRouting();              
        app.UseAuthorization();        
        app.UseCors("CorsPolicy");      
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllers();
            endpoints.MapHub<ArduCopterManager>("/droneHub");
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }
    
    public static TBuilder AddNewtonsoftJsonProtocol<TBuilder>(TBuilder builder, Action<NewtonsoftJsonHubProtocolOptions> configure) where TBuilder : ISignalRBuilder
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHubProtocol, NewtonsoftJsonHubProtocol>());
        builder.Services.Configure(configure);
        return builder;
    }
}