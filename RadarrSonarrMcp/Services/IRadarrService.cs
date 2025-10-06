using RadarrSonarrMcp.Models;

namespace RadarrSonarrMcp.Services;

/// <summary>
/// Interface for Radarr API service operations
/// </summary>
public interface IRadarrService
{
  /// <summary>
  /// Get all movies from Radarr
  /// </summary>
  Task<List<Movie>> GetAllMoviesAsync();

  /// <summary>
  /// Get a specific movie by ID
  /// </summary>
  Task<Movie?> GetMovieByIdAsync(int movieId);

  /// <summary>
  /// Add a new movie to Radarr
  /// </summary>
  Task<Movie?> AddMovieAsync(AddMovieRequest request);

  /// <summary>
  /// Update an existing movie
  /// </summary>
  Task<Movie?> UpdateMovieAsync(int movieId, Movie movie);

  /// <summary>
  /// Delete a movie from Radarr
  /// </summary>
  Task<bool> DeleteMovieAsync(int movieId, bool deleteFiles = false, bool addImportExclusion = false);

  /// <summary>
  /// Search for movies by title
  /// </summary>
  Task<List<Movie>> SearchMoviesAsync(string term);

  /// <summary>
  /// Get the current download queue
  /// </summary>
  Task<RadarrQueueResponse> GetQueueAsync(int page = 1, int pageSize = 20);

  /// <summary>
  /// Get movies that are missing (monitored but not downloaded)
  /// </summary>
  Task<List<Movie>> GetMissingMoviesAsync();

  /// <summary>
  /// Execute a command (e.g., MovieSearch, RefreshMovie, RescanMovie)
  /// </summary>
  Task<RadarrCommand?> ExecuteCommandAsync(RadarrCommandRequest command);

  /// <summary>
  /// Get system status information
  /// </summary>
  Task<RadarrSystemStatus?> GetSystemStatusAsync();

  /// <summary>
  /// Get all configured root folders
  /// </summary>
  Task<List<RadarrRootFolder>> GetRootFoldersAsync();

  /// <summary>
  /// Get all quality profiles
  /// </summary>
  Task<List<RadarrQualityProfile>> GetQualityProfilesAsync();
}
