using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPI.Dtos;
using WeatherAPI.Services;
using Xunit;

namespace WeatherApi.Tests.Services;

public class WeatherServiceTests
{
    [Fact]
    public async Task GetCurrentWeather_ReturnsWeatherDto_WhenApiReturnsSuccess()
    {
        
    }

    [Fact]
    public async Task GetCurrentWeather_ReturnsNull_WhenApiReturnsFailure()
    {
        
    }
}
