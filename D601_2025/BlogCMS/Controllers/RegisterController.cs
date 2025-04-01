using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        // GET: api/<RegisterController>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Pobrano listę użytkowników");
            return Ok("Wszystko gra :)");
        }
    }
}
