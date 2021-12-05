using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestGrpcService1.Service;

namespace TestGrpcService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
          

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((context, options) =>
                {
                    options.Listen(IPAddress.Any, 7042, listenOptions =>
                    {
                        // Use HTTP/3
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                        listenOptions.UseHttps();
                    });
                });


            builder.Services.AddGrpc(options => {
                options.EnableDetailedErrors = true;
                options.IgnoreUnknownServices = true;
                options.MaxReceiveMessageSize = 6291456; // 6 MB
                options.MaxSendMessageSize = 6291456; // 6 MB
                options.Interceptors.Add<ServerLoggerInterceptor>();
                //options.ResponseCompressionLevel = CompressionLevel.Optimal; // compression level used if not set on the provider
                // options.Interceptors.Add<ExceptionInterceptor>(); // Register custom ExceptionInterceptor interceptor
            });
            builder.Services.AddGrpcReflection();
            builder.Services.AddSingleton<ProtoService>();
            builder.Services.AddSingleton<WeatherService>();
            builder.Services.AddSingleton<GreeterService>();

            var app = builder.Build();

            app.MapGrpcReflectionService();
            app.MapGrpcService<WeatherService>();
            app.MapGrpcService<GreeterService>();

            app.MapGet("/protos", (ProtoService protoService) =>
            {
                var result =  Results.Ok(protoService.GetAll());
                return result;
            });


            app.MapGet("/protos/{version}/{protoName}", (ProtoService protoService, int version, string protoName) =>
            {
                var filePath = protoService.Get(version, protoName);

                if (filePath != null)
                    return Results.File(filePath);

                return Results.NotFound();
            });
            app.MapGet("/protos/{protoName}/view", async (ProtoService protoService, int version, string protoName) =>
            {
                var text = await protoService.ViewAsync(version, protoName);

                if (!string.IsNullOrEmpty(text))
                    return Results.Text(text);

                return Results.NotFound();
            });

            // Custom response handling over ASP.NET Core middleware
            //app.Use(async (context, next) =>
            //{
            //    if (!context.Request.Path.Value.Contains("protos/v"))
            //    {
            //        context.Response.ContentType = "application/grpc";
            //        context.Response.Headers.Add("grpc-status", ((int)StatusCode.NotFound).ToString());
            //    }
            //    await next();
            //});

            // Run the app
            app.Run();
        }

        
    }
}
