using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public WeatherController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] string location)
    {
        if (string.IsNullOrEmpty(location))
            return BadRequest("Location is required");

        var response = await _httpClient.GetAsync($"http://localhost:5001/weather/current?location={location}");

        if (!response.IsSuccessStatusCode)
            return BadRequest("Error fetching weather data");

        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecastWeather([FromQuery] string location, [FromQuery] int days = 5)
    {
        if (string.IsNullOrEmpty(location))
            return BadRequest("Location is required");

        var response = await _httpClient.GetAsync($"http://localhost:5001/weather/forecast?location={location}&days={days}");

        if (!response.IsSuccessStatusCode)
            return BadRequest("Error fetching forecast data");

        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }
}
