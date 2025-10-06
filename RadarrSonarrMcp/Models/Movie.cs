using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a movie in Radarr.
/// </summary>
public record Movie
{
  /// <summary>
  /// Unique identifier for the movie.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// Title of the movie.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; init; } = string.Empty;

  /// <summary>
  /// Original title of the movie.
  /// </summary>
  [JsonPropertyName("originalTitle")]
  public string? OriginalTitle { get; init; }

  /// <summary>
  /// Release year.
  /// </summary>
  [JsonPropertyName("year")]
  public int Year { get; init; }

  /// <summary>
  /// File path where the movie is stored.
  /// </summary>
  [JsonPropertyName("path")]
  public string? Path { get; init; }

  /// <summary>
  /// Quality profile ID.
  /// </summary>
  [JsonPropertyName("qualityProfileId")]
  public int QualityProfileId { get; init; }

  /// <summary>
  /// Whether the movie is monitored.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; init; }

  /// <summary>
  /// Overview/description of the movie.
  /// </summary>
  [JsonPropertyName("overview")]
  public string? Overview { get; init; }

  /// <summary>
  /// Runtime in minutes.
  /// </summary>
  [JsonPropertyName("runtime")]
  public int Runtime { get; init; }

  /// <summary>
  /// IMDB ID.
  /// </summary>
  [JsonPropertyName("imdbId")]
  public string? ImdbId { get; init; }

  /// <summary>
  /// TMDB ID.
  /// </summary>
  [JsonPropertyName("tmdbId")]
  public int TmdbId { get; init; }

  /// <summary>
  /// Title slug for URLs.
  /// </summary>
  [JsonPropertyName("titleSlug")]
  public string TitleSlug { get; init; } = string.Empty;

  /// <summary>
  /// Root folder path.
  /// </summary>
  [JsonPropertyName("rootFolderPath")]
  public string? RootFolderPath { get; init; }

  /// <summary>
  /// Movie certification/rating (e.g., PG-13, R).
  /// </summary>
  [JsonPropertyName("certification")]
  public string? Certification { get; init; }

  /// <summary>
  /// List of genres.
  /// </summary>
  [JsonPropertyName("genres")]
  public List<string>? Genres { get; init; }

  /// <summary>
  /// List of tag IDs.
  /// </summary>
  [JsonPropertyName("tags")]
  public List<int> Tags { get; init; } = new();

  /// <summary>
  /// Date the movie was added to Radarr.
  /// </summary>
  [JsonPropertyName("added")]
  public DateTime Added { get; init; }

  /// <summary>
  /// Current status of the movie.
  /// </summary>
  [JsonPropertyName("status")]
  public string Status { get; init; } = string.Empty;

  /// <summary>
  /// Whether the movie file has been downloaded.
  /// </summary>
  [JsonPropertyName("hasFile")]
  public bool HasFile { get; init; }

  /// <summary>
  /// Movie file ID (if downloaded).
  /// </summary>
  [JsonPropertyName("movieFileId")]
  public int? MovieFileId { get; init; }

  /// <summary>
  /// Size on disk in bytes.
  /// </summary>
  [JsonPropertyName("sizeOnDisk")]
  public long SizeOnDisk { get; init; }

  /// <summary>
  /// Date the movie was in cinemas.
  /// </summary>
  [JsonPropertyName("inCinemas")]
  public DateTime? InCinemas { get; init; }

  /// <summary>
  /// Physical release date.
  /// </summary>
  [JsonPropertyName("physicalRelease")]
  public DateTime? PhysicalRelease { get; init; }

  /// <summary>
  /// Digital release date.
  /// </summary>
  [JsonPropertyName("digitalRelease")]
  public DateTime? DigitalRelease { get; init; }

  /// <summary>
  /// Movie ratings from various sources.
  /// </summary>
  [JsonPropertyName("ratings")]
  public MovieRatings? Ratings { get; init; }

  /// <summary>
  /// Movie statistics.
  /// </summary>
  [JsonPropertyName("statistics")]
  public MovieStatistics? Statistics { get; init; }
}

/// <summary>
/// Ratings from various sources.
/// </summary>
public record MovieRatings
{
  /// <summary>
  /// IMDB rating.
  /// </summary>
  [JsonPropertyName("imdb")]
  public MovieRating? Imdb { get; init; }

  /// <summary>
  /// TMDB rating.
  /// </summary>
  [JsonPropertyName("tmdb")]
  public MovieRating? Tmdb { get; init; }

  /// <summary>
  /// Rotten Tomatoes rating.
  /// </summary>
  [JsonPropertyName("rottenTomatoes")]
  public MovieRating? RottenTomatoes { get; init; }
}

/// <summary>
/// Rating information.
/// </summary>
public record MovieRating
{
  /// <summary>
  /// Number of votes.
  /// </summary>
  [JsonPropertyName("votes")]
  public int Votes { get; init; }

  /// <summary>
  /// Rating value.
  /// </summary>
  [JsonPropertyName("value")]
  public double Value { get; init; }
}

/// <summary>
/// Movie statistics.
/// </summary>
public record MovieStatistics
{
  /// <summary>
  /// Number of movie files.
  /// </summary>
  [JsonPropertyName("movieFileCount")]
  public int MovieFileCount { get; init; }

  /// <summary>
  /// Total size on disk in bytes.
  /// </summary>
  [JsonPropertyName("sizeOnDisk")]
  public long SizeOnDisk { get; init; }

  /// <summary>
  /// List of release groups.
  /// </summary>
  [JsonPropertyName("releaseGroups")]
  public List<string>? ReleaseGroups { get; init; }
}
