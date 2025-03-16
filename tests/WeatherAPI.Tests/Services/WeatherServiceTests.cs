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
        // Arrange
        var jsonResponse = "{ \"type\": \"Feature\", \"geometry\": { \"type\": \"Point\" } }";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.ToString().Contains("location=Oslo")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://testapi/")
        };

        var clientFactoryMock = new Mock<IHttpClientFactory>();
        clientFactoryMock.Setup(cf => cf.CreateClient("WeatherApiClient"))
                         .Returns(httpClient);

        var weatherService = new WeatherService(clientFactoryMock.Object);

        // Act
        var result = await weatherService.GetCurrentWeather("Oslo");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Feature", result!.type);
        Assert.Equal("Point", result.geometry.type);
    }

    [Fact]
    public async Task GetCurrentWeather_ReturnsNull_WhenApiFails()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://testapi/")
        };

        var clientFactoryMock = new Mock<IHttpClientFactory>();
        clientFactoryMock.Setup(cf => cf.CreateClient("WeatherApiClient"))
                         .Returns(httpClient);

        var weatherService = new WeatherService(clientFactoryMock.Object);

        // Act
        var result = await weatherService.GetCurrentWeather("Oslo");

        // Assert
        Assert.Null(result);
    }
}
