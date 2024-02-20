## Zadanie 4 z wykładowcą

1. W `RegisterController` w metodzie `GetById` wewnątrz `if (user == null)` ustaw poziom logowania na `Warning`.
```
_logger.LogWarning($"Nie znaleziono użytkownika o id {id}");
```
1. Wywołaj tą metodę kilka razy z parametrem id, któy jeszcze nie istnieje w bazie danych.
1. Dodaj do interface `ILogs` definicję nowej metody `Task<IEnumerable<SeriLog>> GetByLogLevel(string logLevel);`.
3. W `SerilogRepository` dodaj metodę, która będzie filtrować i zwracać logi po poziomie informacji.
```
public async Task<IEnumerable<SeriLog>> GetByLogLevel(string logLevel) 
{
    var sql = @"
        SELECT *
        FROM SeriLogs
        WHERE Level = @Level";
    var results = await _context.CreateConnection().QueryAsync<SeriLog>(sql, new { Level = logLevel });
    return results;
}
```
1. W `SeriLogController` dodaj akcję `GetByLogLevel`.
```
[HttpGet("get-by-logLevel")]
public async Task<IActionResult> GetByLogLevel(string logLevel)
{
    var logs = await _repository.GetByLogLevel(logLevel);
    return Ok(logs);
}
```
1. Przetestuj działanie akcji `GetByLogLevel` dla poziomów logów `Information`, `Warning`, `Error`.




