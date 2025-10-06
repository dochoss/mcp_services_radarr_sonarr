namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Configuration for Radarr API access.
/// </summary>
public class RadarrConfig
{
    /// <summary>
    /// API key for authentication with Radarr.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Base API path (e.g., "/api/v3").
    /// </summary>
    public string BasePath { get; set; } = "/api/v3";

    /// <summary>
    /// Port where Radarr is running (default: 7878).
    /// </summary>
    public string Port { get; set; } = "7878";

    /// <summary>
    /// Validates that required configuration is present.
    /// </summary>
    /// <returns>True if configuration is valid.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ApiKey) &&
               !string.IsNullOrWhiteSpace(BasePath) &&
               !string.IsNullOrWhiteSpace(Port);
    }

    /// <summary>
    /// Constructs the base URL for Radarr API using NAS IP.
    /// </summary>
    /// <param name="nasIp">IP address of the NAS.</param>
    /// <returns>Full base URL for Radarr API.</returns>
    public string GetBaseUrl(string nasIp)
    {
        return $"http://{nasIp}:{Port}{BasePath}";
    }
}
