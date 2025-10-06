# RadarrSonarrMcp.Tests

Unit tests for the Radarr/Sonarr MCP Server project.

## Test Framework

This project uses:
- **xUnit** - Test framework
- **FluentAssertions** - Expressive assertion library
- **Moq** - Mocking framework for dependencies

## Project Structure

```
RadarrSonarrMcp.Tests/
├── Configuration/    # Tests for configuration classes
├── Models/          # Tests for data models
├── Services/        # Tests for API service classes
└── UnitTest1.cs     # Example tests (can be deleted)
```

## Running Tests

### Run all tests
```powershell
dotnet test
```

### Run tests with detailed output
```powershell
dotnet test --verbosity normal
```

### Run tests with code coverage
```powershell
dotnet test /p:CollectCoverage=true
```

### Run specific test
```powershell
dotnet test --filter "FullyQualifiedName~ExampleTests"
```

### Watch mode (auto-run on file changes)
```powershell
dotnet watch test
```

## Writing Tests

### Basic Test Structure

```csharp
using FluentAssertions;
using Xunit;

namespace RadarrSonarrMcp.Tests.Services;

public class RadarrServiceTests
{
    [Fact]
    public void GetAllMovies_ShouldReturnEmptyList_WhenNoMoviesExist()
    {
        // Arrange
        var service = new RadarrService(config);
        
        // Act
        var result = service.GetAllMovies();
        
        // Assert
        result.Should().BeEmpty();
    }
}
```

### Using Moq for Mocking

```csharp
using Moq;

[Fact]
public void Example_WithMocking()
{
    // Arrange
    var mockHttpClient = new Mock<HttpClient>();
    mockHttpClient
        .Setup(x => x.GetAsync(It.IsAny<string>()))
        .ReturnsAsync(new HttpResponseMessage());
    
    // Act & Assert
    // ... your test code
}
```

### Theory Tests (Parameterized)

```csharp
[Theory]
[InlineData("The Matrix", 1999)]
[InlineData("Inception", 2010)]
public void Example_Theory(string title, int year)
{
    // Test with multiple data sets
}
```

## FluentAssertions Examples

```csharp
// Equality
result.Should().Be(expected);
result.Should().NotBe(unexpected);

// Collections
list.Should().BeEmpty();
list.Should().NotBeEmpty();
list.Should().HaveCount(5);
list.Should().Contain(item);

// Strings
text.Should().StartWith("Hello");
text.Should().EndWith("World");
text.Should().Contain("test");

// Exceptions
Action act = () => method();
act.Should().Throw<InvalidOperationException>();

// Objects
obj.Should().NotBeNull();
obj.Should().BeOfType<MyClass>();
```

## Best Practices

1. **Follow AAA Pattern**: Arrange, Act, Assert
2. **One assertion per test** (when possible)
3. **Use descriptive test names**: `MethodName_Scenario_ExpectedBehavior`
4. **Test both success and failure cases**
5. **Mock external dependencies** (HTTP clients, file system, etc.)
6. **Keep tests isolated** - no shared state between tests
7. **Make tests fast** - avoid unnecessary delays or I/O

## CI/CD Integration

Tests are automatically run in the CI/CD pipeline on every commit.
All tests must pass before code can be merged.
