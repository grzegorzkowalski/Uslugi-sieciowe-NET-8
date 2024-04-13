using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly List<City> cities = new List<City>
        {
            new City { ID = 1, Name = "Warszawa" },
            new City { ID = 2, Name = "Kraków" },
            new City { ID = 3, Name = "Gdańsk" },
            new City { ID = 4, Name = "Wrocław" },
            new City { ID = 5, Name = "Poznań" },
            new City { ID = 6, Name = "Łódź" },
            new City { ID = 7, Name = "Szczecin" },
            new City { ID = 8, Name = "Bydgoszcz" },
            new City { ID = 9, Name = "Lublin" },
            new City { ID = 10, Name = "Białystok" }
        };

        // GET: api/<CityController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(cities);
        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var city = cities.FirstOrDefault(x => x.ID == id);  
            return Ok(city);
        }

        // POST api/<CityController>
        [HttpPost]
        public IActionResult Post(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required");
            }
            var cityExists = cities.Where(x => x.Name == city).FirstOrDefault();
            if (cityExists != null)
            {
                return Ok(cities);
            }
            cities.Add(new City() { Name = city, ID = cities.Count + 1 });
            return Ok(cities);
        }

        // PUT api/<CityController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string city)
        {
            City cityToUpdate = cities.FirstOrDefault(x => x.ID == id);
            if (cityToUpdate != null)
            {
                cityToUpdate.Name = city;
            }

            return Ok(cities);
        }

        // DELETE api/<CityController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            City cityToDelete = cities.FirstOrDefault(x => x.ID == id);
            if (cityToDelete != null)
            {
                cities.Remove(cityToDelete);
            }   
            return Ok(cities);
        }
    }
}
