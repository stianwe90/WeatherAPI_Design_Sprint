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

var app = builder.Build();

app.UseCors("AllowedOrigins"); // Enable CORS
app.UseAuthorization();
app.MapControllers();

app.Run();