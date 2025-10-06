# Sonarr MCP Server - Implementation Complete! 🎉

## Overview

A C# Model Context Protocol (MCP) server that provides AI assistants with access to Sonarr (TV series management) APIs. Built with the official ModelContextProtocol SDK for .NET.

## Features Implemented

### Configuration
- ✅ Strongly-typed configuration models (`AppSettings`, `SonarrConfig`, `RadarrConfig`)
- ✅ JSON-based configuration loading from `config.json`
- ✅ Validation of required configuration fields

### Sonarr Data Models
- ✅ `Series` - Complete TV series information with seasons and statistics
- ✅ `Episode` - Episode details with file information
- ✅ `QueueItem` - Download queue items with progress tracking
- ✅ `RootFolder` - Storage locations for TV series
- ✅ `QualityProfile` - Quality settings and profiles
- ✅ `Command` - Command execution and status
- ✅ `SystemStatus` - Sonarr system information

### Sonarr Service (`ISonarrService`)
Implements all 10 recommended Sonarr API endpoints:

1. ✅ `GetAllSeriesAsync()` - Get all TV series
2. ✅ `GetSeriesByIdAsync(id)` - Get specific series details
3. ✅ `AddSeriesAsync(request)` - Add new series to Sonarr
4. ✅ `UpdateSeriesAsync(series)` - Update existing series
5. ✅ `GetEpisodesAsync(seriesId?)` - Get episodes (optionally filtered)
6. ✅ `GetMissingEpisodesAsync(page, pageSize)` - Get wanted/missing episodes
7. ✅ `ExecuteCommandAsync(request)` - Execute commands (search, refresh, rescan)
8. ✅ `GetQueueAsync(page, pageSize)` - Get download queue
9. ✅ `GetSystemStatusAsync()` - Get Sonarr system status
10. ✅ `GetRootFoldersAsync()` - Get storage locations
11. ✅ `GetQualityProfilesAsync()` - Get quality profiles

### MCP Tools (`SonarrTools`)
All 11 MCP tools registered with proper attribute-based declaration:

1. **sonarr_get_all_series** - Get all TV series with statistics
2. **sonarr_get_series** - Get detailed info for a specific series
3. **sonarr_add_series** - Add a new TV series to Sonarr
4. **sonarr_update_series** - Update series monitoring/quality settings
5. **sonarr_get_episodes** - Get episodes for a series
6. **sonarr_get_missing_episodes** - Get wanted but missing episodes  
7. **sonarr_execute_command** - Execute Sonarr commands
8. **sonarr_get_queue** - Get current download queue
9. **sonarr_get_system_status** - Get Sonarr version and status
10. **sonarr_get_root_folders** - Get configured storage locations
11. **sonarr_get_quality_profiles** - Get available quality profiles

## Architecture

```
RadarrSonarrMcp/
├── Configuration/
│   ├── AppSettings.cs         # Root configuration model
│   ├── SonarrConfig.cs        # Sonarr API configuration
│   └── RadarrConfig.cs        # Radarr API configuration
├── Models/
│   ├── Series.cs              # Series, Season, and Statistics models
│   ├── Episode.cs             # Episode and EpisodeFile models
│   ├── QueueItem.cs           # Queue and download tracking models
│   ├── RootFolder.cs          # Storage location models
│   ├── QualityProfile.cs      # Quality settings models
│   └── Command.cs             # Command and SystemStatus models
├── Services/
│   ├── ISonarrService.cs      # Service interface
│   └── SonarrService.cs       # HTTP-based Sonarr API client
├── Program.cs                 # Main MCP server setup with DI
├── SonarrTools.cs             # MCP tool definitions
└── config.json                # Runtime configuration (gitignored)
```

## Technology Stack

- **.NET 8.0** - Target framework
- **C# 12** - Language version
- **ModelContextProtocol SDK** (v0.4.0-preview.1) - MCP server implementation
- **Microsoft.Extensions.Hosting** - Dependency injection and hosting
- **Microsoft.Extensions.Http** - HttpClient factory
- **System.Text.Json** - JSON serialization

## Setup & Configuration

### 1. Create config.json

Copy `config.example.json` to `config.json` and update with your settings:

```json
{
  "NasConfig": {
    "Ip": "10.0.0.23"
  },
  "SonarrConfig": {
    "ApiKey": "your_sonarr_api_key_here",
    "BasePath": "/api/v3",
    "Port": "8989"
  },
  "ServerConfig": {
    "Port": 3000
  }
}
```

### 2. Build the Project

```powershell
dotnet build
```

### 3. Run the MCP Server

```powershell
cd RadarrSonarrMcp
dotnet run
```

The server will:
- Load configuration from `config.json`
- Validate Sonarr API settings
- Register all 11 Sonarr tools
- Start the MCP server on stdio transport
- Log all messages to stderr (MCP requirement)

## Usage with AI Assistants

### Claude Desktop Configuration

Add to your Claude Desktop MCP settings:

```json
{
  "mcpServers": {
    "sonarr": {
      "command": "dotnet",
      "args": ["run", "--project", "C:\\path\\to\\RadarrSonarrMcp\\RadarrSonarrMcp.csproj"]
    }
  }
}
```

### Example Queries

- "What TV shows am I currently tracking in Sonarr?"
- "Show me missing episodes for Breaking Bad"
- "Add The Office to Sonarr with quality profile HD-1080p"
- "What's currently downloading?"
- "Search for missing episodes of Game of Thrones"
- "What's the status of my Sonarr server?"

## Key Design Decisions

### 1. Attribute-Based Tool Registration
Tools use `[McpServerTool]` and `[McpServerToolType]` attributes for automatic discovery, making it easy to add new tools without manual registration.

### 2. Dependency Injection
The service layer uses DI for testability and clean architecture. Tools receive `ISonarrService` via constructor injection.

### 3. Error Handling
- Services return empty collections instead of throwing exceptions
- Tools return error objects for user-friendly messages
- All API calls have 30-second timeouts
- Comprehensive logging to stderr

### 4. Async/Await Throughout
All I/O operations are async for better performance and scalability.

### 5. Strongly-Typed Models
All API responses are deserialized into proper C# models with nullable annotations enabled.

## Testing

```powershell
# Run all tests
dotnet test

# Or use the test runner script
.\run-tests.ps1
```

## Next Steps

### Radarr Integration
The foundation is ready for Radarr (movies):
1. Create `RadarrService` implementing `IRadarrService`
2. Create `RadarrTools` class with movie management tools
3. Register Radarr tools in `Program.cs`

### Additional Services
- SABnzbd integration for download client management
- Jellyfin/Plex integration for watch status
- Calendar integration for upcoming episodes

### Testing
- Add unit tests for `SonarrService`
- Add integration tests with mock Sonarr API
- Add tests for MCP tool functions

## Dependencies

- ModelContextProtocol (0.4.0-preview.1)
- Microsoft.Extensions.Hosting (9.0.9)
- Microsoft.Extensions.Http (9.0.9)
- System.Text.Json (9.0.9)

## Contributing

Follow the conventions in `.github/copilot-instructions.md` for:
- Code style (PascalCase, Allman braces, async methods)
- Error handling patterns
- Configuration management
- Service implementation patterns

## License

MIT License - See LICENSE file for details.
