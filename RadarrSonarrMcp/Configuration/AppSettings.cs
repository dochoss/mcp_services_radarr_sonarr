namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Root configuration model for the application.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// NAS configuration for base URL construction.
    /// </summary>
    public NasConfig NasConfig { get; set; } = new();

    /// <summary>
    /// Radarr API configuration.
    /// </summary>
    public RadarrConfig RadarrConfig { get; set; } = new();

    /// <summary>
    /// Sonarr API configuration.
    /// </summary>
    public SonarrConfig SonarrConfig { get; set; } = new();

    /// <summary>
    /// Jellyfin API configuration.
    /// </summary>
    public JellyfinConfig? JellyfinConfig { get; set; }

    /// <summary>
    /// Plex API configuration.
    /// </summary>
    public PlexConfig? PlexConfig { get; set; }

    /// <summary>
    /// MCP Server configuration.
    /// </summary>
    public ServerConfig ServerConfig { get; set; } = new();
}

/// <summary>
/// NAS network configuration.
/// </summary>
public class NasConfig
{
    /// <summary>
    /// IP address of the NAS server.
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
public class ServerConfig
{
    /// <summary>
    /// Port for the MCP server to listen on.
    /// </summary>
    public int Port { get; set; } = 3000;
}

/// <summary>
/// Jellyfin media server configuration.
/// </summary>
public class JellyfinConfig
{
    /// <summary>
    /// Base URL for Jellyfin API.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// API key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// User ID for querying watched status.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
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
