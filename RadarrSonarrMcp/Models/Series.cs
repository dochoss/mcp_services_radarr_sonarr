using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a TV series in Sonarr.
/// </summary>
public class Series
{
  /// <summary>
  /// Unique identifier for the series.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Title of the TV series.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Sorting title (used for alphabetical sorting).
  /// </summary>
  [JsonPropertyName("sortTitle")]
  public string? SortTitle { get; set; }

  /// <summary>
  /// Current status of the series (continuing, ended, etc.).
  /// </summary>
  [JsonPropertyName("status")]
  public string Status { get; set; } = string.Empty;

  /// <summary>
  /// Overview/description of the series.
  /// </summary>
  [JsonPropertyName("overview")]
  public string? Overview { get; set; }

  /// <summary>
  /// Network that airs the series.
  /// </summary>
  [JsonPropertyName("network")]
  public string? Network { get; set; }

  /// <summary>
  /// Air time for new episodes.
  /// </summary>
  [JsonPropertyName("airTime")]
  public string? AirTime { get; set; }

  /// <summary>
  /// Whether the series is currently monitored.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; set; }

  /// <summary>
  /// Quality profile ID.
  /// </summary>
  [JsonPropertyName("qualityProfileId")]
  public int QualityProfileId { get; set; }

  /// <summary>
  /// Type of series (standard, daily, anime).
  /// </summary>
  [JsonPropertyName("seriesType")]
  public string SeriesType { get; set; } = "standard";

  /// <summary>
  /// Whether to use season folders.
  /// </summary>
  [JsonPropertyName("seasonFolder")]
  public bool SeasonFolder { get; set; }

  /// <summary>
  /// List of seasons for this series.
  /// </summary>
  [JsonPropertyName("seasons")]
  public List<Season> Seasons { get; set; } = new();

  /// <summary>
  /// Path where series files are stored.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// Root folder path.
  /// </summary>
  [JsonPropertyName("rootFolderPath")]
  public string? RootFolderPath { get; set; }

  /// <summary>
  /// Year the series first aired.
  /// </summary>
  [JsonPropertyName("year")]
  public int Year { get; set; }

  /// <summary>
  /// Genres of the series.
  /// </summary>
  [JsonPropertyName("genres")]
  public List<string> Genres { get; set; } = new();

  /// <summary>
  /// TVDB ID.
  /// </summary>
  [JsonPropertyName("tvdbId")]
  public int TvdbId { get; set; }

  /// <summary>
  /// IMDb ID.
  /// </summary>
  [JsonPropertyName("imdbId")]
  public string? ImdbId { get; set; }

  /// <summary>
  /// TV Rage ID.
  /// </summary>
  [JsonPropertyName("tvRageId")]
  public int? TvRageId { get; set; }

  /// <summary>
  /// TV Maze ID.
  /// </summary>
  [JsonPropertyName("tvMazeId")]
  public int? TvMazeId { get; set; }

  /// <summary>
  /// Runtime in minutes.
  /// </summary>
  [JsonPropertyName("runtime")]
  public int Runtime { get; set; }

  /// <summary>
  /// Content rating (TV-MA, TV-14, etc.).
  /// </summary>
  [JsonPropertyName("certification")]
  public string? Certification { get; set; }

  /// <summary>
  /// Statistics about the series (episode counts, size, etc.).
  /// </summary>
  [JsonPropertyName("statistics")]
  public SeriesStatistics? Statistics { get; set; }
}

/// <summary>
/// Represents a season within a series.
/// </summary>
public class Season
{
  /// <summary>
  /// Season number.
  /// </summary>
  [JsonPropertyName("seasonNumber")]
  public int SeasonNumber { get; set; }

  /// <summary>
  /// Whether this season is monitored.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; set; }

  /// <summary>
  /// Statistics for this season.
  /// </summary>
  [JsonPropertyName("statistics")]
  public SeasonStatistics? Statistics { get; set; }
}

/// <summary>
/// Statistics for a season.
/// </summary>
public class SeasonStatistics
{
  /// <summary>
  /// Total number of episodes in the season.
  /// </summary>
  [JsonPropertyName("episodeCount")]
  public int EpisodeCount { get; set; }

  /// <summary>
  /// Number of episodes with files.
  /// </summary>
  [JsonPropertyName("episodeFileCount")]
  public int EpisodeFileCount { get; set; }

  /// <summary>
  /// Total size of files in bytes.
  /// </summary>
  [JsonPropertyName("sizeOnDisk")]
  public long SizeOnDisk { get; set; }

  /// <summary>
  /// Percentage of episodes downloaded (0-100).
  /// </summary>
  [JsonPropertyName("percentOfEpisodes")]
  public double PercentOfEpisodes { get; set; }
}

/// <summary>
/// Statistics for an entire series.
/// </summary>
public class SeriesStatistics
{
  /// <summary>
  /// Number of seasons.
  /// </summary>
  [JsonPropertyName("seasonCount")]
  public int SeasonCount { get; set; }

  /// <summary>
  /// Total episode count.
  /// </summary>
  [JsonPropertyName("episodeCount")]
  public int EpisodeCount { get; set; }

  /// <summary>
  /// Number of episodes with files.
  /// </summary>
  [JsonPropertyName("episodeFileCount")]
  public int EpisodeFileCount { get; set; }

  /// <summary>
  /// Total size on disk in bytes.
  /// </summary>
  [JsonPropertyName("sizeOnDisk")]
  public long SizeOnDisk { get; set; }

  /// <summary>
  /// Percentage of episodes downloaded (0-100).
  /// </summary>
  [JsonPropertyName("percentOfEpisodes")]
  public double PercentOfEpisodes { get; set; }
}
