using HelloWorld.Interface;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestInjection _testInjection;
        public TestController(ITestInjection testInjection) 
        {
            _testInjection = testInjection;
        }

        // GET: api/<TestController>
        [HttpGet]
        public string Get()
        {
            //var test = new TestInjection();
            //var testName = test.SetName();
            var testName = _testInjection.SetName();
            return testName;
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
