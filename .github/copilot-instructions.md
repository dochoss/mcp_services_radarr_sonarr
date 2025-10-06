# GitHub Copilot Instructions for Radarr/Sonarr MCP Server

## Project Overview

This is a C#-based Model Context Protocol (MCP) server that provides AI assistants with access to Radarr (movies) and Sonarr (TV series) data. The project uses the ModelContextProtocol SDK for implementing the standardized MCP protocol.

## Tech Stack

- **.NET**: 8.0
- **Language**: C# 12
- **Framework**: ModelContextProtocol SDK (0.4.0-preview.1)
- **HTTP Client**: HttpClient
- **JSON**: System.Text.Json
- **Testing**: xUnit, FluentAssertions, Moq

## Code Style and Standards

### C# Style
- Follow .NET coding conventions and C# style guidelines
- Use PascalCase for class names, method names, and properties
- Use camelCase for local variables and parameters
- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Use async/await for I/O operations
- Keep methods focused and single-purpose
- Use descriptive names (e.g., `movieTitle` not `mt`)
- Use `var` when the type is obvious from the right side
- Place opening braces on new lines (Allman style)

### File Organization
```
RadarrSonarrMcp/
├── Configuration/          # Configuration models
│   ├── AppSettings.cs
│   ├── RadarrConfig.cs
│   └── SonarrConfig.cs
├── Models/                 # Data models
│   ├── Movie.cs
│   ├── Series.cs
│   └── Episode.cs
├── Services/               # API service classes
│   ├── IRadarrService.cs
│   ├── RadarrService.cs
│   ├── ISonarrService.cs
│   └── SonarrService.cs
└── Program.cs              # Main entry point

RadarrSonarrMcp.Tests/
├── Configuration/          # Configuration tests
├── Models/                 # Model tests
├── Services/               # Service tests
└── UnitTest1.cs           # Example tests
```

### Configuration
- Configuration is loaded from `config.json` or environment variables
- Environment variables take precedence over config file
- Use strongly-typed configuration classes with properties
- Always validate configuration before using
- Configuration schema:
  ```json
  {
    "NasConfig": {"Ip": "...", "Port": "..."},
    "RadarrConfig": {"ApiKey": "...", "BasePath": "/api/v3", "Port": "7878"},
    "SonarrConfig": {"ApiKey": "...", "BasePath": "/api/v3", "Port": "8989"},
    "PlexConfig": {"BaseUrl": "...", "Token": "..."},
    "ServerConfig": {"Port": 3000}
  }
  ```
- Use PascalCase for JSON keys to match C# property names
- Create configuration model classes with proper types (not dictionaries)

## API Integration Guidelines

### Radarr/Sonarr API
- Always use `/api/v3` as the base path
- Include `apikey` parameter in all requests
- Use 30-second timeout for API requests
- Handle connection errors gracefully with logging
- Return empty lists/default values on API failures
- Use `HttpClient` for all HTTP requests

### Service Classes
- Each service (Radarr, Sonarr, Jellyfin, Plex) should have its own class
- Define an interface for each service (e.g., `IRadarrService`)
- Services should accept a config object in the constructor
- Use records or classes for API response models (e.g., `Movie`, `Series`, `Episode`)
- Use `System.Text.Json` for JSON serialization/deserialization
- Make methods async (return `Task<T>`)

### Example Service Pattern
```csharp
using System.Text.Json;

public record Movie(
    int Id,
    string Title,
    int Year,
    bool HasFile
);

public interface IRadarrService
{
    Task<List<Movie>> GetAllMoviesAsync();
}

public class RadarrService : IRadarrService
{
    private readonly HttpClient _httpClient;
    private readonly RadarrConfig _config;
    
    public RadarrService(HttpClient httpClient, RadarrConfig config)
    {
        _httpClient = httpClient;
        _config = config;
    }
    
    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        try
        {
            var url = $"{_config.BaseUrl}/movie?apikey={_config.ApiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Movie>>(response) ?? new();
        }
        catch (Exception ex)
        {
            // Log error
            return new List<Movie>();
        }
    }
}
```

## MCP Server Tools

When adding new MCP tools, follow this pattern:

### Tool Definition
- Use the ModelContextProtocol SDK's tool registration methods
- Provide clear XML documentation comments
- Return serializable data structures
- Handle errors and return meaningful error messages
- Use async methods for I/O operations

### Example Tool
```csharp
/// <summary>
/// Get a list of available movies with optional filters.
/// </summary>
/// <param name="year">Filter by release year</param>
/// <param name="watched">Filter by watched status (requires Jellyfin/Plex)</param>
/// <returns>List of movies with title, year, and status</returns>
public async Task<List<MovieDto>> GetAvailableMoviesAsync(
    int? year = null,
    bool? watched = null)
{
    var movies = await _radarrService.GetAllMoviesAsync();
    
    if (year.HasValue)
    {
        movies = movies.Where(m => m.Year == year.Value).ToList();
    }
    
    if (watched.HasValue)
    {
        // Filter by watched status from Jellyfin/Plex
    }
    
    return movies.Select(m => new MovieDto(m.Title, m.Year, m.HasFile)).ToList();
}
```

## Error Handling

- Use try-catch blocks for all external API calls
- Log errors using `ILogger` or `Console.WriteLine` for simple cases
- Return user-friendly error messages
- Never expose API keys or sensitive data in error messages
- Default to returning empty collections rather than throwing exceptions
- Use specific exception types when catching (avoid catching generic `Exception` unless necessary)

## Testing

- Write tests for all service methods in `RadarrSonarrMcp.Tests/` directory
- Use xUnit as the test framework
- Use FluentAssertions for readable assertions
- Mock external API calls using Moq
- Test both success and failure scenarios
- Include edge cases (empty responses, malformed data)
- Run tests with: `.\run-tests.ps1` or `dotnet test`

### Test File Pattern
```csharp
using Xunit;
using Moq;
using FluentAssertions;

namespace RadarrSonarrMcp.Tests.Services;

public class RadarrServiceTests
{
    [Fact]
    public async Task GetAllMovies_ShouldReturnEmptyList_WhenNoMoviesExist()
    {
        // Arrange
        var mockHttpClient = new Mock<HttpClient>();
        var config = new RadarrConfig 
        { 
            BaseUrl = "http://localhost:7878/api/v3",
            ApiKey = "test_key"
        };
        var service = new RadarrService(mockHttpClient.Object, config);
        
        // Act
        var result = await service.GetAllMoviesAsync();
        
        // Assert
        result.Should().BeEmpty();
    }
}
```

## Logging

- Use `ILogger<T>` for dependency injection or `Console.WriteLine` for simple scenarios
- Log levels:
  - `LogTrace`: Detailed debugging information
  - `LogDebug`: Debug information
  - `LogInformation`: General informational messages (default)
  - `LogWarning`: Warning messages for recoverable issues
  - `LogError`: Error messages for failures
- Include context in log messages (e.g., which API, what endpoint)
- Use structured logging with parameters: `logger.LogInformation("Fetching movies from {Url}", url)`

## Security

- Never commit API keys or tokens to version control
- Use environment variables or `config.json` (gitignored) for secrets
- Validate and sanitize all user inputs
- Use HTTPS URLs when available
- Set reasonable timeouts on all HTTP requests (30 seconds)

## Dependencies

When adding new dependencies:
1. Add via `dotnet add package <PackageName>`
2. Document the purpose of the new dependency in comments or README
3. Choose mature, well-maintained NuGet packages
4. Prefer packages that are actively maintained and well-documented
5. Check compatibility with .NET 8.0

## Documentation

- Keep `README.md` up to date with new features
- Document all public methods and classes with XML documentation comments
- Use `///` for XML doc comments with `<summary>`, `<param>`, `<returns>` tags
- Include usage examples for complex features
- Update `config.example.json` if adding new config options

## Git Workflow

- Branch: `development` is the default branch
- Write clear, descriptive commit messages
- Keep commits focused on single changes
- Test before committing

## Common Patterns

### Making API Requests
```csharp
public async Task<List<T>> MakeRequestAsync<T>(string endpoint, Dictionary<string, string>? parameters = null)
{
    var url = $"{_config.BaseUrl}/{endpoint}";
    var queryString = $"?apikey={_config.ApiKey}";
    
    if (parameters != null)
    {
        foreach (var param in parameters)
        {
            queryString += $"&{param.Key}={param.Value}";
        }
    }
    
    try
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var response = await _httpClient.GetStringAsync(url + queryString, cts.Token);
        return JsonSerializer.Deserialize<List<T>>(response) ?? new List<T>();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error making request to {Url}", url);
        return new List<T>();
    }
}
```

### Filtering Data
```csharp
public static List<T> FilterItems<T>(List<T> items, Func<T, bool>? predicate = null)
{
    return predicate != null ? items.Where(predicate).ToList() : items;
}

// Usage:
var filteredMovies = FilterItems(movies, m => m.Year == 2023 && m.HasFile);
```

## When Suggesting Code

1. **Prefer existing patterns**: Look at existing service implementations before creating new patterns
2. **Type safety**: Always use proper types (no `dynamic` or `object` unless necessary)
3. **Error handling**: Include try-catch blocks for external calls
4. **Logging**: Add appropriate log statements with structured logging
5. **Configuration**: Use strongly-typed config classes
6. **Testing**: Suggest corresponding test cases with xUnit/FluentAssertions/Moq
7. **Documentation**: Include XML doc comments with examples
8. **Async/await**: Use async methods for I/O operations

## Project-Specific Conventions

- Use `HasFile` (PascalCase) for C# properties to match .NET conventions
- Port numbers: Radarr default is 7878, Sonarr default is 8989, Jellyfin default is 8096, Plex default is 32400
- API paths: Always use `/api/v3` for Radarr/Sonarr
- Config classes: Use `Config` suffix (e.g., `RadarrConfig`, `SonarrConfig`)
- Service classes: Use `Service` suffix (e.g., `RadarrService`, `SonarrService`)
- Interfaces: Prefix with `I` (e.g., `IRadarrService`, `ISonarrService`)
- Async methods: Suffix with `Async` (e.g., `GetMoviesAsync`, `FetchDataAsync`)
