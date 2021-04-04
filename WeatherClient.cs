using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotNetCore5MicroService
{
    public class WeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceSettings _serviceSettings;
    
        //options gets the values from the startup class (when we registered the ServiceSettings)
        public WeatherClient(HttpClient httpClient, IOptions<ServiceSettings> options)
        {
            _httpClient = httpClient;
            _serviceSettings = options.Value;
        }

        //here we use Record Types which is new in .NETCore 5
        public record Weather(string description);
        public record Main(decimal temp);
        //We create the main record below that contains the Weather array, the Main instance and the datetime
        public record Forecast(Weather[] weather, Main main, long dt);

        //get the actual weather
        public async Task<Forecast> GetCurrentWeatherAsync(string city)
        {
            return await _httpClient.GetFromJsonAsync<Forecast>(
                $"https://{_serviceSettings.OpenWeatherHost}/data/2.5/weather?q={city}&appid={_serviceSettings.ApiKey}&units=metric");

        }
    }
}