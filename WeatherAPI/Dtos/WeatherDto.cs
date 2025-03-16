namespace WeatherAPI.Dtos;

public class WeatherDto
{
    public string Location { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public List<WeatherDetails> Forecasts { get; set; } = null!;
}

public class WeatherDetails
{
    public DateTime Timestamp { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public double Precipitation { get; set; }
    public string SymbolCode { get; set; } = null!;
}
