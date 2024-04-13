using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        // GET api/<WeatherController>/Warszawa
        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            var httpClient = new HttpClient();
            var apiKey = "5b2965ceb7056ac1cb87a3f4581e90b4"; // Zastąp swoim kluczem API
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}" +
                $"&appid={apiKey}&units=metric";
            try
            {
                var responce = await httpClient.GetAsync(url);
                responce.EnsureSuccessStatusCode();
                var content = await responce.Content.ReadAsStringAsync();
                //WeatherForecast weatherForecast =
                //JsonConvert.DeserializeObject<WeatherForecast>(content);
                return Ok(content);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
