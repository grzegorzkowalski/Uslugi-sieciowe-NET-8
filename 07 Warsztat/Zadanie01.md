## Warsztat: Budowa CRUD API z wykorzystaniem, .NET , wzorca Repository i WebAPI

### Tematyka: System zarządzania treścią (CMS) dla bloga

### Cel warsztatu:
Stworzenie prostego systemu zarządzania treścią (CMS) dla bloga, umożliwiającego dodawanie, odczytywanie, aktualizowanie i usuwanie postów.

### Krok 1: Konfiguracja Projektu

#### 1. Utworzenie Projektu WebAPI
- Użyj CLI .NET do utworzenia nowego projektu WebAPI: `dotnet new webapi -n BlogCMS`
- Przejdź do katalogu projektu: `cd BlogCMS`

#### 2. Instalacja Pakietów NuGet
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
### Krok 2: Konfiguracja Bazy Danych

#### 1. Utworzenie Bazy Danych
- Utwórz bazę danych SQL Server o nazwie `blog`.

### Krok 3: Implementacja Modelu

#### Definicja Modelu Posta
```csharp
namespace BlogCMS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl {get; set; }
        public DateTime Published { get; set; }
    }
}
```

### Krok 4: Implementacja Wzorca Repository

Najpierw zdefiniuj interfejs `IRepository<T>`, który będzie służył jako podstawa dla wszystkich operacji CRUD w aplikacji:

```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<int> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}
```
Implementacja EF Core Repository
```
public class EfCoreRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private DbSet<T> _entities;

    public EfCoreRepository(DbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

    public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }
}

```

### Krok 5: Implementacja Kontrolera

Utwórz `PostsController`, który wykorzysta `IRepository<Post>` do obsługi żądań HTTP. Kontroler ten będzie odpowiedzialny za przekierowywanie żądań do odpowiednich metod w repozytorium i zwracanie odpowiedzi HTTP.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlogCMS.Models; // Upewnij się, że używasz odpowiedniej przestrzeni nazw
using BlogCMS.Repositories; // Upewnij się, że używasz odpowiedniej przestrzeni nazw

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IRepository<Post> _postRepository;

    public PostsController(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    // POST: api/posts
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newPostId = await _postRepository.AddAsync(post);
        return CreatedAtAction(nameof(GetPostById), new { id = newPostId }, post);
    }

    // PUT: api/posts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }

        var result = await _postRepository.UpdateAsync(post);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/posts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _postRepository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
```

### Krok 6: Konfiguracja Dependency Injection i Bazy Danych

W tym kroku skonfigurujesz Dependency Injection (DI) w Twojej aplikacji .NET 6, aby umożliwić łatwe wstrzykiwanie zależności, takich jak `IRepository<Post>` i Twoje konkretne repozytorium `PostRepository`. Ponadto, ustawisz połączenie z bazą danych.
```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<Post>, EfCoreRepository<Post>>();

var app = builder.Build();

```

Konfiguracja Connection String
Dodaj ciąg połączenia do `appsettings.json`.

#### Konfiguracja `IServiceCollection`

Musisz dodać swoje repozytorium `Program.cs`. Poniżej przedstawiono, jak to zrobić:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Rejestracja repozytorium
builder.Services.AddScoped<IRepository<Post>, PostRepository>();

var app = builder.Build();

// Konfiguracja middleware i endpointów
// ...

app.Run();
```

Dodaj również obsługę wstrzykiwania contextu w `Program.cs` i repozytorium.

### Krok 7: Migracje i Database Update

Tworzenie Migracji
```
dotnet ef migrations add InitialCreate

```

Aktualizacja Bazy Danych
```
dotnet ef database update
```


### Krok 8: Testowanie API

Po skonfigurowaniu Twojego API i zaimplementowaniu operacji CRUD, nadszedł czas, aby przetestować Twoje endpointy i upewnić się, że wszystko działa jak należy. Do testowania API możesz użyć narzędzi takich jak Postman lub Swagger UI.

#### Testowanie za pomocą Postmana

1. **Instalacja Postmana**: Jeśli jeszcze nie masz Postmana, pobierz i zainstaluj go ze strony [Postman](https://www.postman.com/downloads/).

2. **Tworzenie Nowej Kolekcji**: Utwórz nową kolekcję w Postmanie, która będzie zawierać Twoje żądania do API.

3. **Konfiguracja Żądań**: Dla każdej operacji CRUD skonfiguruj odpowiednie żądanie w Postmanie:
    - **GET** `/api/posts` - Pobranie wszystkich postów.
    - **GET** `/api/posts/{id}` - Pobranie posta o konkretnym ID.
    - **POST** `/api/posts` - Dodanie nowego posta (dodaj odpowiednie dane w body żądania).
    - **PUT** `/api/posts/{id}` - Aktualizacja posta o konkretnym ID (dodaj zmienione dane w body żądania).
    - **DELETE** `/api/posts/{id}` - Usunięcie posta o konkretnym ID.

4. **Wykonywanie Żądań**: Wykonaj żądania i sprawdź odpowiedzi od serwera. Upewnij się, że wszystkie operacje zwracają oczekiwane wyniki.

#### Testowanie za pomocą Swagger UI

Jeśli Twoje API .NET Core jest skonfigurowane do używania Swaggera (zwykle jest to domyślnie włączone w nowych projektach API .NET Core), możesz łatwo przetestować swoje API za pomocą wbudowanego interfejsu użytkownika Swagger.

1. **Uruchom Swoje API**: Uruchom aplikację .NET Core.

2. **Otwórz Swagger UI**: W przeglądarce internetowej przejdź do URL Swagger UI, który zwykle znajduje się pod adresem `https://localhost:5001/swagger` (port może się różnić w zależności od konfiguracji).

3. **Testowanie Endpointów**: Za pomocą interfejsu Swagger UI możesz wykonywać żądania do Twojego API bezpośrednio z przeglądarki. Swagger automatycznie generuje interfejs dla wszystkich Twoich endpointów wraz z możliwością wprowadzania danych wejściowych (takich jak parametry żądania i ciała żądań).

#### Analiza Wyników

Po przetestowaniu wszystkich endpointów przeanalizuj otrzymane odpowiedzi. Sprawdź, czy statusy odpowiedzi HTTP są odpowiednie dla każdej operacji (np. 200 OK dla operacji odczytu, 201 Created dla operacji tworzenia, itp.) oraz czy dane zwracane przez API są poprawne.

#### Wnioski

Testowanie jest kluczowym elementem procesu deweloperskiego, a narzędzia takie jak Postman i Swagger UI znacznie ułatwiają to zadanie, oferując przejrzysty i wygodny sposób na interakcję z Twoim API.

### Krok 8: Refaktoryzacja i Rozszerzanie

Po wstępnych testach i potwierdzeniu, że podstawowa funkcjonalność Twojego API działa poprawnie, możesz rozważyć kilka kierunków dla dalszego rozwoju i ulepszenia Twojego projektu.

#### Dodawanie Nowych Funkcjonalności

1. **Filtrowanie Postów**
   - Rozszerz repozytorium o metodę umożliwiającą filtrowanie postów według różnych kryteriów, np. daty publikacji, słów kluczowych w tytule lub treści.

2. **Paginacja i Sortowanie**
   - Implementacja paginacji i sortowania w API pozwoli na bardziej efektywne zarządzanie dużymi zestawami danych.
   - Można dodać parametry do endpointów GET, które określą, jakie rekordy powinny być zwrócone i w jakiej kolejności.

3. **Uwierzytelnianie i Autentykacja**
   - Zabezpiecz swoje API, dodając obsługę autentykacji i autoryzacji, np. za pomocą JWT (JSON Web Tokens).
   - Określ, które endpointy wymagają zalogowanego użytkownika, a które są dostępne publicznie.

#### Refaktoryzacja

2. **Middleware do Obsługi Błędów**
   - Implementacja middleware do obsługi błędów pozwoli na centralne zarządzanie błędami i spójne formatowanie odpowiedzi błędów.

3. **Walidacja Modeli**
   - Użyj atrybutów walidacji na modelach DTO (Data Transfer Object), aby upewnić się, że dane wejściowe od użytkownika są prawidłowe przed przekazaniem ich do logiki biznesowej.

4. **Dokumentacja API**
   - Ulepsz dokumentację swojego API, korzystając z narzędzi takich jak Swagger, które umożliwia generowanie interaktywnej dokumentacji na podstawie Twojego kodu.

#### Wnioski

Rozszerzanie i refaktoryzacja projektu to proces ciągły. Dzięki regularnym ulepszeniom i dostosowaniom, Twój projekt będzie mógł lepiej sprostać rosnącym wymaganiom i oczekiwaniom użytkowników. Pamiętaj, aby każdą znaczącą zmianę poprzedzać testami, aby utrzymać jakość i stabilność Twojego API na wysokim poziomie.
