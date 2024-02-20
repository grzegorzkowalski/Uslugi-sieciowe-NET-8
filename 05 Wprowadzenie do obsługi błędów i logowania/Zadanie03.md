## Zadanie 3 z wykładowcą

1. Dodaj nowy model:
```
    public class SeriLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
    }
```
1. Dodaj w folderze `Repositories` interface `ILogs`.
```
    public interface ILogs
    {
        Task<IEnumerable<LogCountByDayDto>> GetLogCountByDay();
    }
```
1. Dodaj repozytorium SerilogLogRepository implementujący `IRepository<SerilogLog>` oraz `ILogs`.
```
using Dapper;
using Flashcards.Dal;
using Flashcards.Models;

namespace Flashcards.Repositories
{
    public class SerilogRepository : IRepository<SeriLog>, ILogs
    {
        private DapperContext _context;

        public SerilogRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeriLog>> GetAllAsync()
        {
            var sql = "SELECT * FROM SeriLogs";
            return await _context.CreateConnection().QueryAsync<SeriLog>(sql);
        }

        public async Task<SeriLog> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM SeriLogs WHERE Id = @Id";
            return (await _context.CreateConnection().QueryAsync<SeriLog>(sql, new { Id = id })).FirstOrDefault();
        }

        // Dla uproszczenia poniższe metody mogą nie być zaimplementowane dla logów
        public Task<int> AddAsync(SeriLog entity) => throw new System.NotImplementedException();
        public Task<bool> DeleteAsync(int id) => throw new System.NotImplementedException();
        public Task<bool> UpdateAsync(SeriLog entity) => throw new System.NotImplementedException();
    }
}

```
1. Zarejestruj odpowiednie wstrzyknięcie `IRepository<Serilog>`.
```
builder.Services.AddScoped<SerilogRepository>();
```
1. Dodaj nowy kontroler `SerilogController`.
```
using Flashcards.Models;
using Flashcards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriLogController : ControllerBase
    {
        private IRepository<SeriLog> _repository;

        public SeriLogController(IRepository<SeriLog> logRepository)
        {
            _repository = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _repository.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
        }
    }
}
```
1. Dodaj model typu DTO `LogCountByDayDto`.
```
public class LogCountByDay
{
    public DateTime Day { get; set; }
    public int Count { get; set; }
}

```
1. Dodaj w repozytorium metodę, która będzie agregować ile logów każdego dnia pojawia się w tabeli `SeriLogs`.
```
public async Task<IEnumerable<LogCountByDayDto>> GetLogCountByDay()
{
    var sql = @"
    SELECT 
        CAST(TimeStamp AS DATE) AS Day, 
        COUNT(*) AS Count
    FROM SeriLogs
    GROUP BY CAST(TimeStamp AS DATE)
    ORDER BY Day";
    var results = await _context.CreateConnection().QueryAsync<LogCountByDayDto>(sql);
    return results;
}
```
1. Dodaj odpowiednią akcję kontrolera.
```
[HttpGet("count-by-day")]
public async Task<IActionResult> GetLogCountByDay()
{
    var logs = await _repository.GetLogCountByDay();
    return Ok(logs);
}
```
1. Przetestuj nową metodę za pomocą Swaggera.



