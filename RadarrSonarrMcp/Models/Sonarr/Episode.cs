using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents an episode in Sonarr.
/// </summary>
public class Episode
{
  /// <summary>
  /// Unique identifier for the episode.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// ID of the parent series.
  /// </summary>
  [JsonPropertyName("seriesId")]
  public int SeriesId { get; set; }

  /// <summary>
  /// TVDB episode ID.
  /// </summary>
  [JsonPropertyName("tvdbId")]
  public int? TvdbId { get; set; }

  /// <summary>
  /// Episode file ID (0 if no file).
  /// </summary>
  [JsonPropertyName("episodeFileId")]
  public int EpisodeFileId { get; set; }

  /// <summary>
  /// Season number.
  /// </summary>
  [JsonPropertyName("seasonNumber")]
  public int SeasonNumber { get; set; }

  /// <summary>
  /// Episode number within the season.
  /// </summary>
  [JsonPropertyName("episodeNumber")]
  public int EpisodeNumber { get; set; }

  /// <summary>
  /// Title of the episode.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Air date in ISO format.
  /// </summary>
  [JsonPropertyName("airDate")]
  public string? AirDate { get; set; }

  /// <summary>
  /// Air date in UTC.
  /// </summary>
  [JsonPropertyName("airDateUtc")]
  public DateTime? AirDateUtc { get; set; }

  /// <summary>
  /// Overview/description of the episode.
  /// </summary>
  [JsonPropertyName("overview")]
  public string? Overview { get; set; }

  /// <summary>
  /// Whether the episode is monitored.
  /// </summary>
  [JsonPropertyName("monitored")]
  public bool Monitored { get; set; }

  /// <summary>
  /// Absolute episode number (for anime).
  /// </summary>
  [JsonPropertyName("absoluteEpisodeNumber")]
  public int? AbsoluteEpisodeNumber { get; set; }

  /// <summary>
  /// Scene absolute episode number.
  /// </summary>
  [JsonPropertyName("sceneAbsoluteEpisodeNumber")]
  public int? SceneAbsoluteEpisodeNumber { get; set; }

  /// <summary>
  /// Scene season number.
  /// </summary>
  [JsonPropertyName("sceneSeasonNumber")]
  public int? SceneSeasonNumber { get; set; }

  /// <summary>
  /// Scene episode number.
  /// </summary>
  [JsonPropertyName("sceneEpisodeNumber")]
  public int? SceneEpisodeNumber { get; set; }

  /// <summary>
  /// Whether the episode file has been downloaded.
  /// </summary>
  [JsonPropertyName("hasFile")]
  public bool HasFile { get; set; }

  /// <summary>
  /// Information about the episode file.
  /// </summary>
  [JsonPropertyName("episodeFile")]
  public EpisodeFile? EpisodeFile { get; set; }

  /// <summary>
  /// Series information (included when querying episodes).
  /// </summary>
  [JsonPropertyName("series")]
  public Series? Series { get; set; }
}

/// <summary>
/// Represents an episode file.
/// </summary>
public class EpisodeFile
{
  /// <summary>
  /// Unique identifier for the file.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// ID of the parent series.
  /// </summary>
  [JsonPropertyName("seriesId")]
  public int SeriesId { get; set; }

  /// <summary>
  /// Season number.
  /// </summary>
  [JsonPropertyName("seasonNumber")]
  public int SeasonNumber { get; set; }

  /// <summary>
  /// Relative path to the file.
  /// </summary>
  [JsonPropertyName("relativePath")]
  public string RelativePath { get; set; } = string.Empty;

  /// <summary>
  /// Full path to the file.
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// File size in bytes.
  /// </summary>
  [JsonPropertyName("size")]
  public long Size { get; set; }

  /// <summary>
  /// Date the file was added.
  /// </summary>
  [JsonPropertyName("dateAdded")]
  public DateTime DateAdded { get; set; }

  /// <summary>
  /// Quality information.
  /// </summary>
  [JsonPropertyName("quality")]
  public QualityInfo? Quality { get; set; }

  /// <summary>
  /// Media information (codecs, resolution, etc.).
  /// </summary>
  [JsonPropertyName("mediaInfo")]
  public MediaInfo? MediaInfo { get; set; }
}

/// <summary>
/// Quality information for a file.
/// </summary>
public class QualityInfo
{
  /// <summary>
  /// Quality details.
  /// </summary>
  [JsonPropertyName("quality")]
  public QualityDefinition? Quality { get; set; }

  /// <summary>
  /// Revision information.
  /// </summary>
  [JsonPropertyName("revision")]
  public Revision? Revision { get; set; }
}

/// <summary>
/// Quality definition.
/// </summary>
public class QualityDefinition
{
  /// <summary>
  /// Quality ID.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Quality name (e.g., "HDTV-1080p").
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Revision information for quality.
/// </summary>
public class Revision
{
  /// <summary>
  /// Version number.
  /// </summary>
  [JsonPropertyName("version")]
  public int Version { get; set; }

  /// <summary>
  /// Real number.
  /// </summary>
  [JsonPropertyName("real")]
  public int Real { get; set; }

  /// <summary>
  /// Whether this is a proper release.
  /// </summary>
  [JsonPropertyName("isRepack")]
  public bool IsRepack { get; set; }
}

/// <summary>
/// Media information for a file.
/// </summary>
public class MediaInfo
{
  /// <summary>
  /// Video codec.
  /// </summary>
  [JsonPropertyName("videoCodec")]
  public string? VideoCodec { get; set; }

  /// <summary>
  /// Video bitrate.
  /// </summary>
  [JsonPropertyName("videoBitrate")]
  public int? VideoBitrate { get; set; }

  /// <summary>
  /// Video bit depth.
  /// </summary>
  [JsonPropertyName("videoBitDepth")]
  public int? VideoBitDepth { get; set; }

  /// <summary>
  /// Video width in pixels.
  /// </summary>
  [JsonPropertyName("width")]
  public int? Width { get; set; }

  /// <summary>
  /// Video height in pixels.
  /// </summary>
  [JsonPropertyName("height")]
  public int? Height { get; set; }

  /// <summary>
  /// Audio codec.
  /// </summary>
  [JsonPropertyName("audioCodec")]
  public string? AudioCodec { get; set; }

  /// <summary>
  /// Audio bitrate.
  /// </summary>
  [JsonPropertyName("audioBitrate")]
  public int? AudioBitrate { get; set; }

  /// <summary>
  /// Audio channels.
  /// </summary>
  [JsonPropertyName("audioChannels")]
  public double? AudioChannels { get; set; }

  /// <summary>
  /// Runtime in seconds.
  /// </summary>
  [JsonPropertyName("runTime")]
  public string? RunTime { get; set; }

  /// <summary>
  /// Subtitle languages.
  /// </summary>
  [JsonPropertyName("subtitles")]
  public string? Subtitles { get; set; }
}
