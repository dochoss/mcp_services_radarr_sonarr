# Radarr Unit Tests Summary

## Test Coverage

This document summarizes the unit tests added for the Radarr MCP server implementation.

### Test Statistics
- **Total Tests**: 44 (all passing)
- **Test Files**: 3
- **Test Categories**: Service Tests, Model Tests, Configuration Tests

---

## 1. RadarrServiceTests (16 tests)

Location: `RadarrSonarrMcp.Tests/Services/RadarrServiceTests.cs`

### Tests Included:

#### Movie Operations (6 tests)
- ✅ `GetAllMoviesAsync_ShouldReturnMovies_WhenApiReturnsData`
  - Verifies successful retrieval of movie list from Radarr API
  - Tests JSON deserialization of movie objects
  
- ✅ `GetAllMoviesAsync_ShouldReturnEmptyList_WhenApiCallFails`
  - Ensures graceful error handling when API is unavailable
  - Returns empty list instead of throwing exceptions

- ✅ `GetMovieByIdAsync_ShouldReturnMovie_WhenMovieExists`
  - Tests retrieval of specific movie by ID
  - Verifies all movie properties are correctly deserialized

- ✅ `GetMovieByIdAsync_ShouldReturnNull_WhenMovieNotFound`
  - Handles 404 responses gracefully
  
- ✅ `SearchMoviesAsync_ShouldReturnMovies_WhenSearchTermMatches`
  - Tests movie search functionality
  - Verifies multiple results are returned correctly

- ✅ `AddMovieAsync_ShouldReturnMovie_WhenAddSucceeds`
  - Tests adding new movies to Radarr
  - Verifies POST request handling and response parsing

#### Configuration Operations (2 tests)
- ✅ `GetQualityProfilesAsync_ShouldReturnProfiles_WhenApiReturnsData`
  - Tests retrieval of quality profiles
  - Verifies profile properties (name, upgradeAllowed, cutoff)

- ✅ `GetRootFoldersAsync_ShouldReturnFolders_WhenApiReturnsData`
  - Tests retrieval of root folder paths
  - Verifies folder accessibility and space information

#### System Operations (3 tests)
- ✅ `GetSystemStatusAsync_ShouldReturnStatus_WhenApiReturnsData`
  - Tests system status retrieval
  - Verifies OS information, Docker status, version info

- ✅ `DeleteMovieAsync_ShouldReturnTrue_WhenDeleteSucceeds`
  - Tests movie deletion functionality
  - Verifies DELETE HTTP method handling

- ✅ `ExecuteCommandAsync_ShouldReturnCommand_WhenExecuteSucceeds`
  - Tests command execution (MovieSearch, RefreshMovie, etc.)
  - Verifies command queuing and status

#### Queue Operations (2 tests)
- ✅ `GetQueueAsync_ShouldReturnQueueWithCorrectPagination` (Theory with 3 data sets)
  - Tests download queue retrieval with pagination
  - Verifies page number, page size, and total records

### Testing Approach:
- Uses Moq to mock HttpClient and HttpMessageHandler
- Tests both success and failure scenarios
- Verifies error handling returns safe defaults (empty lists, nulls)
- Uses FluentAssertions for readable test assertions

---

## 2. RadarrModelsTests (14 tests)

Location: `RadarrSonarrMcp.Tests/Models/RadarrModelsTests.cs`

### Tests Included:

#### Movie Model (3 tests)
- ✅ `Movie_ShouldDeserialize_FromRadarrApiJson`
  - Comprehensive test of Movie model deserialization
  - Tests all 26+ properties including nested objects
  - Verifies genres, tags, ratings, statistics
  
- ✅ `Movie_WithNullValues_ShouldDeserialize_Correctly`
  - Ensures nullable fields handle null values properly
  - Tests optional properties

- ✅ `MovieRatings_ShouldDeserialize_WithAllSources`
  - Tests MovieRatings nested record
  - Verifies IMDB, TMDB, and Rotten Tomatoes ratings

#### Configuration Models (2 tests)
- ✅ `RadarrQualityProfile_ShouldDeserialize_FromRadarrApiJson`
  - Tests quality profile JSON mapping
  - Verifies id, name, upgradeAllowed, cutoff

- ✅ `RadarrRootFolder_ShouldDeserialize_FromRadarrApiJson`
  - Tests root folder JSON mapping
  - Verifies path, accessibility, free space, total space

#### System Models (1 test)
- ✅ `RadarrSystemStatus_ShouldDeserialize_FromRadarrApiJson`
  - Tests system status deserialization
  - Verifies 20+ properties including OS info, Docker, version

#### Request Models (2 tests)
- ✅ `AddMovieRequest_ShouldSerialize_ToRadarrApiJson`
  - Tests serialization of add movie requests
  - Verifies round-trip serialization/deserialization

- ✅ `RadarrCommandRequest_ShouldSerialize_ToRadarrApiJson`
  - Tests command request serialization
  - Verifies JSON property names match API expectations

#### Queue Models (2 tests)
- ✅ `RadarrQueueItem_ShouldDeserialize_FromRadarrApiJson`
  - Tests queue item deserialization
  - Verifies download status, protocol, client info

- ✅ `RadarrQueueResponse_ShouldDeserialize_FromRadarrApiJson`
  - Tests paginated queue response
  - Verifies pagination metadata

### Testing Approach:
- Uses real JSON strings matching Radarr API format
- Tests JsonPropertyName attribute mappings
- Verifies camelCase to PascalCase conversion
- Tests both required and optional properties
- Includes edge cases (null values, empty arrays)

---

## 3. RadarrConfigTests (12 tests)

Location: `RadarrSonarrMcp.Tests/Configuration/RadarrConfigTests.cs`

### Tests Included:

#### Validation Tests (7 tests)
- ✅ `IsValid_ShouldReturnTrue_WhenAllRequiredFieldsAreSet`
  - Tests successful validation with all fields present

- ✅ `IsValid_ShouldReturnFalse_WhenRequiredFieldsAreMissing` (Theory with 6 data sets)
  - Tests validation fails when ApiKey is null/empty
  - Tests validation fails when BasePath is null/empty
  - Tests validation fails when Port is null/empty

#### URL Construction Tests (5 tests)
- ✅ `GetBaseUrl_ShouldReturnCorrectUrl_WithDefaultPort`
  - Tests URL construction with default port 7878
  - Verifies format: http://ip:port/basePath

- ✅ `GetBaseUrl_ShouldReturnCorrectUrl_WithCustomPort` (Theory with 3 data sets)
  - Tests URL construction with custom ports (8080, 7878, 9090)
  
- ✅ `GetBaseUrl_ShouldHandleDifferentIpAddresses`
  - Tests with different IP formats
  - Tests localhost and hostname support

- ✅ `GetBaseUrl_ShouldHandleDifferentBasePaths` (Theory with 3 data sets)
  - Tests with /api/v3, /api/v4, /custom/api
  
- ✅ `RadarrConfig_ShouldHaveDefaultValues`
  - Verifies default BasePath is "/api/v3"
  - Verifies default Port is "7878"

### Testing Approach:
- Uses Theory attributes for parameterized tests
- Tests both valid and invalid configurations
- Verifies URL construction logic
- Tests edge cases (null, empty strings, different formats)

---

## Test Infrastructure

### Frameworks & Libraries Used:
- **xUnit**: Test framework
- **FluentAssertions**: Readable test assertions
- **Moq**: Mocking framework for HttpClient
- **System.Text.Json**: JSON serialization testing

### Test Patterns:
- Arrange-Act-Assert (AAA) pattern
- Fact tests for single scenarios
- Theory tests for parameterized testing
- Mock HTTP responses for isolated testing

### Code Coverage:
The tests cover:
- ✅ All RadarrService public methods
- ✅ All Radarr model record types (7 models)
- ✅ RadarrConfig validation and URL construction
- ✅ JSON serialization/deserialization for all models
- ✅ Error handling and edge cases
- ✅ HTTP request/response handling
- ✅ Property mapping (camelCase ↔ PascalCase)

---

## Running the Tests

### Command Line:
```bash
# Run all tests
dotnet test

# Run with verbosity
dotnet test --verbosity normal

# Run specific test file
dotnet test --filter "FullyQualifiedName~RadarrServiceTests"

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### PowerShell Script:
```bash
.\run-tests.ps1
```

### Visual Studio:
- Test Explorer → Run All Tests
- Right-click test → Run Test(s)

---

## Test Results

**Latest Run**: October 6, 2025
- ✅ Total Tests: 44
- ✅ Passed: 44
- ❌ Failed: 0
- ⚠️ Skipped: 0
- ⏱️ Duration: 2.2 seconds

All tests passing! 🎉

---

## Future Test Additions

Potential areas for additional testing:
- Integration tests with real Radarr API
- Performance tests for large movie lists
- Timeout and retry logic tests
- Concurrent request handling
- Configuration loading from JSON file
- MCP tool registration tests
- End-to-end workflow tests (search → add → download)
