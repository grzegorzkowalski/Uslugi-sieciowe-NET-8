using HelloWorld.Models;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/api/error");
}
else
{
    app.UseExceptionHandler("/api/error");
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var windDirection = new[]
{
    "N", "S", "W", "E", "NE", "NW", "SE", "SW"
};

var cities = new List<string>();

app.MapPost("/addCity", (string city) =>
{
    cities.Add(city);
});

app.MapGet("/getCities", () =>
{
    return Results.Ok(cities);
});

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

app.MapControllers();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
