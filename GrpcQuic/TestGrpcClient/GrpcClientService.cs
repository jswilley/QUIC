
using Grpc.Core;
using Grpc.Net.Client;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestGrpcService1;


namespace TestClientGrpc
{
    public class GrpcApiService
    {
        private readonly IHttpClientFactory _clientFactory;
       

        public GrpcApiService(
            IHttpClientFactory clientFactory
           )
        {
            _clientFactory = clientFactory;
            
        }

        public async Task<string> GetGrpcGreetingsAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient("grpc");
                             
                var baseUri = "https://localhost:7042";
                var channel = GrpcChannel.ForAddress(baseUri);
               
                var clientGrpc = new Greeter.GreeterClient(channel);
                
                var response = await clientGrpc.SayHelloAsync(
                 new HelloRequest { Name = "GreeterClient managed" });

                return response.Message;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }

        public async Task<IList<WeatherForecast>> GetGrpcWeatherDataAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient("grpc");
                var baseUri = "https://localhost:7042";
                var channel = GrpcChannel.ForAddress(baseUri);
                var clientGrpc = new WeatherForecasts.WeatherForecastsClient(channel);
                var response = await clientGrpc.GetWeatherAsync(new WeatherForecast());

                return response.Forecasts;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }
    }
}