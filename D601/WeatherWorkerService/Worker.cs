using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherWorkerService.Data;
using WeatherWorkerService.Mappers;
using WeatherWorkerService.Models;
using WeatherWorkerService.Repository;

namespace WeatherWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WeatherRepository _repository;

        public Worker(ILogger<Worker> logger, WeatherRepository repository)
        {
            _repository = repository;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cities = new[] { "Warszawa", "Che³m", "Lublin" };
            var httpClient = new HttpClient();
            var apiKey = "5b2965ceb7056ac1cb87a3f4581e90b4";

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var city in cities)
                {
                    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

                    try
                    {
                        var response = await httpClient.GetAsync(url, stoppingToken);
                        response.EnsureSuccessStatusCode();
                        var content = await response.Content.ReadAsStringAsync();
                        var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                        var openWeather = WeatherMapper.MapWeather(weatherData);
                        _repository.AddAsync(openWeather);         
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError($"B³¹d przy próbie pobrania pogody dla miasta {city}: {ex.Message}");
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
