
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace TestGrpcService1
{
    [AllowAnonymous]
    public class WeatherService : WeatherForecasts.WeatherForecastsBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public override Task<WeatherReply> GetWeather(WeatherForecast request, ServerCallContext context)
        {
            var reply = new WeatherReply();
            var rng = new Random();

            reply.Forecasts.Add((IEnumerable<WeatherForecast>)Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime().AddDays(index)),
                TemperatureC = rng.Next(20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }));

            return Task.FromResult(reply);
        }
    }
}