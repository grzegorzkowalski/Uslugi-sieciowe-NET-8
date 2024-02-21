using HelloWorld.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city) 
        {
            var httpClient = new HttpClient();
            var apiKey = "5b2965ceb7056ac1cb87a3f4581e90b4"; // Zastąp swoim kluczem API
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                _logger.LogInformation($"Pogoda dla miasta {city} została pobrana.");
                return Ok(weatherData);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Błąd przy próbie pobrania pogody dla miasta {city}, kod: {ex.StatusCode}, opis błędu: {ex.Message}.");
                // Obsługa błędów połączenia
                return StatusCode(500);
            }
        }
    }
}
