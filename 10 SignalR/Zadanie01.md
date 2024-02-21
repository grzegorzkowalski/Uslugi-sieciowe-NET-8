## Zadanie 1 - Implementacja serwisu SignalR z zewnętrznym klientem.

1. Wykorzystaj aplikację `SignalrImplementation` gdzie jest dodana obsługa `SignalR`, którą widziałeś na slajdach.
1. Wykorzystaj klienta `signalRvite`.
1. Żeby zadział klient `html/js` wykonaj następujące kroki.
1. Zainstaluj Node w wersji LTS [https://nodejs.org/en](https://nodejs.org/en)
1. Wewnątrz folderu `signalRvite` otwórz konsolę git bash i zainstaluj niezbędne pakiety `npm i`.
1. Przetestuj czy kod uruchamia się poprawnie `npm run dev`.
1. W pliku `index.html` w znaczniku `<head>` dodaj skrypt klienta z biblioteką SignalR.
```js
  <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.js" integrity="sha512-pn4yorWMbHHvdsldBpkTNjJaoadsoYs/ZgOYHSHUtivn1j/Ddgdnlgt1egjQcP8j4atM3TR+tgIqgjhi5Z11KQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
```
1. Dodaj nową sekcję w `body` strony:
```html
  <section id="signalR" class="gallery-container">
  <div class="container">
    <div class="row p-1">
        <div class="col-1">User</div>
        <div class="col-5"><input type="text" id="userInput" /></div>
    </div>
    <div class="row p-1">
        <div class="col-1">Message</div>
        <div class="col-5"><input type="text" class="w-100" id="messageInput" /></div>
    </div>
    <div class="row p-1">
        <div class="col-6 text-end">
            <input type="button" id="sendButton" value="Send Message" />
        </div>
    </div>
    <div class="row p-1">
        <div class="col-6">
            <hr />
        </div>
    </div>
    <div class="row p-1">
        <div class="col-6">
            <ul id="messagesList"></ul>
        </div>
    </div>
</div>
<script src="js/chat.js"></script>
</section>
```
1. W katalogu głównym dodaj folder `js` i plik chat.js z następującą zawartością:
```js
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7099/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
```
1. Zwróć uwagę na tą linię:
```js
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7099/chatHub").build();
```
1. Wstaw port, któego używa Twoja aplikacja SignalR.
1. Uruchom kod `npm run dev`.
1. Aplikacja nie działa jeszcze poprawnie, bo mamy "problem" z CORS.
1. Musimy zmienić ustawienia w aplikacji SignalR.
1. W pliku `Program.cs` na samej górze dodaj:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:5174") // Specify the allowed origin(s)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()); // Necessary for SignalR
}); 
```
1. Zwróć uwagę, żeby port był zgodny z portem Twojej aplikacji frontendowej.
1. Przed routingiem dodaj jeszcze:
```csharp
app.UseCors("CorsPolicy");
```
1. Zrestartuj obie aplikacje i przetestuj komunikację między aplikacjami.



