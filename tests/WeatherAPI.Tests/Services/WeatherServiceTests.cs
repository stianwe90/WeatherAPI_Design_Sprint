using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPI.Dtos;
using WeatherAPI.Services;
using Xunit;
using System.Linq;

namespace WeatherApi.Tests.Services;

public class WeatherServiceTests
{
    // Helper method to create a WeatherService with a given HTTP response.
    private WeatherService CreateWeatherService(HttpResponseMessage responseMessage) 
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://example.com/")
        };

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(_ => _.CreateClient("WeatherApiClient"))
                   .Returns(httpClient);

        return new WeatherService(factoryMock.Object);
    }

    [Fact]
    public async Task GetCurrentWeather_ReturnsWeatherDto_WhenApiReturnsSuccess()
    {
        // Arrange
        var location = "London";
        var responseJson = @"
        {
            ""properties"": {
                ""timeseries"": [
                    {
                        ""time"": ""2025-03-13T12:00:00Z"",
                        ""data"": {
                            ""instant"": {
                                ""details"": {
                                    ""air_temperature"": 7.1,
                                    ""relative_humidity"": 63.7,
                                    ""wind_speed"": 3.5
                                }
                            },
                            ""next_1_hours"": {
                                ""summary"": { ""symbol_code"": ""test"" },
                                ""details"": { ""precipitation_amount"": 0.0 }
                            }
                        }
                    }
                ]
            }
        }";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson)
        };

        var weatherService = CreateWeatherService(responseMessage);

        // Act
        var result = await weatherService.GetCurrentWeather(location);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("London", result.Location);
        Assert.Single(result.Forecasts);
        Assert.Equal(7.1, result.Forecasts.First().Temperature);
    }

    [Fact]
    public async Task GetCurrentWeather_ReturnsNull_WhenApiReturnsFailure()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var weatherService = CreateWeatherService(responseMessage);

        // Act
        var result = await weatherService.GetCurrentWeather("London");

        // Assert
        Assert.Null(result);
    }
}
