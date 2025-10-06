using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a queued download in Radarr.
/// </summary>
public record RadarrQueueItem
{
  /// <summary>
  /// Queue item ID.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// ID of the movie being downloaded.
  /// </summary>
  [JsonPropertyName("movieId")]
  public int MovieId { get; init; }

  /// <summary>
  /// Title of the download.
  /// </summary>
  [JsonPropertyName("title")]
  public string Title { get; init; } = string.Empty;

  /// <summary>
  /// Total size in bytes.
  /// </summary>
  [JsonPropertyName("size")]
  public double Size { get; init; }

  /// <summary>
  /// Remaining size in bytes.
  /// </summary>
  [JsonPropertyName("sizeleft")]
  public double SizeLeft { get; init; }

  /// <summary>
  /// Download status.
  /// </summary>
  [JsonPropertyName("status")]
  public string Status { get; init; } = string.Empty;

  /// <summary>
  /// Tracked download status.
  /// </summary>
  [JsonPropertyName("trackedDownloadStatus")]
  public string? TrackedDownloadStatus { get; init; }

  /// <summary>
  /// Tracked download state.
  /// </summary>
  [JsonPropertyName("trackedDownloadState")]
  public string? TrackedDownloadState { get; init; }

  /// <summary>
  /// Estimated time remaining.
  /// </summary>
  [JsonPropertyName("timeleft")]
  public TimeSpan? TimeLeft { get; init; }

  /// <summary>
  /// Estimated completion time.
  /// </summary>
  [JsonPropertyName("estimatedCompletionTime")]
  public DateTime? EstimatedCompletionTime { get; init; }

  /// <summary>
  /// Download protocol (usenet, torrent).
  /// </summary>
  [JsonPropertyName("protocol")]
  public string? Protocol { get; init; }

  /// <summary>
  /// Download client name.
  /// </summary>
  [JsonPropertyName("downloadClient")]
  public string? DownloadClient { get; init; }

  /// <summary>
  /// Download ID.
  /// </summary>
  [JsonPropertyName("downloadId")]
  public string? DownloadId { get; init; }

  /// <summary>
  /// Indexer name.
  /// </summary>
  [JsonPropertyName("indexer")]
  public string? Indexer { get; init; }

  /// <summary>
  /// Output path for the download.
  /// </summary>
  [JsonPropertyName("outputPath")]
  public string? OutputPath { get; init; }

  /// <summary>
  /// Status messages.
  /// </summary>
  [JsonPropertyName("statusMessages")]
  public List<StatusMessage>? StatusMessages { get; init; }
}

/// <summary>
/// Paginated response for queue items.
/// </summary>
public record RadarrQueueResponse
{
  /// <summary>
  /// Current page number.
  /// </summary>
  [JsonPropertyName("page")]
  public int Page { get; init; }

  /// <summary>
  /// Page size.
  /// </summary>
  [JsonPropertyName("pageSize")]
  public int PageSize { get; init; }

  /// <summary>
  /// Sort key.
  /// </summary>
  [JsonPropertyName("sortKey")]
  public string? SortKey { get; init; }

  /// <summary>
  /// Sort direction (ascending, descending).
  /// </summary>
  [JsonPropertyName("sortDirection")]
  public string? SortDirection { get; init; }

  /// <summary>
  /// Total number of records.
  /// </summary>
  [JsonPropertyName("totalRecords")]
  public int TotalRecords { get; init; }

  /// <summary>
  /// List of queue items.
  /// </summary>
  [JsonPropertyName("records")]
  public List<RadarrQueueItem> Records { get; init; } = new();
}
