## Zadanie 1 z wykładowcą

1. Dodaj globalną obsługę błedów do potoku w `Program.cs`.
```
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/api/error");
}
```

2. Dodaj kontroler do obsługi błędów.

```
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/api/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                // Logowanie błędu
                Console.WriteLine(exceptionFeature.Error);

                // Zwróć odpowiedź dla klienta
                return Problem(detail: exceptionFeature.Error.Message, title: "Wystąpił błąd");
            }

            return Problem(); // Zwraca odpowiedź 500 Internal Server Error bez szczegółów
        }
    }
```

3. Żeby przetestować brak obsługi w trybie developerskim (nie obsługujemy błędu, żeby dowiedzieć się o problemie) stwórz oddatkowy kontroler.

```
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
```

4. Wywołaj kontroler żądaniem typu `GET`.
5. Zobaczysz jak wygląda nieobsłużony błąd.
6. Teraz przetestujemy obsługę globalną błędu. Na środowisku developerskim na potrzebę tego testu zmień obsługę błędów w pliku `Program.cs`.

```
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/api/error");
}
else
{
    app.UseExceptionHandler("/api/error");
}
```
7. Wywołaj kontroler `TestErrorsController` żądaniem typu `GET`.
8. Po zakończeniu ćwiczenia przywróć wcześniejszą konfigurację globalnej obsługi błędów.
