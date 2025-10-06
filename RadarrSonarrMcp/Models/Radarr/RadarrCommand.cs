using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a command in Radarr.
/// </summary>
public record RadarrCommand
{
  /// <summary>
  /// Command ID.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// Name of the command.
  /// </summary>
  [JsonPropertyName("name")]
  public string? Name { get; init; }

  /// <summary>
  /// Command name.
  /// </summary>
  [JsonPropertyName("commandName")]
  public string? CommandName { get; init; }

  /// <summary>
  /// Command message/description.
  /// </summary>
  [JsonPropertyName("message")]
  public string? Message { get; init; }

  /// <summary>
  /// Current status of the command (queued, started, completed, failed).
  /// </summary>
  [JsonPropertyName("status")]
  public string Status { get; init; } = string.Empty;

  /// <summary>
  /// When the command was queued.
  /// </summary>
  [JsonPropertyName("queued")]
  public DateTime Queued { get; init; }

  /// <summary>
  /// When the command started.
  /// </summary>
  [JsonPropertyName("started")]
  public DateTime? Started { get; init; }

  /// <summary>
  /// When the command ended.
  /// </summary>
  [JsonPropertyName("ended")]
  public DateTime? Ended { get; init; }

  /// <summary>
  /// Trigger type (manual, scheduled, etc.).
  /// </summary>
  [JsonPropertyName("trigger")]
  public string? Trigger { get; init; }
}

/// <summary>
/// Request to execute a command in Radarr.
/// </summary>
public record RadarrCommandRequest
{
  /// <summary>
  /// Name of the command to execute.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; init; } = string.Empty;

  /// <summary>
  /// Movie ID (for movie-specific commands).
  /// </summary>
  [JsonPropertyName("movieId")]
  public int? MovieId { get; init; }

  /// <summary>
  /// Movie IDs (for batch commands).
  /// </summary>
  [JsonPropertyName("movieIds")]
  public List<int>? MovieIds { get; init; }
}

/// <summary>
/// Radarr system status information.
/// </summary>
public record RadarrSystemStatus
{
  /// <summary>
  /// Radarr version.
  /// </summary>
  [JsonPropertyName("version")]
  public string? Version { get; init; }

  /// <summary>
  /// Build time.
  /// </summary>
  [JsonPropertyName("buildTime")]
  public string? BuildTime { get; init; }

  /// <summary>
  /// Whether this is a debug build.
  /// </summary>
  [JsonPropertyName("isDebug")]
  public bool IsDebug { get; init; }

  /// <summary>
  /// Whether this is a production build.
  /// </summary>
  [JsonPropertyName("isProduction")]
  public bool IsProduction { get; init; }

  /// <summary>
  /// Whether the user is an admin.
  /// </summary>
  [JsonPropertyName("isAdmin")]
  public bool IsAdmin { get; init; }

  /// <summary>
  /// Whether the instance is user interactive.
  /// </summary>
  [JsonPropertyName("isUserInteractive")]
  public bool IsUserInteractive { get; init; }

  /// <summary>
  /// Startup path.
  /// </summary>
  [JsonPropertyName("startupPath")]
  public string? StartupPath { get; init; }

  /// <summary>
  /// Application data path.
  /// </summary>
  [JsonPropertyName("appData")]
  public string? AppData { get; init; }

  /// <summary>
  /// Operating system name.
  /// </summary>
  [JsonPropertyName("osName")]
  public string? OsName { get; init; }

  /// <summary>
  /// Operating system version.
  /// </summary>
  [JsonPropertyName("osVersion")]
  public string? OsVersion { get; init; }

  /// <summary>
  /// Whether running on .NET Core.
  /// </summary>
  [JsonPropertyName("isNetCore")]
  public bool IsNetCore { get; init; }

  /// <summary>
  /// Whether running on Linux.
  /// </summary>
  [JsonPropertyName("isLinux")]
  public bool IsLinux { get; init; }

  /// <summary>
  /// Whether running on macOS.
  /// </summary>
  [JsonPropertyName("isOsx")]
  public bool IsOsx { get; init; }

  /// <summary>
  /// Whether running on Windows.
  /// </summary>
  [JsonPropertyName("isWindows")]
  public bool IsWindows { get; init; }

  /// <summary>
  /// Whether running in a Docker container.
  /// </summary>
  [JsonPropertyName("isDocker")]
  public bool IsDocker { get; init; }

  /// <summary>
  /// Mode (standalone, service).
  /// </summary>
  [JsonPropertyName("mode")]
  public string? Mode { get; init; }

  /// <summary>
  /// Git branch.
  /// </summary>
  [JsonPropertyName("branch")]
  public string? Branch { get; init; }

  /// <summary>
  /// Authentication method.
  /// </summary>
  [JsonPropertyName("authentication")]
  public string? Authentication { get; init; }

  /// <summary>
  /// SQLite version.
  /// </summary>
  [JsonPropertyName("sqliteVersion")]
  public string? SqliteVersion { get; init; }

  /// <summary>
  /// URL base path.
  /// </summary>
  [JsonPropertyName("urlBase")]
  public string? UrlBase { get; init; }

  /// <summary>
  /// Runtime version (.NET).
  /// </summary>
  [JsonPropertyName("runtimeVersion")]
  public string? RuntimeVersion { get; init; }

  /// <summary>
  /// Runtime name.
  /// </summary>
  [JsonPropertyName("runtimeName")]
  public string? RuntimeName { get; init; }

  /// <summary>
  /// Start time of Radarr.
  /// </summary>
  [JsonPropertyName("startTime")]
  public DateTime? StartTime { get; init; }
}

/// <summary>
/// Request to add a movie to Radarr.
/// </summary>
public record AddMovieRequest
{
  /// <summary>
  /// Title of the movie.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; init; } = string.Empty;

  /// <summary>
  /// TMDB ID of the movie.
  /// </summary>
  [JsonPropertyName("tmdbId")]
  public int TmdbId { get; init; }

  /// <summary>
  /// Quality profile ID to use.
  /// </summary>
  [JsonPropertyName("qualityProfileId")]
  public int QualityProfileId { get; init; }

  /// <summary>
  /// Root folder path where the movie should be stored.
  /// </summary>
  [JsonPropertyName("rootFolderPath")]
  public string RootFolderPath { get; init; } = string.Empty;

  /// <summary>
  /// Title slug for URLs.
  /// </summary>
  [JsonPropertyName("titleSlug")]
  public string TitleSlug { get; init; } = string.Empty;

  /// <summary>
  /// Release year.
  /// </summary>
  [JsonPropertyName("year")]
  public int Year { get; init; }

  /// <summary>
  /// Whether to monitor the movie.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; init; } = true;

  /// <summary>
  /// Minimum availability (announced, inCinemas, released).
  /// </summary>
  [JsonPropertyName("minimumAvailability")]
  public string MinimumAvailability { get; init; } = "announced";

  /// <summary>
  /// Whether to search for the movie after adding.
  /// </summary>
  [JsonPropertyName("searchForMovie")]
  public bool SearchForMovie { get; init; } = false;

  /// <summary>
  /// List of tag IDs.
  /// </summary>
  [JsonPropertyName("tags")]
  public List<int>? Tags { get; init; }
}
