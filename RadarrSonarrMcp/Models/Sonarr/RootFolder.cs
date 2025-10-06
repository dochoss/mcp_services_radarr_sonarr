using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a root folder where TV series are stored.
/// </summary>
public class RootFolder
{
  /// <summary>
  /// Unique identifier for the root folder.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Full path to the root folder.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// Whether this root folder is accessible.
  /// </summary>
  [JsonPropertyName("accessible")]
  public bool Accessible { get; set; }

  /// <summary>
  /// Free space in bytes.
  /// </summary>
  [JsonPropertyName("freeSpace")]
  public long FreeSpace { get; set; }

  /// <summary>
  /// Total space in bytes.
  /// </summary>
  [JsonPropertyName("totalSpace")]
  public long TotalSpace { get; set; }

  /// <summary>
  /// Unmapped folders within this root folder.
  /// </summary>
  [JsonPropertyName("unmappedFolders")]
  public List<UnmappedFolder>? UnmappedFolders { get; set; }
}

/// <summary>
/// Represents a folder that exists on disk but isn't mapped to a series.
/// </summary>
public class UnmappedFolder
{
  /// <summary>
  /// Name of the unmapped folder.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Full path to the unmapped folder.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;
}
