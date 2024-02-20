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

                return Ok(weatherData);
            }
            catch (HttpRequestException ex)
            {
                // Obsługa błędów połączenia
                return StatusCode(500);
            }
        }
    }
}
