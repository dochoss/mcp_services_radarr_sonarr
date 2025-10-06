using System.ComponentModel;
using ModelContextProtocol.Server;
using RadarrSonarrMcp.Models;
using RadarrSonarrMcp.Services;

namespace RadarrSonarrMcp;

/// <summary>
/// Sonarr MCP tools for managing TV series.
/// </summary>
[McpServerToolType]
public static class SonarrTools
{
  /// <summary>
  /// Get all TV series from Sonarr.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_all_series"), Description("Get all TV series from Sonarr")]
  public static async Task<object> GetAllSeries(ISonarrService sonarrService)
  {
    var series = await sonarrService.GetAllSeriesAsync();
    return new
    {
      count = series.Count,
      series = series.Select(s => new
      {
        id = s.Id,
        title = s.Title,
        year = s.Year,
        status = s.Status,
        monitored = s.Monitored,
        network = s.Network,
        seasonCount = s.Statistics?.SeasonCount ?? 0,
        episodeCount = s.Statistics?.EpisodeCount ?? 0,
        episodeFileCount = s.Statistics?.EpisodeFileCount ?? 0,
        percentOfEpisodes = s.Statistics?.PercentOfEpisodes ?? 0
      }).ToList()
    };
  }

  /// <summary>
  /// Get details for a specific TV series by ID.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_series"), Description("Get details for a specific TV series by ID")]
  public static async Task<object> GetSeries(
      ISonarrService sonarrService,
      [Description("The ID of the series to retrieve")] int seriesId)
  {
    var series = await sonarrService.GetSeriesByIdAsync(seriesId);

    if (series == null)
      return new { error = "Series not found" };

    return new
    {
      id = series.Id,
      title = series.Title,
      year = series.Year,
      status = series.Status,
      overview = series.Overview,
      network = series.Network,
      airTime = series.AirTime,
      monitored = series.Monitored,
      path = series.Path,
      genres = series.Genres,
      runtime = series.Runtime,
      certification = series.Certification,
      seasons = series.Seasons.Select(s => new
      {
        seasonNumber = s.SeasonNumber,
        monitored = s.Monitored,
        episodeCount = s.Statistics?.EpisodeCount ?? 0,
        episodeFileCount = s.Statistics?.EpisodeFileCount ?? 0
      }).ToList(),
      statistics = series.Statistics != null ? new
      {
        seasonCount = series.Statistics.SeasonCount,
        episodeCount = series.Statistics.EpisodeCount,
        episodeFileCount = series.Statistics.EpisodeFileCount,
        sizeOnDisk = series.Statistics.SizeOnDisk,
        percentOfEpisodes = series.Statistics.PercentOfEpisodes
      } : null
    };
  }

  /// <summary>
  /// Add a new TV series to Sonarr.
  /// </summary>
  [McpServerTool(Name = "sonarr_add_series"), Description("Add a new TV series to Sonarr")]
  public static async Task<object> AddSeries(
      ISonarrService sonarrService,
      [Description("The title of the series")] string title,
      [Description("The TVDB ID of the series")] int tvdbId,
      [Description("The quality profile ID to use")] int qualityProfileId,
      [Description("The root folder path where the series should be stored")] string rootFolderPath,
      [Description("Whether to monitor the series (default: true)")] bool monitored = true,
      [Description("Whether to use season folders (default: true)")] bool seasonFolder = true,
      [Description("The series type: standard, daily, or anime (default: standard)")] string seriesType = "standard",
      [Description("Whether to search for missing episodes after adding (default: false)")] bool searchForMissingEpisodes = false)
  {
    var request = new AddSeriesRequest
    {
      Title = title,
      TvdbId = tvdbId,
      QualityProfileId = qualityProfileId,
      RootFolderPath = rootFolderPath,
      Path = "", // Sonarr will construct this
      Monitored = monitored,
      SeasonFolder = seasonFolder,
      SeriesType = seriesType,
      AddOptions = new AddSeriesOptions
      {
        SearchForMissingEpisodes = searchForMissingEpisodes
      }
    };

    var series = await sonarrService.AddSeriesAsync(request);

    if (series == null)
      return new { error = "Failed to add series" };

    return new
    {
      success = true,
      id = series.Id,
      title = series.Title,
      path = series.Path
    };
  }

  /// <summary>
  /// Update an existing TV series in Sonarr.
  /// </summary>
  [McpServerTool(Name = "sonarr_update_series"), Description("Update an existing TV series in Sonarr")]
  public static async Task<object> UpdateSeries(
      ISonarrService sonarrService,
      [Description("The ID of the series to update")] int seriesId,
      [Description("Whether the series should be monitored")] bool? monitored = null,
      [Description("The quality profile ID to use")] int? qualityProfileId = null,
      [Description("The path where the series is stored")] string? path = null)
  {
    var series = await sonarrService.GetSeriesByIdAsync(seriesId);

    if (series == null)
      return new { error = "Series not found" };

    // Update fields if provided
    if (monitored.HasValue)
      series.Monitored = monitored.Value;

    if (qualityProfileId.HasValue)
      series.QualityProfileId = qualityProfileId.Value;

    if (!string.IsNullOrWhiteSpace(path))
      series.Path = path;

    var updatedSeries = await sonarrService.UpdateSeriesAsync(series);

    if (updatedSeries == null)
      return new { error = "Failed to update series" };

    return new { success = true, message = $"Updated series {updatedSeries.Title}" };
  }

  /// <summary>
  /// Get episodes, optionally filtered by series ID.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_episodes"), Description("Get episodes, optionally filtered by series ID")]
  public static async Task<object> GetEpisodes(
      ISonarrService sonarrService,
      [Description("Optional series ID to filter episodes")] int? seriesId = null)
  {
    var episodes = await sonarrService.GetEpisodesAsync(seriesId);

    return new
    {
      count = episodes.Count,
      episodes = episodes.Select(e => new
      {
        id = e.Id,
        seriesId = e.SeriesId,
        episodeNumber = e.EpisodeNumber,
        seasonNumber = e.SeasonNumber,
        title = e.Title,
        airDate = e.AirDate,
        hasFile = e.HasFile,
        monitored = e.Monitored,
        overview = e.Overview
      }).ToList()
    };
  }

  /// <summary>
  /// Get episodes that are wanted but missing.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_missing_episodes"), Description("Get episodes that are wanted but missing")]
  public static async Task<object> GetMissingEpisodes(
      ISonarrService sonarrService,
      [Description("Page number (default: 1)")] int page = 1,
      [Description("Page size (default: 20)")] int pageSize = 20)
  {
    var episodes = await sonarrService.GetMissingEpisodesAsync(page, pageSize);

    return new
    {
      count = episodes.Count,
      page,
      pageSize,
      episodes = episodes.Select(e => new
      {
        id = e.Id,
        seriesId = e.SeriesId,
        seriesTitle = e.Series?.Title,
        episodeNumber = e.EpisodeNumber,
        seasonNumber = e.SeasonNumber,
        title = e.Title,
        airDate = e.AirDate,
        monitored = e.Monitored
      }).ToList()
    };
  }

  /// <summary>
  /// Execute a command (SeriesSearch, EpisodeSearch, RefreshSeries, RescanSeries).
  /// </summary>
  [McpServerTool(Name = "sonarr_execute_command"), Description("Execute a command (SeriesSearch, EpisodeSearch, RefreshSeries, RescanSeries)")]
  public static async Task<object> ExecuteCommand(
      ISonarrService sonarrService,
      [Description("The command name: SeriesSearch, EpisodeSearch, RefreshSeries, or RescanSeries")] string commandName,
      [Description("Optional series ID for series-specific commands")] int? seriesId = null,
      [Description("Optional episode IDs for episode search commands")] int[]? episodeIds = null)
  {
    var request = new CommandRequest
    {
      Name = commandName,
      SeriesId = seriesId,
      EpisodeIds = episodeIds?.ToList()
    };

    var command = await sonarrService.ExecuteCommandAsync(request);

    if (command == null)
      return new { error = "Failed to execute command" };

    return new
    {
      success = true,
      commandId = command.Id,
      commandName = command.Name,
      status = command.Status,
      message = command.Message
    };
  }

  /// <summary>
  /// Get the download queue.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_queue"), Description("Get the download queue")]
  public static async Task<object> GetQueue(
      ISonarrService sonarrService,
      [Description("Page number (default: 1)")] int page = 1,
      [Description("Page size (default: 20)")] int pageSize = 20)
  {
    var queue = await sonarrService.GetQueueAsync(page, pageSize);

    return new
    {
      totalRecords = queue.TotalRecords,
      page = queue.Page,
      pageSize = queue.PageSize,
      items = queue.Records.Select(q => new
      {
        id = q.Id,
        seriesTitle = q.Series?.Title,
        episodeTitle = q.Episode?.Title,
        seasonNumber = q.Episode?.SeasonNumber,
        episodeNumber = q.Episode?.EpisodeNumber,
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
  /// Get Sonarr system status and version information.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_system_status"), Description("Get Sonarr system status and version information")]
  public static async Task<object> GetSystemStatus(ISonarrService sonarrService)
  {
    var status = await sonarrService.GetSystemStatusAsync();

    if (status == null)
      return new { error = "Failed to get system status" };

    return new
    {
      version = status.Version,
      buildTime = status.BuildTime,
      isDebug = status.IsDebug,
      isProduction = status.IsProduction,
      authentication = status.Authentication,
      startTime = status.StartTime,
      osName = status.OsName,
      osVersion = status.OsVersion,
      isDocker = status.IsDocker,
      runtimeVersion = status.RuntimeVersion,
      runtimeName = status.RuntimeName
    };
  }

  /// <summary>
  /// Get all configured root folders.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_root_folders"), Description("Get all configured root folders")]
  public static async Task<object> GetRootFolders(ISonarrService sonarrService)
  {
    var rootFolders = await sonarrService.GetRootFoldersAsync();

    return new
    {
      count = rootFolders.Count,
      rootFolders = rootFolders.Select(rf => new
      {
        id = rf.Id,
        path = rf.Path,
        accessible = rf.Accessible,
        freeSpace = rf.FreeSpace,
        totalSpace = rf.TotalSpace
      }).ToList()
    };
  }

  /// <summary>
  /// Get all configured quality profiles.
  /// </summary>
  [McpServerTool(Name = "sonarr_get_quality_profiles"), Description("Get all configured quality profiles")]
  public static async Task<object> GetQualityProfiles(ISonarrService sonarrService)
  {
    var profiles = await sonarrService.GetQualityProfilesAsync();

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
