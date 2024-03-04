global using System;
global using System.Text;
global using System.Threading;
global using System.Net;
global using System.Net.Sockets;
global using Microsoft.AspNetCore.Mvc;
global using System.Threading.Tasks;
global using System.Linq;
global using System.Collections.Generic;
global using DotNetty.Buffers;
global using DotNetty.Codecs;
global using DotNetty.Common.Utilities;
global using DotNetty.Transport.Bootstrapping;
global using DotNetty.Transport.Channels;
global using DotNetty.Transport.Channels.Sockets;
global using Newtonsoft.Json.Linq;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using Newtonsoft.Json.Converters;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.SignalR.Protocol;
global using Microsoft.AspNetCore.Cors;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using MongoDB.Bson;
global using MongoDB.Driver;
global using MongoDB.Bson.Serialization.Attributes;
using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Services;
using kisa_gcs_system.Services.Helper;

// python sim_vehicle.py -v ArduCopter -I 0 -n 3 --auto-sysid --out=udp:127.0.0.1:14556
// python sim_vehicle.py -L ETRI -v ArduCopter -M --out=udp:127.0.0.1:14556

namespace kisa_gcs_system;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddScoped<AnomalyDetectionApiService>();
        services.AddScoped<GcsApiService>();
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
        services.AddSingleton<DroneControlService>();
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
            endpoints.MapHub<DroneControlService>("/droneHub");
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