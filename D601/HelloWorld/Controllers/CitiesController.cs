using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        //private List<City> _cities = new List<string>();
        private string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "cities.json");

        // GET: api/<CitiesController>
        [HttpGet]
        public List<City> Get()
        {
            
            string json = System.IO.File.ReadAllText(_filePath);
            var cities = JsonSerializer.Deserialize<List<City>>(json); 
            
            return cities;
        }

        // GET api/<CitiesController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(int id)
        {
            try
            {
                string json = await System.IO.File.ReadAllTextAsync(_filePath);
                var cities = JsonSerializer.Deserialize<List<City>>(json);
                var city = cities.FirstOrDefault(x => x.Id == id);
                return Results.Ok<City>(city);
            }
            catch (Exception e)
            {

                return Results.Problem(e.Message);
            }
        }

        // POST api/<CitiesController>
        [HttpPost]
        public async Task<IResult> Post(string city)
        {
            try
            {
                string json = await System.IO.File.ReadAllTextAsync(_filePath);
                var cities = JsonSerializer.Deserialize<List<City>>(json);
                cities.Add(new City() { Name = city, Id = cities.Count+1 });
                var obj = JsonSerializer.Serialize(cities);
                await System.IO.File.WriteAllTextAsync(_filePath, obj);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

        }

        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                string json = await System.IO.File.ReadAllTextAsync(_filePath);
                var cities = JsonSerializer.Deserialize<List<City>>(json);
                var cityToRemove = cities.FirstOrDefault(x => x.Id == id);
                cities.Remove(cityToRemove);
                var obj = JsonSerializer.Serialize(cities);
                await System.IO.File.WriteAllTextAsync(_filePath, obj);
                return Results.NoContent();
            }
            catch (Exception e)
            {

                return Results.Problem(e.Message);
            }
        }
    }
}
