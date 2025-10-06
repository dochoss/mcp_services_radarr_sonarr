using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a movie file in Radarr.
/// </summary>
public record MovieFile
{
  /// <summary>
  /// Unique identifier for the file.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// ID of the parent movie.
  /// </summary>
  [JsonPropertyName("movieId")]
  public int MovieId { get; init; }

  /// <summary>
  /// Relative path to the file.
  /// </summary>
  [JsonPropertyName("relativePath")]
  public string? RelativePath { get; init; }

  /// <summary>
  /// Full path to the file.
  /// </summary>
  [JsonPropertyName("path")]
  public string? Path { get; init; }

  /// <summary>
  /// File size in bytes.
  /// </summary>
  [JsonPropertyName("size")]
  public long Size { get; init; }

  /// <summary>
  /// Date the file was added.
  /// </summary>
  [JsonPropertyName("dateAdded")]
  public DateTime DateAdded { get; init; }

  /// <summary>
  /// Scene name.
  /// </summary>
  [JsonPropertyName("sceneName")]
  public string? SceneName { get; init; }

  /// <summary>
  /// Release group.
  /// </summary>
  [JsonPropertyName("releaseGroup")]
  public string? ReleaseGroup { get; init; }

  /// <summary>
  /// Movie edition.
  /// </summary>
  [JsonPropertyName("edition")]
  public string? Edition { get; init; }

  /// <summary>
  /// Quality information.
  /// </summary>
  [JsonPropertyName("quality")]
  public QualityInfo? Quality { get; init; }

  /// <summary>
  /// Media information (codecs, resolution, etc.).
  /// </summary>
  [JsonPropertyName("mediaInfo")]
  public MediaInfo? MediaInfo { get; init; }
}
