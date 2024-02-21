## Zadanie 1 - Warsztat: Tworzenie usługi gRPC do zarządzania treścią bloga z wykorzystaniem Entity Framework Core

Cel warsztatu: Stworzenie usługi gRPC, która przejmie odpowiedzialność za operacje CRUD na postach bloga, korzystając z Entity Framework Core. Aplikacja Web API będzie komunikować się z bazą danych przez usługę gRPC.

#### Krok 1: Utworzenie projektu gRPC
1. Użyj CLI .NET, aby utworzyć nowy projekt gRPC:

```
dotnet new grpc -n BlogGrpcService
```

1. Przejdź do utworzonego folderu projektu:

```cd cd BlogGrpcService```

1. Instalacja pakietów NuGet dla Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Aby umożliwić migracje, dodaj również:

```
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
#### Krok 2: Konfiguracja bazy danych
1. Utworzenie bazy danych:
Utwórz bazę danych SQL Server o nazwie BlogDb.

1. Konfiguracja DbContext:
W folderze Data utwórz plik BlogDbContext.cs z następującą zawartością:
```
using Microsoft.EntityFrameworkCore;
using BlogGrpcService.Models;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }
}

```
1. Definicja modelu:
W folderze Models utwórz plik Post.cs:
```csharp
namespace BlogGrpcService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Published { get; set; }
    }
}

```

#### Definiowanie usługi w pliku .proto
1. Modyfikacja pliku Protos/blog.proto:
Zdefiniuj usługę gRPC oraz wiadomości dla operacji CRUD:
```
syntax = "proto3";

option csharp_namespace = "BlogGrpcService.Protos";

package blog;

// Usługa gRPC dla postów bloga
service BlogPosts {
  rpc CreatePost (CreatePostRequest) returns (PostReply);
  rpc GetPost (GetPostRequest) returns (PostReply);
  rpc UpdatePost (UpdatePostRequest) returns (PostReply);
  rpc DeletePost (DeletePostRequest) returns (DeletePostReply);
}

message CreatePostRequest {
  string title = 1;
  string content = 2;
}

message GetPostRequest {
  int32 id = 1;
}

message UpdatePostRequest {
  int32 id = 1;
  string title = 2;
  string content = 3;
}

message DeletePostRequest {
  int32 id = 1;
}

message PostReply {
  int32 id = 1;
  string title = 2;
  string content = 3;
  string published = 4;
}

message DeletePostReply {
  bool success = 1;
}
```

##### Krok 4: Implementacja serwisu gRPC
1. Aby zaimplementować serwis gRPC, musisz stworzyć logikę obsługującą operacje CRUD w klasie BlogPostsService. Ta klasa będzie używać BlogDbContext do interakcji z bazą danych.

Services/BlogPostsService.cs
```
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using BlogGrpcService.Protos;
using BlogGrpcService.Models;

public class BlogPostsService : BlogPosts.BlogPostsBase
{
    private readonly BlogDbContext _context;

    public BlogPostsService(BlogDbContext context)
    {
        _context = context;
    }

    public override async Task<PostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
    {
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            Published = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return new PostReply
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Published = post.Published.ToString()
        };
    }

    public override async Task<PostReply> GetPost(GetPostRequest request, ServerCallContext context)
    {
        var post = await _context.Posts.FindAsync(request.Id);

        if (post == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
        }

        return new PostReply
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Published = post.Published.ToString()
        };
    }

    public override async Task<PostReply> UpdatePost(UpdatePostRequest request, ServerCallContext context)
    {
        var post = await _context.Posts.FindAsync(request.Id);
        
        if (post == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
        }

        post.Title = request.Title;
        post.Content = request.Content;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();

        return new PostReply
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Published = post.Published.ToString()
        };
    }

    public override async Task<DeletePostReply> DeletePost(DeletePostRequest request, ServerCallContext context)
    {
        var post = await _context.Posts.FindAsync(request.Id);

        if (post == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Post with ID={request.Id} is not found."));
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return new DeletePostReply
        {
            Success = true
        };
    }
}
```

#### Krok 5: Konfiguracja serwera gRPC

1. Program.cs
```sharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDb")));

var app = builder.Build();

app.MapGrpcService<BlogPostsService>();
app.MapGet("/", () => "This server hosts a gRPC service for Blog.");

app.Run();
```

#### Krok 6: Migracje i aktualizacja bazy danych

1. Po zdefiniowaniu modelu Post i BlogDbContext, użyj EF Core do stworzenia i zastosowania migracji:
```csharp
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Krok 7: Integracja z Web API

1. Dodanie klienta gRPC do aplikacji Web API
Instalacja pakietu gRPC:
W aplikacji Web API dodaj pakiet klienta gRPC:
```
dotnet add package Grpc.Net.Client
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
```

1. Skopiuj plik .proto:
Upewnij się, że masz już zdefiniowany w projektu gRPC ten sam plik .proto, który planujesz użyć w swojej aplikacji Web API. Musisz przenieść lub skopiować plik .proto z projektu gRPC do projektu APS.NET Core na ścieżce, gdzie będzie używany, na przykład Protos/blog.proto.

1. Dodanie metadanych usługi gRPC i Repozytorium:
Następnie, zdefiniuj zależności w swoim projekcie, informując, że klienci z .proto powinni być generowani. Otwarty plik projektu (twoja_nazwa_projektu.csproj) i zaktualizuj go o następujący ItemGroup:

```csharp
<ItemGroup>
  <Protobuf Include="Protos\blog.proto" GrpcServices="Client" Link="Protos\blog.proto" />
</ItemGroup>
```



1. Konfiguracja klienta gRPC:

W `Program.cs`, skonfiguruj klienta gRPC:
```
builder.Services.AddGrpcClient<BlogPosts.BlogPostsClient>(o =>
{
    o.Address = new Uri("https://localhost:7099"); // Adres usługi gRPC
});

```

1. Wywołanie usługi gRPC z kontrolerów Web API
```
[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly BlogPosts.BlogPostsClient _client;

    public BlogController(BlogPosts.BlogPostsClient client)
    {
        _client = client;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPost(int id)
    {
        var reply = await _client.GetPostAsync(new GetPostRequest { Id = id });
        return Ok(reply);
    }

    // Implementacja pozostałych metod CRUD analogicznie
}
```