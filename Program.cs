//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.WebHost.ConfigureKestrel((context, options) =>
//{
//    options.ListenAnyIP(7192, listenOptions =>
//     {
//         listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
//         listenOptions.UseHttps();
//     });
//});
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



//app.MapGet("/", async httpContext => await httpContext.Response.WriteAsync("Testing QUIC"));

//app.Run();


using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Listen(IPAddress.Any, 7192, listenOptions =>
    {
        // Use HTTP/3
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});

// Add services to the container.

var app = builder.Build();

app.MapGet("/", async httpContext =>
{
    await httpContext.Response.WriteAsync("HTTP3 Test!");
});

app.Run();