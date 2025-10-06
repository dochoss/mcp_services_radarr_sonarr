namespace RadarrSonarrMcp.Configuration;

/// <summary>
/// Configuration for Sonarr API access.
/// </summary>
public class SonarrConfig : IServiceConfig
{
  /// <inheritdoc />
  public string ApiKey { get; set; } = string.Empty;

  /// <inheritdoc />
  public string BasePath { get; set; } = "/api/v3";

  /// <inheritdoc />
  public string Port { get; set; } = "8989";

  /// <inheritdoc />
  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(ApiKey) &&
           !string.IsNullOrWhiteSpace(BasePath) &&
           !string.IsNullOrWhiteSpace(Port);
  }

  /// <inheritdoc />
  public string GetBaseUrl(string nasIp)
  {
    return $"http://{nasIp}:{Port}{BasePath}";
  }
}
