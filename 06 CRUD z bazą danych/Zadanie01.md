## Zadanie 1 - Stworzenie aplikacji do zarządzania inspirującymi cytatami podróżniczymi za pomocą .NET 8 Web API i Entity Framework Core.

#### Krok 1: Konfiguracja projektu
1. Utwórz nowy projekt `Web API`:
1. Otwórz terminal i użyj poniższego polecenia, aby utworzyć nowy projekt Web API.

```dotnet new webapi -n TravelQuotesApi```

1. Przejdź do utworzonego folderu projektu:

```cd TravelQuotesApi```

1. Dodaj Entity Framework Core:
Zainstaluj pakiety NuGet dla Entity Framework Core. Załóżmy, że będziemy używać SQL Server.

```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Aby umożliwić migracje, dodaj również:

```
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
#### Krok 2: Konfiguracja Modelu i DbContext
1. Utwórz model danych:
W folderze projektu utwórz folder Models, a w nim plik Quote.cs z definicją modelu.

```
namespace TravelQuotesApi.Models
{
public class Quote
{
public int Id { get; set; }
public string Author { get; set; }
public string Message { get; set; }
}
}
```

1. Utwórz DbContext:
W głównym folderze projektu utwórz folder `Data`,
a w nim plik `ApplicationDbContext.cs` z definicją kontekstu.

```
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }
    }
}
```
#### Krok 3: Konfiguracja połączenia z bazą danych
1. Dodaj ciąg połączenia do bazy danych:
W pliku `appsettings.json` dodaj ciąg połączenia do bazy danych.
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TravelQuotesDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```
1. Zarejestruj DbContext:
W pliku `Program.cs` zarejestruj ApplicationDbContext z EF Core, używając ciągu połączenia.
```
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodaj DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Reszta konfiguracji...
```

#### Krok 4: Migracje i aktualizacja bazy danych
1. Utwórz migrację:
W terminalu, będąc w folderze projektu, utwórz migrację.

```
dotnet ef migrations add Initial
```

1. Zaktualizuj bazę danych:
Zastosuj migrację do bazy danych, aby utworzyć schemat.

```
dotnet ef database update
```

#### Krok 5: Utworzenie kontrolera API

1. Utwórz kontroler:
W folderze Controllers utwórz nowy plik `QuotesController.cs`.

```
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Models;

[Route("api/[controller]")]
[ApiController]
public class QuotesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuotesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Quotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
    {
        return await _context.Quotes.ToListAsync();
    }

    // POST: api/Quotes
    [HttpPost]
    public async Task<ActionResult<Quote>> PostQuote(Quote quote)
    {
        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
    }

    // Dodaj więcej metod API według potrzeb...
}
```

#### Krok 6: Utworzenie interfejsu IRepository

1. Utwórz folder `Interfaces` w głównym katalogu projektu.
1. Dodaj interfejs `IRepository`. W tym folderze utwórz plik `IRepository.cs` z ogólnym interfejsem repozytorium, który będzie można później rozszerzyć dla konkretnych modeli.

```
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelQuotesApi.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
```
Krok 7: Implementacja Repozytorium
```
using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

public class QuoteRepository : IRepository<Quote>
{
    private readonly ApplicationDbContext _context;

    public QuoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Quote> AddAsync(Quote entity)
    {
        _context.Quotes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var quote = await _context.Quotes.FindAsync(id);
        if (quote != null)
        {
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Quote>> GetAllAsync()
    {
        return await _context.Quotes.ToListAsync();
    }

    public async Task<Quote> GetByIdAsync(int id)
    {
        return await _context.Quotes.FindAsync(id);
    }

    public async Task UpdateAsync(Quote entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
```

#### Krok 8: Rejestracja repozytorium w kontenerze DI
1. Zarejestruj QuoteRepository jako serwis W `Program.cs` dodaj `QuoteRepository` do kontenera usług, aby można było go wstrzyknąć tam, gdzie jest potrzebny.

#### Krok 9: Modyfikacja kontrolera do używania repozytorium
1. Zmodyfikuj `QuotesController`, aby używał `IRepository<Quote>`:
Zaktualizuj konstruktor i metody kontrolera, aby korzystały z `IRepository<Quote>` zamiast bezpośrednio z `ApplicationDbContext`.

```
[Route("api/[controller]")]
[ApiController]
public class QuotesController : ControllerBase
{
    private readonly IRepository<Quote> _quoteRepository;

    public QuotesController(IRepository<Quote> quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    // GET: api/Quotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
    {
        var quotes = await _quoteRepository.GetAllAsync();
        return Ok(quotes);
    }

    // Pozostałe metody...
}
```
#### Krok 10: Dodaj Swaggera do projektu

#### Krok 11: Uruchomienie aplikacji

Uruchom aplikację:
W terminalu, będąc w folderze projektu, uruchom aplikację.
```
dotnet run
```

#### Krok 12: Przetestuj aplikację z pomocą Swaggera i Postmana

1. Dodaj kilka inspirujących cytatów podróżniczych, które wykorzystamy na stronie.

