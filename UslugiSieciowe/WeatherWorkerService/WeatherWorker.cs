using BlogCMS.Data;
using Newtonsoft.Json;
using WeatherWorkerService.Models;

namespace WeatherWorkerService
{
    public class WeatherWorker : BackgroundService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeatherWorker> _logger;

        public WeatherWorker(ILogger<WeatherWorker> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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

                        var weather = new WeatherModel()
                        {
                            City = city,
                            WeatherDate = DateTime.Now,
                            Temp = weatherData.Main.Temp,
                            FeelsLike = weatherData.Main.FeelsLike,
                            TempMin = weatherData.Main.TempMin,
                            TempMax = weatherData.Main.TempMax,
                            Pressure = weatherData.Main.Pressure,
                            Humidity = weatherData.Main.Humidity
                        }; 

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
