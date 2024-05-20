using Newtonsoft.Json;
using WeatherWorkerService.Models;

namespace WeatherWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cities = new[] { "Warszawa", "Che�m", "Lublin" };
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

                        // Tutaj zapisz weatherData do bazy danych, pami�taj o mapowaniu
                        // Przyk�ad: _dbContext.WeatherData.Add(weatherData);
                        // await _dbContext.SaveChangesAsync(stoppingToken);           
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError($"B��d przy pr�bie pobrania pogody dla miasta {city}: {ex.Message}");
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
