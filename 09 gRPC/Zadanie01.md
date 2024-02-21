## Zadanie 1 - Aplikacja gRPC Zwracająca Losowy Cytat z Bazy Danych

W tym ćwiczeniu stworzysz prostą aplikację gRPC, która komunikuje się z bazą danych i losowo zwraca cytat. Przykładem modelu, który będziemy używać, jest Quote prezentujący w bazie cytat użytkownika wraz z polem zdefiniowanym twórcą i wiadomością.

#### Krok 1: Przygotowanie Projektu
1. Utworzenie projektu gRPC:

1. Uruchom wiersz poleceń lub terminal.
1. Utwórz projekt używając szablonu gRPC:
```
dotnet new grpc -n RandomQuoteGrpc
```

1. Przejdź do utworzonego folderu projektu:

```cd RandomQuoteGrpc```

1. Instalacja pakietów NuGet dla Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```
#### Krok 2: Model i Baza Danych
Konfiguracja DbContext:
W katalogu Data (jeśli nie istnieje, go stwórz) utwórz plik AppDbContext.cs.
Wpisz w nim następujący kod:
```html
using Microsoft.EntityFrameworkCore;
using RandomQuoteGrpc.Models;

public class AppDbContext : DbContext
{
    public DbSet<Quote> Quotes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tutaj możesz zdefiniować początkowy zestaw cytatów do migracji
    }
}
```
1. Utworzenie Modelu Quote:
Stwórz w katalogu projektu nowy folder Models, a w nim plik Quote.cs.
Wpisz do utworzonego modelu:
```html
namespace RandomQuoteGrpc.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
```

1. Dodaj wstrzyknięcie contextu.
```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionStringName")));
```

1. Dodaj `connectionstring` do bazy danych.
```csharp
"ConnectionStrings": {
    "YourConnectionStringName": "Twoje dane połączeniowe do bazy danych"
}
```

#### Krok 3: Serwis gRPC
Modyfikacja Defined Service w Protos/greet.proto:
Dostosuj ten mieszczonego redirecta, zamieniając zestaw poleceń na wycinek odpowiadający Typowi Cytatu:
```html
syntax = "proto3";

option csharp_namespace = "RandomQuoteGrpc";

package quote;

service QuoteService {
  rpc GetRandomQuote (QuoteRequest) returns (QuoteReply);
}

message QuoteRequest {}

message QuoteReply {
  int32 id = 1;
  string author = 2;
  string message = 3;
}
```

1. Implementacja gRPC Service:
```csharp
public class QuoteService : Quote.QuoteBase
{
    private readonly AppDbContext _context;
    
    public QuoteService(AppDbContext context)
    {
        _context = context;
    }
    
    public override async Task<QuoteReply> GetRandomQuote(QuoteRequest request, ServerCallContext context)
    {
        var count = await _context.Quotes.CountAsync();
        var index = new Random().Next(count);
        var randomQuote = await _context.Quotes.Skip(index).FirstOrDefaultAsync();
        
        if (randomQuote == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "No quotes found."));
        }   
        return new QuoteReply
        {
            Id = randomQuote.Id,
            Author = randomQuote.Author,
            Message = randomQuote.Message
        };
    }
}
```