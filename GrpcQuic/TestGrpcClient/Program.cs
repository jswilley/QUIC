
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using System.Net;
using TestGrpcService1;


namespace TestClientGrpc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var httpClient = new HttpClient();
            httpClient.DefaultRequestVersion = HttpVersion.Version30;
            httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;

            var channel = GrpcChannel.ForAddress("https://localhost:7042", new GrpcChannelOptions() { HttpClient = httpClient });
            var client = new Greeter.GreeterClient(channel);

            var response = await client.SayHelloAsync(
            new HelloRequest { Name = "World" });

            Console.WriteLine(response.Message);

            var clientGrpc = new WeatherForecasts.WeatherForecastsClient(channel);
            var response2 = await clientGrpc.GetWeatherAsync(new WeatherForecast());

            foreach (var forecast in response2.Forecasts)
            {
                Console.WriteLine($"Date: {forecast.Date}  Tempature: {forecast.TemperatureC}  Summary: {forecast.Summary }");
            }


            var invoker = channel.Intercept(new ClientLoggerInterceptor());

            var clientStream = new Greeter.GreeterClient(invoker);
            await ServerStreamingCallExample(clientStream);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static async Task ServerStreamingCallExample(Greeter.GreeterClient client)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(3.5));

            using var call = client.SayHelloStream(new HelloRequest { Name = "GreeterClient" }, cancellationToken: cts.Token);
            try
            {
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine("Greeting: " + message.Message);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }
    }
}