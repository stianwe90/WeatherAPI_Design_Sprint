using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Register HttpClient
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins",
        builder => builder.WithOrigins("http://localhost:5000")  // Replace with your front-end URL
                        .WithMethods("GET")
                        .WithHeaders("Content-Type"));

});
// singleton, scoped, transient
// singleton: one instance for the lifetime of the application
// scoped: one instance for the lifetime of the request
// transient: one instance for each time it is requested

builder.Services.AddScoped<IWeatherService, WeatherService>(); // Register WeatherService

var app = builder.Build();

app.UseCors("AllowedOrigins"); // Enable CORS
app.UseAuthorization();
app.MapControllers();

app.Run();