## Zadanie 1 - Implementacja Worker Service, pobieranie prognozy pogody.

Przygotuj Worker Service będzie co 30 sekund wysyłać zapytanie o pogodę
dla trzech wybranych miast i zapisywać odpowiedzi do bazy danych.

### Krok 1: Utworzenie projektu Worker Service
1. Utwórz nowy projekt Worker Service przy pomocy CLI .NET:

```
dotnet new worker -n WeatherWorkerService
cd WeatherWorkerService
```

1. Dodaj wymagane pakiety NuGet do projektu:
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Newtonsoft.Json
```
#### Krok 2: Konfiguracja bazy i Entity Framework Core
1. Zdefiniuj model `WeatherData` reprezentujący dane pogodowe. Możesz użyć modelu `WeatherData ` z poprzednich zadań.
1. Zdefiniuj klasę, w której będziesz zapisywał wybrane przez Ciebie dane pogodowe. Nie musisz zapisywać całego modelu.
1. Dodaj metodę mapującą z modelu `WeatherData` na model `Weather`, któy utworzysz.
1. Utwórz klasę kontekstu ApplicationDbContext. Dodaj odpowiedni DbSet.
1. Stwórz bazę danych i skonfiguruj connectionstring.
1. Zarejestruj context w `Program.cs`.
1. Wygeneruj migrację i zaktualizuj bazę danych.

#### Krok 3: Implementacja Worker Service
W serwisie zmodyfikuj `ExecuteAsync` aby co 30 sekund wysyłał zapytania o pogodę dla trzech wybranych miast:
```
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    var cities = new[] { "Warszawa", "Chełm", "Lublin" };
    var httpClient = new HttpClient();
    var apiKey = "Twój klucz API";

    while (!stoppingToken.IsCancellationRequested)
    {
        foreach (var city in cities)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                var response = await httpClient.GetAsync(url, stoppingToken);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                // Tutaj zapisz weatherData do bazy danych, pamiętaj o mapowaniu
                // Przykład: _dbContext.WeatherData.Add(weatherData);
                // await _dbContext.SaveChangesAsync(stoppingToken);           
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Błąd przy próbie pobrania pogody dla miasta {city}: {ex.Message}");
            }
        }
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
    }
}
```

1. Upewnij się, że skonfigurowałeś wstrzykiwanie zależności dla `ApplicationDbContext` i `ILogger<Worker>` w konstruktorze klasy Worker.

#### Krok 4: Testowanie i uruchomienie
1. Po zaimplementowaniu powyższej logiki, Twój Worker Service jest gotowy do uruchomienia. Uruchom aplikację przy pomocy:
```
dotnet run
```

1. Usługa będzie teraz cyklicznie wysyłać zapytania o pogodę dla wskazanych miast co 30 sekund i zapisywać wyniki do bazy danych.