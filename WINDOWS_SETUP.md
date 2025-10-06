# Running Radarr/Sonarr MCP Server on Windows 11

This guide explains how to set up and run the Radarr/Sonarr MCP Server on Windows 11 so that **ChatGPT Desktop** or **Claude Desktop** can easily connect to it.

## Understanding MCP Server Architecture

This MCP server uses **stdio transport** (standard input/output), which means:
- Claude Desktop will **launch the executable directly** as a subprocess
- There's no HTTP server to connect to
- The desktop apps communicate via stdin/stdout with the process
- Each desktop app manages its own instance of the server

**Default Configuration**: The server is configured to connect to Radarr and Sonarr running on `localhost` by default, which is ideal for local installations. If your services are running on a different machine, you can change the `Ip` setting in `config.json` to the appropriate server address.

## Quick Start

### 1. Build a Self-Contained Executable

First, publish your application as a self-contained executable:

```powershell
# Navigate to the project directory
cd C:\Users\YourUsername\source\repos\mcp_services_radarr_sonarr

# Run the publish script
.\publish.ps1
```

This creates a standalone executable at:
```
RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\RadarrSonarrMcp.exe
```

The self-contained build includes the .NET runtime, so users don't need .NET installed.

### 2. Ensure config.json is in Place

Make sure your `config.json` is in the publish folder:

```powershell
# Copy the example config and customize it
Copy-Item "RadarrSonarrMcp\config.example.json" "RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\config.json"
```

Or edit the config directly at:
```
RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\config.json
```

**Configuration Notes:**
- The default `Ip` is set to `localhost` for local installations
- Update the API keys for Radarr and Sonarr (find them in Settings → General → Security)
- If Radarr/Sonarr are running on a different machine, change the `Ip` to that server's address
- Default ports are 7878 for Radarr and 8989 for Sonarr

### 3. Configure Desktop Apps

#### For ChatGPT Web (Developer Mode)

**Note:** ChatGPT web developer mode requires an HTTP/SSE server, but the .NET MCP SDK currently only supports stdio transport. ChatGPT web is **not currently supported**.

**Alternative for ChatGPT users:**
- Wait for the MCP SDK to add SSE support
- Use Claude Desktop instead (fully supported with stdio)
- Or help contribute an HTTP/SSE transport layer to this project!

ChatGPT's developer mode requirements:
- Remote MCP server with SSE or streaming HTTP
- Connector configuration via web settings
- OAuth or no authentication

This server currently uses stdio transport (standard input/output), which works perfectly with Claude Desktop but is incompatible with ChatGPT web's requirements.

#### For Claude Desktop

1. Open Claude Desktop Settings
2. Navigate to **Developer** → **Edit Config**
3. This opens `claude_desktop_config.json` in your editor

**Example configuration for `%APPDATA%\Claude\claude_desktop_config.json`:**

```json
{
  "mcpServers": {
    "radarr-sonarr": {
      "command": "<REPO ROOT>\\RadarrSonarrMcp\\bin\\Release\\net8.0\\win-x64\\publish\\RadarrSonarrMcp.exe",
      "args": [],
      "env": {}
    }
  }
}
```

**Important Notes:**
- Use **double backslashes** (`\\`) in JSON paths for Windows
- Use the **full absolute path** to the executable
- The `args` array can remain empty for this server
- The server name (`radarr-sonarr`) can be customized

### 4. Test the Connection

1. Restart ChatGPT Desktop or Claude Desktop
2. Start a new conversation
3. Try asking: "What movies do I have in Radarr?"
4. The assistant should be able to query your Radarr/Sonarr instances

## Alternative: Running from Development Build

If you want to run directly from your development build (useful during development):

### ChatGPT Desktop Config:
```json
{
  "mcpServers": {
    "radarr-sonarr": {
      "command": "<REPO ROOT>\\RadarrSonarrMcp\\bin\\Debug\\net8.0\\RadarrSonarrMcp.exe",
      "args": [],
      "env": {}
    }
  }
}
```

### Claude Desktop Config:
```json
{
  "mcpServers": {
    "radarr-sonarr": {
      "command": "<REPO ROOT>\\RadarrSonarrMcp\\bin\\Debug\\net8.0\\RadarrSonarrMcp.exe",
      "args": [],
      "env": {}
    }
  }
}
```

**Note:** The Debug build requires .NET 8.0 Runtime to be installed on your system.

## Running as a Windows Service (Optional)

If you want the MCP server to run as a background Windows Service (for always-on scenarios), you can use **NSSM (Non-Sucking Service Manager)**:

### Install NSSM

1. Download NSSM from: https://nssm.cc/download
2. Extract to a folder (e.g., `C:\Tools\nssm`)
3. Add to PATH or use full path

### Create the Service

```powershell
# Run as Administrator
nssm install RadarrSonarrMCP "C:\Users\YourUsername\source\repos\mcp_services_radarr_sonarr\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\RadarrSonarrMcp.exe"

# Configure the service
nssm set RadarrSonarrMCP AppDirectory "<REPO ROOT>\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish"
nssm set RadarrSonarrMCP DisplayName "Radarr/Sonarr MCP Server"
nssm set RadarrSonarrMCP Description "MCP Server for Radarr and Sonarr integration"
nssm set RadarrSonarrMCP Start SERVICE_AUTO_START

# Start the service
nssm start RadarrSonarrMCP
```

### Manage the Service

```powershell
# Stop the service
nssm stop RadarrSonarrMCP

# Restart the service
nssm restart RadarrSonarrMCP

# Remove the service
nssm remove RadarrSonarrMCP confirm
```

**Note:** Running as a Windows Service is optional. Most users can simply let ChatGPT/Claude Desktop launch the process directly.

## Troubleshooting

### Desktop App Can't Find the Server

1. **Verify the executable path** is correct and the file exists
2. **Check config.json** is in the same folder as the executable
3. **Use double backslashes** in JSON config files (`\\` not `\`)
4. **Restart the desktop app** after changing configuration

### Server Logs

The MCP server writes logs to stderr. To view logs:

1. **In Claude Desktop**: Check Developer Console (Help → Toggle Developer Tools)
2. **In ChatGPT Desktop**: Check the app logs or console
3. **Manual testing**: Run the executable in PowerShell to see output

```powershell
cd C:\Users\YourUsername\source\repos\mcp_services_radarr_sonarr\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish
.\RadarrSonarrMcp.exe
```

### Configuration Issues

If the server fails to start:

1. Verify `config.json` exists in the executable's directory
2. Verify all API keys are correct
3. Verify Radarr/Sonarr URLs are accessible (use `localhost` if running locally)
4. Check the config matches this format:

```json
{
  "ServicesServerConfig": {
    "Ip": "localhost",
    "Port": "7878"
  },
  "RadarrConfig": {
    "ApiKey": "your_actual_api_key",
    "BasePath": "/api/v3",
    "Port": "7878"
  },
  "SonarrConfig": {
    "ApiKey": "your_actual_api_key",
    "BasePath": "/api/v3",
    "Port": "8989"
  },
  "PlexConfig": {
    "BaseUrl": "http://localhost:32400",
    "Token": "your_plex_token_here"
  },
  "McpServerConfig": {
    "Port": 3000
  }
}
```

### Testing Without Desktop Apps

You can test the MCP server manually using the MCP Inspector:

```powershell
# Install MCP Inspector (Node.js required)
npm install -g @modelcontextprotocol/inspector

# Run the inspector
mcp-inspector C:\Users\YourUsername\source\repos\mcp_services_radarr_sonarr\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\RadarrSonarrMcp.exe
```

## File Locations Reference

| Item | Location |
|------|----------|
| **Source Code** | `<REPO ROOT>\RadarrSonarrMcp\` |
| **Debug Build** | `<REPO ROOT>\RadarrSonarrMcp\bin\Debug\net8.0\RadarrSonarrMcp.exe` |
| **Release Build** | `<REPO ROOT>\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\RadarrSonarrMcp.exe` |
| **Config File** | Same directory as executable + `config.json` |
| **Claude Config** | `%APPDATA%\Claude\claude_desktop_config.json` |

## Available MCP Tools

Once connected, the desktop apps can use these tools:

### Sonarr Tools
- `sonarr_get_all_series` - Get all TV series
- `sonarr_get_series` - Get specific series by ID
- `sonarr_add_series` - Add a new series
- `sonarr_update_series` - Update series settings
- `sonarr_get_episodes` - Get episodes
- `sonarr_get_missing_episodes` - Get missing episodes
- `sonarr_execute_command` - Execute commands (search, refresh, etc.)
- `sonarr_get_queue` - Get download queue
- `sonarr_get_system_status` - Get system status
- `sonarr_get_root_folders` - Get root folders
- `sonarr_get_quality_profiles` - Get quality profiles

### Radarr Tools
- `radarr_get_all_movies` - Get all movies
- `radarr_get_movie_by_id` - Get specific movie by ID
- `radarr_search_movies` - Search for movies
- `radarr_add_movie` - Add a new movie
- `radarr_update_movie` - Update movie settings
- `radarr_delete_movie` - Delete a movie
- `radarr_get_queue` - Get download queue
- `radarr_get_missing_movies` - Get missing monitored movies
- `radarr_execute_command` - Execute commands (search, refresh, etc.)
- `radarr_get_system_status` - Get system status
- `radarr_get_root_folders` - Get root folders
- `radarr_get_quality_profiles` - Get quality profiles

## Next Steps

1. **Publish the executable** using `.\publish.ps1`
2. **Copy config.json** to the publish folder
3. **Update your desktop app config** with the executable path
4. **Restart the desktop app**
5. **Test with a query** like "What movies do I have?"

For more information, see:
- [README.md](README.md) - Project overview
- [IMPLEMENTATION.md](RadarrSonarrMcp/IMPLEMENTATION.md) - Implementation details
- [Model Context Protocol Docs](https://modelcontextprotocol.io/)
