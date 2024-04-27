using System;
using System.Data;

namespace HelloWorld.Models
{
    public class User
    {
        private ILogger _logger;
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(ILogger logger)
        {
            _logger = logger;
        }

        public void LogIn()
        {
            var message = $"User {Name} logged in - {DateTime.Now}";
            _logger.Log(message);
        }
    }
}
