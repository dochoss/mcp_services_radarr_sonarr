using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a command that can be executed in Sonarr.
/// </summary>
public class Command
{
  /// <summary>
  /// Command ID (returned after posting a command).
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Name of the command.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Command message/description.
  /// </summary>
  [JsonPropertyName("commandName")]
  public string? CommandName { get; set; }

  /// <summary>
  /// Current status of the command (queued, started, completed, failed).
  /// </summary>
  [JsonPropertyName("status")]
  public string? Status { get; set; }

  /// <summary>
  /// Whether the command is queued.
  /// </summary>
  [JsonPropertyName("queued")]
  public DateTime? Queued { get; set; }

  /// <summary>
  /// When the command started.
  /// </summary>
  [JsonPropertyName("started")]
  public DateTime? Started { get; set; }

  /// <summary>
  /// When the command ended.
  /// </summary>
  [JsonPropertyName("ended")]
  public DateTime? Ended { get; set; }

  /// <summary>
  /// Duration of the command.
  /// </summary>
  [JsonPropertyName("duration")]
  public string? Duration { get; set; }

  /// <summary>
  /// Trigger type (manual, scheduled, etc.).
  /// </summary>
  [JsonPropertyName("trigger")]
  public string? Trigger { get; set; }

  /// <summary>
  /// Status message.
  /// </summary>
  [JsonPropertyName("message")]
  public string? Message { get; set; }

  /// <summary>
  /// Body of the command (used when posting).
  /// </summary>
  [JsonPropertyName("body")]
  public CommandBody? Body { get; set; }
}

/// <summary>
/// Body for command requests.
/// </summary>
public class CommandBody
{
  /// <summary>
  /// Series ID (for series-specific commands).
  /// </summary>
  [JsonPropertyName("seriesId")]
  public int? SeriesId { get; set; }

  /// <summary>
  /// Episode IDs (for episode search commands).
  /// </summary>
  [JsonPropertyName("episodeIds")]
  public List<int>? EpisodeIds { get; set; }
}

/// <summary>
/// Request to execute a command.
/// </summary>
public class CommandRequest
{
  /// <summary>
  /// Name of the command to execute.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Series ID (for series-specific commands).
  /// </summary>
  [JsonPropertyName("seriesId")]
  public int? SeriesId { get; set; }

  /// <summary>
  /// Episode IDs (for episode search commands).
  /// </summary>
  [JsonPropertyName("episodeIds")]
  public List<int>? EpisodeIds { get; set; }
}

/// <summary>
/// System status information.
/// </summary>
public class SystemStatus
{
  /// <summary>
  /// Sonarr version.
  /// </summary>
  [JsonPropertyName("version")]
  public string Version { get; set; } = string.Empty;

  /// <summary>
  /// Build time.
  /// </summary>
  [JsonPropertyName("buildTime")]
  public DateTime BuildTime { get; set; }

  /// <summary>
  /// Whether this is a debug build.
  /// </summary>
  [JsonPropertyName("isDebug")]
  public bool IsDebug { get; set; }

  /// <summary>
  /// Whether this is a production build.
  /// </summary>
  [JsonPropertyName("isProduction")]
  public bool IsProduction { get; set; }

  /// <summary>
  /// Whether authentication is enabled.
  /// </summary>
  [JsonPropertyName("authentication")]
  public string Authentication { get; set; } = string.Empty;

  /// <summary>
  /// Start time of Sonarr.
  /// </summary>
  [JsonPropertyName("startTime")]
  public DateTime StartTime { get; set; }

  /// <summary>
  /// Application URL.
  /// </summary>
  [JsonPropertyName("appData")]
  public string? AppData { get; set; }

  /// <summary>
  /// Operating system name.
  /// </summary>
  [JsonPropertyName("osName")]
  public string? OsName { get; set; }

  /// <summary>
  /// Operating system version.
  /// </summary>
  [JsonPropertyName("osVersion")]
  public string? OsVersion { get; set; }

  /// <summary>
  /// Whether the instance is running in a Docker container.
  /// </summary>
  [JsonPropertyName("isDocker")]
  public bool IsDocker { get; set; }

  /// <summary>
  /// Runtime version (.NET).
  /// </summary>
  [JsonPropertyName("runtimeVersion")]
  public string? RuntimeVersion { get; set; }

  /// <summary>
  /// Runtime name.
  /// </summary>
  [JsonPropertyName("runtimeName")]
  public string? RuntimeName { get; set; }
}

/// <summary>
/// Request to add a new series to Sonarr.
/// </summary>
public class AddSeriesRequest
{
  /// <summary>
  /// Title of the series.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// TVDB ID of the series.
  /// </summary>
  [JsonPropertyName("tvdbId")]
  public int TvdbId { get; set; }

  /// <summary>
  /// Quality profile ID to use.
  /// </summary>
  [JsonPropertyName("qualityProfileId")]
  public int QualityProfileId { get; set; }

  /// <summary>
  /// Path where the series should be stored.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// Root folder path.
  /// </summary>
  [JsonPropertyName("rootFolderPath")]
  public string? RootFolderPath { get; set; }

  /// <summary>
  /// Whether to monitor the series.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; set; } = true;

  /// <summary>
  /// Whether to use season folders.
  /// </summary>
  [JsonPropertyName("seasonFolder")]
  public bool SeasonFolder { get; set; } = true;

  /// <summary>
  /// Type of series (standard, daily, anime).
  /// </summary>
  [JsonPropertyName("seriesType")]
  public string SeriesType { get; set; } = "standard";

  /// <summary>
  /// List of seasons with monitoring status.
  /// </summary>
  [JsonPropertyName("seasons")]
  public List<Season>? Seasons { get; set; }

  /// <summary>
  /// Whether to search for missing episodes after adding.
  /// </summary>
  [JsonPropertyName("addOptions")]
  public AddSeriesOptions? AddOptions { get; set; }
}

/// <summary>
/// Options for adding a series.
/// </summary>
public class AddSeriesOptions
{
  /// <summary>
  /// Whether to search for missing episodes.
  /// </summary>
  [JsonPropertyName("searchForMissingEpisodes")]
  public bool SearchForMissingEpisodes { get; set; } = false;
}
