## Zadanie 2

1. Podstawowe logowanie w aplikacji typu WebAPI jest dodane domyślnie.
Żeby z niego skorzystać wystarczy do kontrolera wstrzyknąć instancję logera.

```
private RegisterRepository _repository;
private readonly ILogger<RegisterController> _logger;

public RegisterController(RegisterRepository repository, ILogger<RegisterController> logger)
{
    _repository = repository;
    _logger = logger;
}
```
2. Dzięki temu możemy już wywoływać metody logera.
```
// GET: api/<RegisterController>
[HttpGet]
public List<User> Get()
{
    _logger.LogInformation("Pobrano listę użytkowników");
    return new List<User>();
}
```
3. Komunikaty z logera będziemy widzieć w `Output` w Visual Studio.
4. Jeśli chcemy zapisywać logi w bazie danych musimy skorzystać z dodatkowego narzędzia.
Skorzystamy z biblioteki `Serilog`.
5. Zainstaluj odpowiednie pakiety w projekcie.
```
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Settings.Configuration
dotnet add package Serilog.Sinks.MSSqlServer
```
6. Dodaj konfigurację w `Program.cs`. Upewnij się, że dodajesz ją na samym początku zaraz po metodzie `WebApplication.CreateBuilder(args);`.

```
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));
```

7. Dodaj do pliku `appsettings.json` connection string do bazy danych np.
```
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HelloWorld;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },
```
8. Dodaj konfigurację `Serilog` i odpowiedniego `sinka` pliku konfiguracyjny.

```
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.MSSqlServer" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "DefaultConnection",
          "sinkOptionsSection": {
            "tableName": "SeriLogs",
            "autoCreateSqlTable": true
          }
        }
      }
    ],
    "Enrich": [ "FromLogContext" ]
  },
```
9. Od teraz wszystko, co zalogujesz za pomocą metod typu `_logger.LogInformation("Pobrano listę użytkowników");` zostanie wyświetlone na konsoli i dodane do bazy danych.
10. Sprawdź jak jest zbudowana tabela `SeriLogs` i jakie informacje zostały do niej zapisane.    

