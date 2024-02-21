## Zadanie 1 - konfiguracja mechanizmów uwierzytelniania i autoryzacji

1. W projekcie BlogCMS zainstaluj następujący pakiet:
```csharp
Microsoft.AspNetCore.Authentication.JwtBearer (6.0.27)
```
2. Dodaj ustawienie w `appsetting.json`. Otwórz appsetting.json i dodaj następujące Key, Issuer i Audience.
```
"Jwt": {
    "Key": "22DayVrx7KruOMOiSQcQXRRRW0UDptGP", // wygeneruj, co najmniej 32 znaki https://www.random.org/strings/
    "Issuer": "https://localhost:7128/", //Adres aplikacji wysyłanie danych
    "Audience": "https://localhost:7128/" //Adres aplikacji odbiór danych
}
```
3. W `Program.cs` dodaj obsługę JWT:
```
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
```
4. W `Program.cs` dodaj:
```
app.UseAuthentication();
app.UseAuthorization();
```
1. Dodaj klasę, która będzie zawierała listę dopuszczalnych kont, które mogą się zalogować.
Symulujesz obsługę kont z bazy danych.
```csharp
public class UserConstants
{
    public static List<LoginModel> Users = new()
        {
                new LoginModel(){ Username="Grzegorz",Password="TajneHaslo_1234",Role="Admin"}
        };
}
```

1. W folderze `Models` dodajemy:
```csharp
public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
} 
```
1. Dodajemy nowy `LoginController`.
```csharp
[Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }

        // To generate token
        private string GenerateToken(LoginModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private LoginModel Authenticate(LoginModel userLogin)
        {
            var currentUser = UserConstants.Users.FirstOrDefault(x => x.Username.ToLower() ==
                userLogin.Username.ToLower() && x.Password == userLogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    } 
```
1. Dodaj testowy kontroler `UserController`.
```csharp
using Flashcards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Flashcards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //For admin Only
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }
        private LoginModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new LoginModel
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
} 
```
1. Dodaj obsługę Bearer Tokenów do Swaggera w pliku `Program.cs`.
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Konfiguracja Swaggera do używania tokenów Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Proszę wprowadzić token JWT z prefiksem 'Bearer' w polu",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
```

1. Uruchom projekt.
1. Zwróć uwagę, że w `Swagger` pojawiłą się opcja Authorize.
1. Zaloguj się za pomocą danych użytkownika dodanych w klasie `UserConstants`.
1. Wykorzystaj endpoint typu Post `/api/Login`.
1. W odpowiedzi dostaniemy klucz, którym będziemy uwierzytelniać nasze zapytania. Np.
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IkdyemVnb3J6IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MDgzMDcxMzcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxMjgvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEyOC8ifQ.DIEVa2zTwFFKJniKzWJBWl8WrDgsvjI5MbJw2WL9YEY
```
1. W `Swaggerze` w polu autoryzacji dodaj token w następujący sposób:
```csharp
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IkdyemVnb3J6IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MDgzMDcxMzcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxMjgvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEyOC8ifQ.DIEVa2zTwFFKJniKzWJBWl8WrDgsvjI5MbJw2WL9YEY
```
1. Wykonaj request na chroniony endpoint `/api/User.Admins`.
Jeśli wszystko zrobiliśmy poprawnie w odpowiedzi dostaniemy: `Hi you are an Admin`. 
1. Jeśli nie przekażemy poprawnego tokenu w zapytaniu w odpowiedzi dostaniemy błąd `401`.
