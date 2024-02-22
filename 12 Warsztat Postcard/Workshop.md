## Warsztat: Tworzenie Aplikacji do Generowania Personalizowanych Pocztówek

### Wprowadzenie
1. W tym warsztacie przejdziemy przez proces tworzenia systemu, który pozwala użytkownikom 
na wysyłanie prośby o przygotowanie personalizowanej kartki pocztowej przez stronę internetową.
Proces ten obejmuje przekazanie prośby do `Web API`, zapisanie jej w 
`bazie danych` przez usługę `gRPC`, przetworzenie przez `Worker Service`, 
który wygeneruje pocztówkę za pomocą API `OpenAI` 
i ostatecznie wysłanie gotowej pocztówki do użytkownika za pośrednictwem `SignalR`.
1. Użycie każdej z wymienionych usług ćwiczyliśmy rozpisując konfiguracje na drobne kroki dlatego w opisie poniższego warsztatu pominięte są elementy oczywiste, które należy wykonać, żeby aplikacje działały poprawnie.
#### Krok 1 - WebAPI i SignalR

1. **Stwórz projekt WebAPI.**
2. **Skonfiguruj SignalR.**
    - Dodaj pakiet NuGet `Microsoft.AspNetCore.SignalR.Client`.
    - Zarejestruj usługę SignalR w `Program.cs`.
3. **Stwórz hub SignalR** dla komunikacji z klientem frontendowym.
4. **Obsłuż prośby o generowanie pocztówki**, przekazując je do usługi `gRPC` wraz z identyfikatorem klienta.
5. **Zaimplementuj grupy prywatne** dla wysyłania danych tylko do jednego klienta.

    ```csharp
    public async Task AddToGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }
    
    public async Task SendMessageToSpecificUser(string userId, string message)
    {
        await Clients.Group(userId).SendAsync("ReceiveMessage", message);
    }
    ```

#### Krok 2 - gRPC

1. **Stwórz projekt gRPC.**
    - Użyj Visual Studio lub dotnet CLI.
2. **Zdefiniuj serwis w pliku `.proto`** do zapisywania żądań użytkownika.
3. **Skonfiguruj klienta gRPC** w projekcie WebAPI.
4. Zapisuj w bazie danych każde żądanie z prośbą o pocztówkę.

#### Krok 3 - Worker Service

1. **Stwórz usługę Worker Service**, która regularnie sprawdza bazę danych pod kątem nowych próśb o pocztówki.
2. **Generuj pocztówki przy użyciu OpenAI API.**

    ```csharp
    using System.Net.Http.Headers;
    using Newtonsoft.Json;

    public class OpenAiImageGenerator
    {
        private readonly HttpClient _httpClient;

        public OpenAiImageGenerator()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_API_KEY");
        }

        public async Task GenerateImageAsync(string prompt)
        {
            var requestBody = new
            {
                model = "dall-e-3",
                prompt = prompt,
                n = 1,
                size = "1024x1024",
                response_format = "url",
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/images/generations", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                var imageUrl = (string)responseObject.data[0].url;
                Console.WriteLine($"Generated Image URL: {imageUrl}");
            }
            else
            {
                Console.WriteLine("Failed to generate the image.");
            }
        }
    }
    ```
3. Po wygenerowaniu pocztówki wywołaj odpowiedni endpoint w aplikacji typu SignalR, który będzie trigerował wysłanie informacji do klienta.
Prześlij na front link do pobrania pocztówki. 
```
[HttpPost("send-message")]
public async Task<IActionResult> SendMessageToClient(MessagePayload payload)
{
    await _hubContext.Clients.All.SendAsync("ReceiveMessage", payload.Message);
    return Ok();
}
```

#### Krok 4 - Frontend Integracja

1. **Zintegruj klienta przeglądarkowego**, aby mógł wysyłać prośby i odbierać wygenerowane pocztówki.

    ```javascript
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/postcardHub")
        .build();

    connection.on("ReceiveMessage", function(message) {
        console.log("Received message:", message);
    });

    connection.start().catch(err => console.error(err.toString()));
    ```

## Zakończenie
1. Po zakończeniu tego warsztatu będziesz miał funkcjonalny system do generowania i dostarczania personalizowanych pocztówek dla użytkowników, łączący w sobie technologie WebAPI, gRPC, SignalR oraz AI.
