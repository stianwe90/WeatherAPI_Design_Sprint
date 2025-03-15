namespace WeatherAPI.Dtos

{
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public Instant instant { get; set; }
        public Next12Hours next_12_hours { get; set; }
        public Next1Hours next_1_hours { get; set; }
        public Next6Hours next_6_hours { get; set; }
    }

    public class Details
    {
        public double air_pressure_at_sea_level { get; set; }
        public double air_temperature { get; set; }
        public double cloud_area_fraction { get; set; }
        public double relative_humidity { get; set; }
        public double wind_from_direction { get; set; }
        public double wind_speed { get; set; }
        public double precipitation_amount { get; set; }
    }

    public class Geometry
    {
        public List<double> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Instant
    {
        public Details details { get; set; }
    }

    public class Meta
    {
        public Units units { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Next12Hours
    {
        public Details details { get; set; }
        public Summary summary { get; set; }
    }

    public class Next1Hours
    {
        public Details details { get; set; }
        public Summary summary { get; set; }
    }

    public class Next6Hours
    {
        public Details details { get; set; }
        public Summary summary { get; set; }
    }

    public class Properties
    {
        public Meta meta { get; set; }
        public List<Timeseries> timeseries { get; set; }
    }

    public class WeatherDto
    {
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
        public string type { get; set; }
    }

    public class Summary
    {
        public string symbol_code { get; set; }
    }

    public class Timeseries
    {
        public Data data { get; set; }
        public DateTime time { get; set; }
    }

    public class Units
    {
        public string air_pressure_at_sea_level { get; set; }
        public string air_temperature { get; set; }
        public string cloud_area_fraction { get; set; }
        public string precipitation_amount { get; set; }
        public string relative_humidity { get; set; }
        public string wind_from_direction { get; set; }
        public string wind_speed { get; set; }
    }


}