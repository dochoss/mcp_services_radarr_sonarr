namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Root configuration model for the application.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Server configuration for base URL construction.
    /// </summary>
    public ServicesServerConfig ServicesServerConfig { get; set; } = new();

    /// <summary>
    /// Radarr API configuration.
    /// </summary>
    public RadarrConfig RadarrConfig { get; set; } = new();

    /// <summary>
    /// Sonarr API configuration.
    /// </summary>
    public SonarrConfig SonarrConfig { get; set; } = new();

    /// <summary>
    /// Plex API configuration.
    /// </summary>
    public PlexConfig? PlexConfig { get; set; }

    /// <summary>
    /// MCP Server configuration.
    /// </summary>
    public McpServerConfig McpServerConfig { get; set; } = new();
}

/// <summary>
/// Server network configuration where services are running (Radarr, Sonarr, etc.).
/// </summary>
public class ServicesServerConfig
{
    /// <summary>
    /// IP address or hostname of the server (defaults to localhost for local installations).
    /// </summary>
    public string Ip { get; set; } = "localhost";

    /// <summary>
    /// Default port (not used for specific services).
    /// </summary>
    public string Port { get; set; } = "80";
}

/// <summary>
/// MCP Server configuration.
/// </summary>
public class McpServerConfig
{
    /// <summary>
    /// Port for the MCP server to listen on.
    /// </summary>
    public int Port { get; set; } = 3000;
}

/// <summary>
/// Plex media server configuration.
/// </summary>
public class PlexConfig
{
    /// <summary>
    /// Base URL for Plex API.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Authentication token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
