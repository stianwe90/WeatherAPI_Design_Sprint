using System.Text.Json;
using WeatherAPI.Dtos;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client;

    public WeatherService(IHttpClientFactory clientFactory) // Public constructor for WeatherService
    {
        _client = clientFactory.CreateClient("WeatherApiClient");
    }

    public async Task<WeatherDto?> GetCurrentWeather(string location)
    {
        var response = await _client.GetAsync($"weather/current?location={location}");
        if (!response.IsSuccessStatusCode)
            return null;

        var rawJson = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(rawJson);

        var timeseries = document.RootElement
            .GetProperty("properties")
            .GetProperty("timeseries");

        var forecasts = new List<WeatherDetails>();

        foreach (var entry in timeseries.EnumerateArray().Take(12)) // Hours of data to return. (NB! Need error handling)
        {
            var instantDetails = entry.GetProperty("data")
                                      .GetProperty("instant")
                                      .GetProperty("details");

            var nextHour = entry.GetProperty("data")
                                .GetProperty("next_1_hours");

            forecasts.Add(new WeatherDetails
            {
                Timestamp = entry.GetProperty("time").GetDateTime(),
                Temperature = instantDetails.GetProperty("air_temperature").GetDouble(),
                Humidity = instantDetails.GetProperty("relative_humidity").GetDouble(),
                WindSpeed = instantDetails.GetProperty("wind_speed").GetDouble(),
                Precipitation = nextHour.GetProperty("details").GetProperty("precipitation_amount").GetDouble(),
                SymbolCode = nextHour.GetProperty("summary").GetProperty("symbol_code").GetString()!
            });
        }

        return new WeatherDto
        {
            Location = location , // Adjust extraction as needed
            Timestamp = DateTime.Now, //formatted to hh:mm:ss,
            Forecasts = forecasts
        };
    }
}

public interface IWeatherService
{
    Task<WeatherDto?> GetCurrentWeather(string location);
}
