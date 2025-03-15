using System.Text.Json;
using WeatherAPI.Dtos;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _client;

        public WeatherService(IHttpClientFactory clientFactory) 
        {
            _client = clientFactory.CreateClient();
        }
        public async Task<WeatherDto?> GetCurrentWeather(string location)
        {
            var response = await _client.GetAsync($"http://localhost:5001/weather/current?location={location}");
            if (!response.IsSuccessStatusCode)
                return null;
            var data = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherDto>(data);
        }
    }

    public interface IWeatherService
    {
        public Task<WeatherDto?> GetCurrentWeather(string location);
        
    }
}