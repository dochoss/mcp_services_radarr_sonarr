# Radarr and Sonarr MCP Server

A C# .NET-based Model Context Protocol (MCP) server that provides AI assistants like Claude and ChatGPT with access to your Radarr (movies) and Sonarr (TV series) data.

## Overview

This MCP server allows AI assistants to query and manage your movie and TV show collection via Radarr and Sonarr APIs. Built with the ModelContextProtocol SDK, it implements the standardized protocol for AI context that Claude Desktop, ChatGPT Desktop, and other MCP-compatible clients can use.

## Features

- **Native MCP Implementation**: Built with ModelContextProtocol SDK for seamless AI integration
- **Radarr Integration**: Full access to your movie collection (list, search, add, update, delete)
- **Sonarr Integration**: Complete TV show and episode management
- **Rich API Access**: Quality profiles, root folders, system status, download queue
- **Claude Desktop Compatible**: Works seamlessly with Claude Desktop via stdio transport
- **Windows-Optimized**: Easy setup on Windows 11 with PowerShell scripts
- **Self-Contained Executable**: No .NET runtime installation required
- **Well-Tested**: Comprehensive xUnit test suite for reliability

## Installation & Setup

### Windows 11 Quick Start

1. **Clone and Build:**
   ```powershell
   git clone https://github.com/yourusername/radarr-sonarr-mcp.git
   cd radarr-sonarr-mcp
   .\publish.ps1
   ```

2. **Configure:**
   - Edit `RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\config.json`
   - Add your Radarr/Sonarr API keys and server details

3. **Connect to Desktop App:**
   - **Claude Desktop**: Settings → Developer → Edit Config
   - **ChatGPT Desktop**: Settings → MCP Servers
   - Add the executable path (see example config files)

4. **Test:**
   - Restart your desktop app
   - Ask: "What movies do I have in Radarr?"

📖 **See [QUICKSTART.md](QUICKSTART.md) for the fastest setup**  
📖 **See [WINDOWS_SETUP.md](WINDOWS_SETUP.md) for detailed instructions**

### Development Build

For development, you can run directly from the Debug build:

```powershell
# Build
dotnet build

# Run (stdio mode - for Claude Desktop)
dotnet run --project RadarrSonarrMcp

# Run (HTTP mode - for web-based clients)
dotnet run --project RadarrSonarrMcp -- --http

# Test
dotnet test
```

## Transport Modes

This MCP server supports two transport modes:

### stdio Mode (Default)
- **Best for**: Claude Desktop, ChatGPT Desktop, and other desktop MCP clients
- **How to use**: Run without any flags (default mode)
- **Command**: `dotnet run` or just execute the built .exe
- **Communication**: Standard input/output streams

### HTTP Mode
- **Best for**: Web-based clients, custom integrations, testing with HTTP tools
- **How to use**: Add `--http` or `--mode=http` flag when running
- **Command**: `dotnet run -- --http`
- **Port**: Configured in `config.json` (default: 3000)
- **Endpoint**: `http://localhost:3000` (or your configured port)

**Example HTTP mode:**
```powershell
# Run in HTTP mode
dotnet run --project RadarrSonarrMcp -- --http

# Or from published executable
.\RadarrSonarrMcp.exe --http
```

The server will display which mode it's running in at startup:
```
Radarr/Sonarr MCP Server
========================

Loaded configuration:
  NAS IP: 10.0.0.23
  Sonarr URL: http://10.0.0.23:8989/api/v3
  Radarr URL: http://10.0.0.23:7878/api/v3
  Transport Mode: HTTP
  HTTP Port: 3000

Starting HTTP MCP server...
Server listening on: http://localhost:3000
```

## Configuration

Edit the `config.json` file in the same directory as the executable:

```json
{
  "NasConfig": {
    "Ip": "10.0.0.23",
    "Port": "7878"
  },
  "RadarrConfig": {
    "ApiKey": "YOUR_RADARR_API_KEY",
    "BasePath": "/api/v3",
    "Port": "7878"
  },
  "SonarrConfig": {
    "ApiKey": "YOUR_SONARR_API_KEY",
    "BasePath": "/api/v3",
    "Port": "8989"
  },
  "PlexConfig": {
    "BaseUrl": "http://10.0.0.23:32400",
    "Token": "YOUR_PLEX_TOKEN"
  },
  "ServerConfig": {
    "Port": 3000
  }
}
```

**Note**: The `ServerConfig.Port` is only used when running in HTTP mode.

### Finding API Keys

**Radarr API Key:**
1. Open Radarr in your browser
2. Go to Settings → General → Security
3. Copy the API Key

**Sonarr API Key:**
1. Open Sonarr in your browser
2. Go to Settings → General → Security
3. Copy the API Key

## Available MCP Tools

### Radarr (Movies)
- `radarr_get_all_movies` - Get all movies in your library
- `radarr_get_movie_by_id` - Get details for a specific movie
- `radarr_search_movies` - Search for movies to add
- `radarr_add_movie` - Add a new movie to Radarr
- `radarr_update_movie` - Update movie settings
- `radarr_delete_movie` - Delete a movie from Radarr
- `radarr_get_queue` - View the download queue
- `radarr_get_missing_movies` - Get monitored movies that are missing
- `radarr_execute_command` - Execute commands (MovieSearch, RefreshMovie, etc.)
- `radarr_get_system_status` - Get Radarr system information
- `radarr_get_root_folders` - Get configured root folders
- `radarr_get_quality_profiles` - Get quality profiles

### Sonarr (TV Shows)
- `sonarr_get_all_series` - Get all TV series in your library
- `sonarr_get_series` - Get details for a specific series
- `sonarr_add_series` - Add a new TV series to Sonarr
- `sonarr_update_series` - Update series settings
- `sonarr_get_episodes` - Get episodes for a series
- `sonarr_get_missing_episodes` - Get monitored episodes that are missing
- `sonarr_execute_command` - Execute commands (SeriesSearch, EpisodeSearch, etc.)
- `sonarr_get_queue` - View the download queue
- `sonarr_get_system_status` - Get Sonarr system information
- `sonarr_get_root_folders` - Get configured root folders
- `sonarr_get_quality_profiles` - Get quality profiles

## Example Queries

Once connected, you can ask ChatGPT or Claude:

- "What movies do I have in Radarr?"
- "Search for The Matrix and add it to Radarr"
- "What TV shows am I tracking in Sonarr?"
- "Show me my missing episodes"
- "Add Breaking Bad to Sonarr"
- "What's in my download queue?"
- "Update Game of Thrones to use the HD quality profile"

## Development

### Building

```powershell
# Build the project
dotnet build

# Run the project
dotnet run --project RadarrSonarrMcp

# Publish a release build
.\publish.ps1
```

### Testing

```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Project Structure

```
RadarrSonarrMcp/
├── Configuration/     # Configuration models
├── Models/           # Data models for Radarr/Sonarr
├── Services/         # API service implementations
├── RadarrTools.cs    # Radarr MCP tool definitions
├── SonarrTools.cs    # Sonarr MCP tool definitions
└── Program.cs        # Main entry point

RadarrSonarrMcp.Tests/
├── Configuration/    # Config tests
├── Models/          # Model tests
└── Services/        # Service tests
```

## Tech Stack

- **.NET 8.0** - Runtime
- **C# 12** - Language
- **ModelContextProtocol SDK** - MCP implementation
- **xUnit** - Testing framework
- **FluentAssertions** - Assertion library
- **Moq** - Mocking framework

## Notes

- The watched/watchlist status functionality assumes these are tracked using specific mechanisms in Radarr/Sonarr. You may need to adapt this to your specific setup.
- For security reasons, it's recommended to run this server only on your local network.
