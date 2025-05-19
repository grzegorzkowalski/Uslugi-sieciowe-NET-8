using Newtonsoft.Json;
using WeatherWorkerService.Data;
using WeatherWorkerService.Logic;
using WeatherWorkerService.Models;

namespace WeatherWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cities = new[] { "Warszawa", "Che³m", "Lublin" };
            var httpClient = new HttpClient();
            var apiKey = "44dd6177c8582aa89dc05870bcd84970";

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

                        var mapper = new WeatherMapper();
                        var weather = mapper.GetWeatherData(weatherData, city);

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            dbContext.Weather.Add(weather);
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }

                        _logger.LogInformation($"Pomyœlnie dodano dane pogodowe dla miasta: {city}.");

                        // Tutaj zapisz weatherData do bazy danych, pamiêtaj o mapowaniu
                        // Przyk³ad: _dbContext.WeatherData.Add(weatherData);
                        // await _dbContext.SaveChangesAsync(stoppingToken);           
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
