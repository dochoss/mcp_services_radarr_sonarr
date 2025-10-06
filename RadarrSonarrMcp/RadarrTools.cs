using System.ComponentModel;
using ModelContextProtocol.Server;
using RadarrSonarrMcp.Models;
using RadarrSonarrMcp.Services;

namespace RadarrSonarrMcp;

/// <summary>
/// MCP tools for Radarr operations
/// </summary>
[McpServerToolType]
public static class RadarrTools
{
  /// <summary>
  /// Get all movies from Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_get_all_movies"), Description("Get a list of all movies in Radarr")]
  public static async Task<object> GetAllMovies(IRadarrService radarrService)
  {
    var movies = await radarrService.GetAllMoviesAsync();

    return new
    {
      count = movies.Count,
      movies = movies.Select(m => new
      {
        id = m.Id,
        title = m.Title,
        year = m.Year,
        tmdbId = m.TmdbId,
        imdbId = m.ImdbId,
        hasFile = m.HasFile,
        monitored = m.Monitored,
        status = m.Status,
        path = m.Path,
        qualityProfileId = m.QualityProfileId,
        sizeOnDisk = m.SizeOnDisk,
        runtime = m.Runtime,
        overview = m.Overview,
        inCinemas = m.InCinemas,
        physicalRelease = m.PhysicalRelease
      }).ToList()
    };
  }

  /// <summary>
  /// Get a specific movie by ID from Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_get_movie_by_id"), Description("Get details of a specific movie by ID")]
  public static async Task<object> GetMovieById(IRadarrService radarrService, int movieId)
  {
    var movie = await radarrService.GetMovieByIdAsync(movieId);

    if (movie == null)
    {
      return new { success = false, message = $"Movie with ID {movieId} not found" };
    }

    return new
    {
      success = true,
      movie = new
      {
        id = movie.Id,
        title = movie.Title,
        year = movie.Year,
        tmdbId = movie.TmdbId,
        imdbId = movie.ImdbId,
        hasFile = movie.HasFile,
        monitored = movie.Monitored,
        status = movie.Status,
        path = movie.Path,
        rootFolderPath = movie.RootFolderPath,
        qualityProfileId = movie.QualityProfileId,
        sizeOnDisk = movie.SizeOnDisk,
        runtime = movie.Runtime,
        overview = movie.Overview,
        genres = movie.Genres,
        tags = movie.Tags,
        inCinemas = movie.InCinemas,
        physicalRelease = movie.PhysicalRelease,
        digitalRelease = movie.DigitalRelease,
        certification = movie.Certification
      }
    };
  }

  /// <summary>
  /// Search for movies in Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_search_movies"), Description("Search for movies by title in Radarr/TMDB")]
  public static async Task<object> SearchMovies(IRadarrService radarrService, string term)
  {
    var movies = await radarrService.SearchMoviesAsync(term);

    return new
    {
      count = movies.Count,
      results = movies.Select(m => new
      {
        title = m.Title,
        year = m.Year,
        tmdbId = m.TmdbId,
        imdbId = m.ImdbId,
        titleSlug = m.TitleSlug,
        overview = m.Overview,
        status = m.Status,
        runtime = m.Runtime,
        inCinemas = m.InCinemas,
        physicalRelease = m.PhysicalRelease
      }).ToList()
    };
  }

  /// <summary>
  /// Add a new movie to Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_add_movie"), Description("Add a new movie to Radarr")]
  public static async Task<object> AddMovie(
      IRadarrService radarrService,
      string title,
      int tmdbId,
      int qualityProfileId,
      string rootFolderPath,
      string titleSlug,
      int year,
      bool monitored = true,
      string minimumAvailability = "announced",
      bool searchForMovie = false)
  {
    var request = new AddMovieRequest
    {
        Title = title,
        TmdbId = tmdbId,
        QualityProfileId = qualityProfileId,
        RootFolderPath = rootFolderPath,
        TitleSlug = titleSlug,
        Year = year,
        Monitored = monitored,
        MinimumAvailability = minimumAvailability,
        SearchForMovie = searchForMovie,
        Tags = null
    };

    var movie = await radarrService.AddMovieAsync(request);

    if (movie == null)
    {
      return new { success = false, message = "Failed to add movie" };
    }

    return new
    {
      success = true,
      message = $"Movie '{movie.Title}' added successfully",
      movie = new
      {
        id = movie.Id,
        title = movie.Title,
        year = movie.Year,
        path = movie.Path,
        monitored = movie.Monitored,
        qualityProfileId = movie.QualityProfileId
      }
    };
  }

  /// <summary>
  /// Update an existing movie in Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_update_movie"), Description("Update an existing movie's settings in Radarr")]
  public static async Task<object> UpdateMovie(
      IRadarrService radarrService,
      int movieId,
      bool? monitored = null,
      int? qualityProfileId = null,
      string? path = null)
  {
    var existingMovie = await radarrService.GetMovieByIdAsync(movieId);

    if (existingMovie == null)
    {
      return new { success = false, message = $"Movie with ID {movieId} not found" };
    }

    var updatedMovie = existingMovie with
    {
      Monitored = monitored ?? existingMovie.Monitored,
      QualityProfileId = qualityProfileId ?? existingMovie.QualityProfileId,
      Path = path ?? existingMovie.Path
    };

    var result = await radarrService.UpdateMovieAsync(movieId, updatedMovie);

    if (result == null)
    {
      return new { success = false, message = "Failed to update movie" };
    }

    return new
    {
      success = true,
      message = $"Movie '{result.Title}' updated successfully",
      movie = new
      {
        id = result.Id,
        title = result.Title,
        monitored = result.Monitored,
        qualityProfileId = result.QualityProfileId,
        path = result.Path
      }
    };
  }

  /// <summary>
  /// Delete a movie from Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_delete_movie"), Description("Delete a movie from Radarr")]
  public static async Task<object> DeleteMovie(
      IRadarrService radarrService,
      int movieId,
      bool deleteFiles = false,
      bool addImportExclusion = false)
  {
    var success = await radarrService.DeleteMovieAsync(movieId, deleteFiles, addImportExclusion);

    return new
    {
      success,
      message = success ? $"Movie {movieId} deleted successfully" : $"Failed to delete movie {movieId}"
    };
  }

  /// <summary>
  /// Get the download queue from Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_get_queue"), Description("Get the current download queue in Radarr")]
  public static async Task<object> GetQueue(IRadarrService radarrService, int page = 1, int pageSize = 20)
  {
    var queue = await radarrService.GetQueueAsync(page, pageSize);

    return new
    {
      page = queue.Page,
      pageSize = queue.PageSize,
      totalRecords = queue.TotalRecords,
      records = queue.Records.Select(q => new
      {
        id = q.Id,
        movieId = q.MovieId,
        title = q.Title,
        size = q.Size,
        sizeLeft = q.SizeLeft,
        status = q.Status,
        timeLeft = q.TimeLeft,
        estimatedCompletionTime = q.EstimatedCompletionTime,
        protocol = q.Protocol,
        downloadClient = q.DownloadClient,
        indexer = q.Indexer
      }).ToList()
    };
  }

  /// <summary>
  /// Get movies that are missing (monitored but not downloaded)
  /// </summary>
  [McpServerTool(Name = "radarr_get_missing_movies"), Description("Get a list of monitored movies that are missing (not downloaded)")]
  public static async Task<object> GetMissingMovies(IRadarrService radarrService)
  {
    var movies = await radarrService.GetMissingMoviesAsync();

    return new
    {
      count = movies.Count,
      movies = movies.Select(m => new
      {
        id = m.Id,
        title = m.Title,
        year = m.Year,
        status = m.Status,
        monitored = m.Monitored,
        hasFile = m.HasFile,
        inCinemas = m.InCinemas,
        physicalRelease = m.PhysicalRelease
      }).ToList()
    };
  }

  /// <summary>
  /// Execute a command in Radarr (e.g., MovieSearch, RefreshMovie)
  /// </summary>
  [McpServerTool(Name = "radarr_execute_command"), Description("Execute a command in Radarr (MovieSearch, RefreshMovie, RescanMovie, etc.)")]
  public static async Task<object> ExecuteCommand(
      IRadarrService radarrService,
      string commandName,
      int? movieId = null)
  {
    var command = new RadarrCommandRequest
    {
        Name = commandName,
        MovieId = movieId
    };
    var result = await radarrService.ExecuteCommandAsync(command);

    if (result == null)
    {
      return new { success = false, message = $"Failed to execute command '{commandName}'" };
    }

    return new
    {
      success = true,
      commandId = result.Id,
      commandName = result.CommandName,
      status = result.Status,
      queued = result.Queued,
      message = $"Command '{commandName}' executed successfully"
    };
  }

  /// <summary>
  /// Get Radarr system status
  /// </summary>
  [McpServerTool(Name = "radarr_get_system_status"), Description("Get Radarr system status and version information")]
  public static async Task<object> GetSystemStatus(IRadarrService radarrService)
  {
    var status = await radarrService.GetSystemStatusAsync();

    if (status == null)
    {
      return new { success = false, message = "Failed to fetch system status" };
    }

    return new
    {
      success = true,
      version = status.Version,
      buildTime = status.BuildTime,
      isDocker = status.IsDocker,
      osName = status.OsName,
      osVersion = status.OsVersion,
      branch = status.Branch,
      authentication = status.Authentication,
      startTime = status.StartTime
    };
  }

  /// <summary>
  /// Get all root folders configured in Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_get_root_folders"), Description("Get all root folders configured in Radarr")]
  public static async Task<object> GetRootFolders(IRadarrService radarrService)
  {
    var folders = await radarrService.GetRootFoldersAsync();

    return new
    {
      count = folders.Count,
      folders = folders.Select(f => new
      {
        id = f.Id,
        path = f.Path,
        accessible = f.Accessible,
        freeSpace = f.FreeSpace,
        totalSpace = f.TotalSpace
      }).ToList()
    };
  }

  /// <summary>
  /// Get all quality profiles configured in Radarr
  /// </summary>
  [McpServerTool(Name = "radarr_get_quality_profiles"), Description("Get all quality profiles configured in Radarr")]
  public static async Task<object> GetQualityProfiles(IRadarrService radarrService)
  {
    var profiles = await radarrService.GetQualityProfilesAsync();

    return new
    {
      count = profiles.Count,
      profiles = profiles.Select(p => new
      {
        id = p.Id,
        name = p.Name,
        upgradeAllowed = p.UpgradeAllowed,
        cutoff = p.Cutoff
      }).ToList()
    };
  }
}
