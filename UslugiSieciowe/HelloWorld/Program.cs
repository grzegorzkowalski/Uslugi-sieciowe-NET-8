using HelloWorld.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var windDirection = new[]
{
    "N", "S", "W", "E", "NE", "NW", "SE", "SW"
};

app.MapGet("/temperature", () =>
{
    return Random.Shared.Next(-20, 55);
});

app.MapGet("/windDirection", () =>
{
    return windDirection[Random.Shared.Next(windDirection.Length)];
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/weatherByCity", async (string city) =>
{
    var httpClient = new HttpClient();
    var apiKey = "5b2965ceb7056ac1cb87a3f4581e90b4"; // Zast¹p swoim kluczem API
    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

    try
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

        return Results.Ok(weatherData);
    }
    catch (HttpRequestException ex)
    {
        // Obs³uga b³êdów po³¹czenia
        return Results.StatusCode(500);
    }
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
