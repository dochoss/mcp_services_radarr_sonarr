using RadarrSonarrMcp.Models;

namespace RadarrSonarrMcp.Services;

/// <summary>
/// Interface for Sonarr API operations.
/// </summary>
public interface ISonarrService
{
  /// <summary>
  /// Get all series from Sonarr.
  /// </summary>
  /// <returns>List of all series.</returns>
  Task<List<Series>> GetAllSeriesAsync();

  /// <summary>
  /// Get a specific series by ID.
  /// </summary>
  /// <param name="seriesId">The ID of the series.</param>
  /// <returns>Series details or null if not found.</returns>
  Task<Series?> GetSeriesByIdAsync(int seriesId);

  /// <summary>
  /// Add a new series to Sonarr.
  /// </summary>
  /// <param name="request">The series details to add.</param>
  /// <returns>The added series.</returns>
  Task<Series?> AddSeriesAsync(AddSeriesRequest request);

  /// <summary>
  /// Update an existing series.
  /// </summary>
  /// <param name="series">The series with updated information.</param>
  /// <returns>The updated series.</returns>
  Task<Series?> UpdateSeriesAsync(Series series);

  /// <summary>
  /// Get episodes, optionally filtered by series ID.
  /// </summary>
  /// <param name="seriesId">Optional series ID to filter by.</param>
  /// <returns>List of episodes.</returns>
  Task<List<Episode>> GetEpisodesAsync(int? seriesId = null);

  /// <summary>
  /// Get missing/wanted episodes.
  /// </summary>
  /// <param name="page">Page number (default: 1).</param>
  /// <param name="pageSize">Page size (default: 20).</param>
  /// <param name="sortKey">Sort key (default: airDateUtc).</param>
  /// <param name="sortDirection">Sort direction (default: descending).</param>
  /// <returns>Paginated list of missing episodes.</returns>
  Task<List<Episode>> GetMissingEpisodesAsync(int page = 1, int pageSize = 20, string sortKey = "airDateUtc", string sortDirection = "descending");

  /// <summary>
  /// Execute a command (search, refresh, rescan, etc.).
  /// </summary>
  /// <param name="request">The command to execute.</param>
  /// <returns>Command status.</returns>
  Task<Command?> ExecuteCommandAsync(CommandRequest request);

  /// <summary>
  /// Get the download queue.
  /// </summary>
  /// <param name="page">Page number (default: 1).</param>
  /// <param name="pageSize">Page size (default: 20).</param>
  /// <returns>Queue response with download items.</returns>
  Task<QueueResponse> GetQueueAsync(int page = 1, int pageSize = 20);

  /// <summary>
  /// Get system status.
  /// </summary>
  /// <returns>System status information.</returns>
  Task<SystemStatus?> GetSystemStatusAsync();

  /// <summary>
  /// Get all root folders.
  /// </summary>
  /// <returns>List of root folders.</returns>
  Task<List<RootFolder>> GetRootFoldersAsync();

  /// <summary>
  /// Get all quality profiles.
  /// </summary>
  /// <returns>List of quality profiles.</returns>
  Task<List<QualityProfile>> GetQualityProfilesAsync();
}
