using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityWeatherController : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> Get(string city)
        {
            var httpClient = new HttpClient();
            var APIkey = "4686bef79e65858daeab9e553ecdaeb5";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={APIkey}&units=metric";
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                return Results.Ok(weatherData);
            }
            catch (Exception ex)
            {
                var error = ex;
                return Results.StatusCode(500);
            }
        }

        [HttpPost]
        public IResult Post(string city)
        {
            return Results.Ok();
        }
    }
}
