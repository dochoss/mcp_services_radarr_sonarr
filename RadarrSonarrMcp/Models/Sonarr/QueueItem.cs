using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents an item in the download queue.
/// </summary>
public class QueueItem
{
  /// <summary>
  /// Unique identifier for the queue item.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// ID of the series being downloaded.
  /// </summary>
  [JsonPropertyName("seriesId")]
  public int SeriesId { get; set; }

  /// <summary>
  /// ID of the episode being downloaded.
  /// </summary>
  [JsonPropertyName("episodeId")]
  public int EpisodeId { get; set; }

  /// <summary>
  /// Series information.
  /// </summary>
  [JsonPropertyName("series")]
  public Series? Series { get; set; }

  /// <summary>
  /// Episode information.
  /// </summary>
  [JsonPropertyName("episode")]
  public Episode? Episode { get; set; }

  /// <summary>
  /// Quality information.
  /// </summary>
  [JsonPropertyName("quality")]
  public QualityInfo? Quality { get; set; }

  /// <summary>
  /// Total size in bytes.
  /// </summary>
  [JsonPropertyName("size")]
  public long Size { get; set; }

  /// <summary>
  /// Title of the download.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Amount downloaded in bytes.
  /// </summary>
  [JsonPropertyName("sizeleft")]
  public long SizeLeft { get; set; }

  /// <summary>
  /// Time remaining as a string.
  /// </summary>
  [JsonPropertyName("timeleft")]
  public string? TimeLeft { get; set; }

  /// <summary>
  /// Estimated completion time.
  /// </summary>
  [JsonPropertyName("estimatedCompletionTime")]
  public DateTime? EstimatedCompletionTime { get; set; }

  /// <summary>
  /// Current status (downloading, paused, etc.).
  /// </summary>
  [JsonPropertyName("status")]
  public string Status { get; set; } = string.Empty;

  /// <summary>
  /// Tracking ID in the download client.
  /// </summary>
  [JsonPropertyName("trackedDownloadStatus")]
  public string? TrackedDownloadStatus { get; set; }

  /// <summary>
  /// Status messages.
  /// </summary>
  [JsonPropertyName("statusMessages")]
  public List<StatusMessage> StatusMessages { get; set; } = new();

  /// <summary>
  /// Download ID from the download client.
  /// </summary>
  [JsonPropertyName("downloadId")]
  public string? DownloadId { get; set; }

  /// <summary>
  /// Protocol used (usenet, torrent).
  /// </summary>
  [JsonPropertyName("protocol")]
  public string Protocol { get; set; } = string.Empty;

  /// <summary>
  /// Download client that's handling this download.
  /// </summary>
  [JsonPropertyName("downloadClient")]
  public string? DownloadClient { get; set; }

  /// <summary>
  /// Indexer that provided this download.
  /// </summary>
  [JsonPropertyName("indexer")]
  public string? Indexer { get; set; }

  /// <summary>
  /// Output path where the file will be saved.
  /// </summary>
  [JsonPropertyName("outputPath")]
  public string? OutputPath { get; set; }
}

/// <summary>
/// Status message for a queue item.
/// </summary>
public class StatusMessage
{
  /// <summary>
  /// Title of the message.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// List of messages.
  /// </summary>
  [JsonPropertyName("messages")]
  public List<string> Messages { get; set; } = new();
}

/// <summary>
/// Response wrapper for queue endpoint (includes pagination).
/// </summary>
public class QueueResponse
{
  /// <summary>
  /// Current page number.
  /// </summary>
  [JsonPropertyName("page")]
  public int Page { get; set; }

  /// <summary>
  /// Page size.
  /// </summary>
  [JsonPropertyName("pageSize")]
  public int PageSize { get; set; }

  /// <summary>
  /// Sort key.
  /// </summary>
  [JsonPropertyName("sortKey")]
  public string? SortKey { get; set; }

  /// <summary>
  /// Sort direction (ascending, descending).
  /// </summary>
  [JsonPropertyName("sortDirection")]
  public string? SortDirection { get; set; }

  /// <summary>
  /// Total number of records.
  /// </summary>
  [JsonPropertyName("totalRecords")]
  public int TotalRecords { get; set; }

  /// <summary>
  /// Queue items.
  /// </summary>
  [JsonPropertyName("records")]
  public List<QueueItem> Records { get; set; } = new();
}
