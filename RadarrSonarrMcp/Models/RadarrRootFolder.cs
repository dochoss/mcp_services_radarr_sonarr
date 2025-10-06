using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a root folder in Radarr.
/// </summary>
public record RadarrRootFolder
{
  /// <summary>
  /// Root folder ID.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// Path to the root folder.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; init; } = string.Empty;

  /// <summary>
  /// Whether the folder is accessible.
  /// </summary>
  [JsonPropertyName("accessible")]
  public bool Accessible { get; init; }

  /// <summary>
  /// Free space in bytes.
  /// </summary>
  [JsonPropertyName("freeSpace")]
  public long? FreeSpace { get; init; }

  /// <summary>
  /// Total space in bytes.
  /// </summary>
  [JsonPropertyName("totalSpace")]
  public long? TotalSpace { get; init; }
}
