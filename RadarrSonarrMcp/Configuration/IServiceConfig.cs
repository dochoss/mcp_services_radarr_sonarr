namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Interface for service configuration (Radarr, Sonarr, etc.).
/// </summary>
public interface IServiceConfig
{
  /// <summary>
  /// API key for authentication.
  /// </summary>
  string ApiKey { get; set; }

  /// <summary>
  /// Base API path (e.g., "/api/v3").
  /// </summary>
  string BasePath { get; set; }

  /// <summary>
  /// Port where the service is running.
  /// </summary>
  string Port { get; set; }

  /// <summary>
  /// Constructs the base URL for the service API using server IP/hostname.
  /// </summary>
  string GetBaseUrl(string nasIp);

  /// <summary>
  /// Validates that required configuration is present.
  /// </summary>
  bool IsValid();
}