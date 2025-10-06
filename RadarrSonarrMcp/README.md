# Radarr & Sonarr MCP Server (C#)

A C# implementation of a Model Context Protocol (MCP) server for Radarr and Sonarr media management systems.

## Overview

This MCP server provides AI assistants with access to your Radarr (movies) and Sonarr (TV series) data through a standardized protocol.

## Prerequisites

- .NET 8.0 SDK or higher
- Radarr instance with API access
- Sonarr instance with API access

## Project Structure

```
RadarrSonarrMcp/
├── Configuration/     # Configuration models and loading
├── Models/           # Data models for movies, series, episodes
├── Services/         # API service classes for Radarr/Sonarr
└── Program.cs        # Main entry point
```

## Getting Started

### Build the Project

```powershell
dotnet build
```

### Run the Server

```powershell
dotnet run
```

## Configuration

Configuration can be provided via:
- `appsettings.json` file
- Environment variables
- `config.json` file (create from `config.example.json`)

### Example Configuration

```json
{
  "NasConfig": {
    "Ip": "10.0.0.23",
    "Port": "7878"
  },
  "RadarrConfig": {
    "ApiKey": "your_radarr_api_key",
    "BasePath": "/api/v3",
    "Port": "7878"
  },
  "SonarrConfig": {
    "ApiKey": "your_sonarr_api_key",
    "BasePath": "/api/v3",
    "Port": "8989"
  },
  "ServerConfig": {
    "Port": 3000
  }
}
```

## Development

This project uses:
- **ModelContextProtocol SDK**: For MCP server implementation
- **System.Text.Json**: For JSON serialization
- **HttpClient**: For API communication with Radarr/Sonarr

## License

MIT
