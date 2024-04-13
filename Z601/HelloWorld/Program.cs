using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var windDirection = new[]
{
    "N", "S", "W", "E", "NE", "NW", "SE", "SW"
};


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

app.MapGet("/", () => "Hello, World!");
app.MapGet("/users/{id}", (int id) => $"User ID: {id}");

app.MapGet("/temperature", () =>
{
    return Random.Shared.Next(-20, 55);
});

app.MapGet("/windDirection", () =>
{
    return windDirection[Random.Shared.Next(windDirection.Length)];
});

app.MapGet("/cityWeather", async (string city) =>
{
    var httpClient = new HttpClient();
    var apiKey = "5b2965ceb7056ac1cb87a3f4581e90b4"; // Zast¹p swoim kluczem API
    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
    try
    {
        var responce = await httpClient.GetAsync(url);
        responce.EnsureSuccessStatusCode();
        var content = await responce.Content.ReadAsStringAsync();
        //WeatherForecast weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(content);
        return Results.Ok(content);
    }
    catch (Exception ex)
    {

        throw;
    }
});

app.MapControllers();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
