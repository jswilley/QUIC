using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestGrpcService1.Service;

namespace TestGrpcService1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddGrpc(options => {
        //        options.EnableDetailedErrors = true;
        //        options.IgnoreUnknownServices = true;
        //        options.MaxReceiveMessageSize = 6291456; // 6 MB
        //        options.MaxSendMessageSize = 6291456; // 6 MB
               
        //                                              //options.ResponseCompressionLevel = CompressionLevel.Optimal; // compression level used if not set on the provider
        //                                              // options.Interceptors.Add<ExceptionInterceptor>(); // Register custom ExceptionInterceptor interceptor
        //    });
        //    services.AddGrpcReflection();
        //    services.AddSingleton<ProtoService>();
        //    services.AddSingleton<WeatherService>();


         
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

       
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGrpcService<WeatherService>();
            //    endpoints.MapGrpcService<GreeterService>();
            //    endpoints.MapGrpcReflectionService();
            //});
        }
    }
}
