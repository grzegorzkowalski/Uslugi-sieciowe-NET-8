using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestErrorsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("Testowy wyjątek");
        }
    }
}
