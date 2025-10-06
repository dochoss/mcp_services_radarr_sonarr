namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Configuration for Sonarr API access.
/// </summary>
public class SonarrConfig
{
    /// <summary>
    /// API key for authentication with Sonarr.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Base API path (e.g., "/api/v3").
    /// </summary>
    public string BasePath { get; set; } = "/api/v3";

    /// <summary>
    /// Port where Sonarr is running (default: 8989).
    /// </summary>
    public string Port { get; set; } = "8989";

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
    /// Constructs the base URL for Sonarr API using NAS IP.
    /// </summary>
    /// <param name="nasIp">IP address of the NAS.</param>
    /// <returns>Full base URL for Sonarr API.</returns>
    public string GetBaseUrl(string nasIp)
    {
        return $"http://{nasIp}:{Port}{BasePath}";
    }
}
