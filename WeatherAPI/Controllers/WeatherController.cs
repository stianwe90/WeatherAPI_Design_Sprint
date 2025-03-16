using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherAPI.Controllers;

[ApiController]
// [Authorize]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }



    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] string location)
    {
        if (string.IsNullOrEmpty(location))
            return BadRequest("Location is required");

        var weather = await _weatherService.GetCurrentWeather(location);
        if (weather == null)
            return NotFound("Warning, weather data not found");

        return Ok(weather);

    }

    // [HttpGet("forecast")]
    // public async Task<IActionResult> GetForecastWeather([FromQuery] string location, [FromQuery] int days = 5)
    // {
    //     if (string.IsNullOrEmpty(location))
    //         return BadRequest("Location is required");

    //     var response = await _httpClient.GetAsync($"http://localhost:5001/weather/forecast?location={location}&days={days}");

    //     if (!response.IsSuccessStatusCode)
    //         return BadRequest("Error fetching forecast data");

    //     var data = await response.Content.ReadAsStringAsync();
    //     return Content(data, "application/json");
    // }

    // 
}
