using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using RadarrSonarrMcp.Configuration;
using RadarrSonarrMcp.Models;

namespace RadarrSonarrMcp.Services;

/// <summary>
/// Service for interacting with Sonarr API.
/// </summary>
public class SonarrService : ISonarrService
{
  private readonly HttpClient _httpClient;
  private readonly SonarrConfig _config;
  private readonly string _baseUrl;
  private readonly ILogger<SonarrService> _logger;
  private readonly JsonSerializerOptions _jsonOptions;

  /// <summary>
  /// Initializes a new instance of the SonarrService.
  /// </summary>
  /// <param name="httpClient">HTTP client for making requests.</param>
  /// <param name="config">Sonarr configuration.</param>
  /// <param name="nasIp">IP address of the NAS.</param>
  /// <param name="logger">Logger instance.</param>
  public SonarrService(HttpClient httpClient, SonarrConfig config, string nasIp, ILogger<SonarrService> logger)
  {
    _httpClient = httpClient;
    _config = config;
    _baseUrl = config.GetBaseUrl(nasIp);
    _logger = logger;

    _jsonOptions = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    _httpClient.Timeout = TimeSpan.FromSeconds(30);
  }

  /// <inheritdoc/>
  public async Task<List<Series>> GetAllSeriesAsync()
  {
    try
    {
      var url = $"{_baseUrl}/series?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching all series from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var series = JsonSerializer.Deserialize<List<Series>>(response, _jsonOptions);

      return series ?? new List<Series>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching series from Sonarr");
      return new List<Series>();
    }
  }

  /// <inheritdoc/>
  public async Task<Series?> GetSeriesByIdAsync(int seriesId)
  {
    try
    {
      var url = $"{_baseUrl}/series/{seriesId}?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching series {SeriesId} from {Url}", seriesId, url);

      var response = await _httpClient.GetStringAsync(url);
      var series = JsonSerializer.Deserialize<Series>(response, _jsonOptions);

      return series;
    }
    catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
      _logger.LogWarning("Series {SeriesId} not found", seriesId);
      return null;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching series {SeriesId} from Sonarr", seriesId);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<Series?> AddSeriesAsync(AddSeriesRequest request)
  {
    try
    {
      var url = $"{_baseUrl}/series?apikey={_config.ApiKey}";
      _logger.LogInformation("Adding series {Title} to Sonarr", request.Title);

      var json = JsonSerializer.Serialize(request, _jsonOptions);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseBody = await response.Content.ReadAsStringAsync();
      var series = JsonSerializer.Deserialize<Series>(responseBody, _jsonOptions);

      _logger.LogInformation("Successfully added series {Title} with ID {SeriesId}", request.Title, series?.Id);
      return series;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error adding series {Title} to Sonarr", request.Title);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<Series?> UpdateSeriesAsync(Series series)
  {
    try
    {
      var url = $"{_baseUrl}/series/{series.Id}?apikey={_config.ApiKey}";
      _logger.LogInformation("Updating series {SeriesId} in Sonarr", series.Id);

      var json = JsonSerializer.Serialize(series, _jsonOptions);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PutAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseBody = await response.Content.ReadAsStringAsync();
      var updatedSeries = JsonSerializer.Deserialize<Series>(responseBody, _jsonOptions);

      _logger.LogInformation("Successfully updated series {SeriesId}", series.Id);
      return updatedSeries;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating series {SeriesId} in Sonarr", series.Id);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<List<Episode>> GetEpisodesAsync(int? seriesId = null)
  {
    try
    {
      var url = $"{_baseUrl}/episode?apikey={_config.ApiKey}";
      if (seriesId.HasValue)
      {
        url += $"&seriesId={seriesId.Value}";
      }

      _logger.LogInformation("Fetching episodes from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var episodes = JsonSerializer.Deserialize<List<Episode>>(response, _jsonOptions);

      return episodes ?? new List<Episode>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching episodes from Sonarr");
      return new List<Episode>();
    }
  }

  /// <inheritdoc/>
  public async Task<List<Episode>> GetMissingEpisodesAsync(int page = 1, int pageSize = 20, string sortKey = "airDateUtc", string sortDirection = "descending")
  {
    try
    {
      var url = $"{_baseUrl}/wanted/missing?page={page}&pageSize={pageSize}&sortKey={sortKey}&sortDirection={sortDirection}&apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching missing episodes from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);

      // The wanted/missing endpoint returns a paginated response
      var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<Episode>>(response, _jsonOptions);

      return paginatedResponse?.Records ?? new List<Episode>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching missing episodes from Sonarr");
      return new List<Episode>();
    }
  }

  /// <inheritdoc/>
  public async Task<Command?> ExecuteCommandAsync(CommandRequest request)
  {
    try
    {
      var url = $"{_baseUrl}/command?apikey={_config.ApiKey}";
      _logger.LogInformation("Executing command {CommandName}", request.Name);

      var json = JsonSerializer.Serialize(request, _jsonOptions);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseBody = await response.Content.ReadAsStringAsync();
      var command = JsonSerializer.Deserialize<Command>(responseBody, _jsonOptions);

      _logger.LogInformation("Successfully executed command {CommandName} with ID {CommandId}", request.Name, command?.Id);
      return command;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error executing command {CommandName}", request.Name);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<QueueResponse> GetQueueAsync(int page = 1, int pageSize = 20)
  {
    try
    {
      var url = $"{_baseUrl}/queue?page={page}&pageSize={pageSize}&includeUnknownSeriesItems=false&apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching queue from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var queue = JsonSerializer.Deserialize<QueueResponse>(response, _jsonOptions);

      return queue ?? new QueueResponse();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching queue from Sonarr");
      return new QueueResponse();
    }
  }

  /// <inheritdoc/>
  public async Task<SystemStatus?> GetSystemStatusAsync()
  {
    try
    {
      var url = $"{_baseUrl}/system/status?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching system status from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var status = JsonSerializer.Deserialize<SystemStatus>(response, _jsonOptions);

      return status;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching system status from Sonarr");
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<List<RootFolder>> GetRootFoldersAsync()
  {
    try
    {
      var url = $"{_baseUrl}/rootfolder?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching root folders from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var rootFolders = JsonSerializer.Deserialize<List<RootFolder>>(response, _jsonOptions);

      return rootFolders ?? new List<RootFolder>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching root folders from Sonarr");
      return new List<RootFolder>();
    }
  }

  /// <inheritdoc/>
  public async Task<List<QualityProfile>> GetQualityProfilesAsync()
  {
    try
    {
      var url = $"{_baseUrl}/qualityprofile?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching quality profiles from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      var profiles = JsonSerializer.Deserialize<List<QualityProfile>>(response, _jsonOptions);

      return profiles ?? new List<QualityProfile>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching quality profiles from Sonarr");
      return new List<QualityProfile>();
    }
  }
}

/// <summary>
/// Generic paginated response wrapper.
/// </summary>
/// <typeparam name="T">Type of records in the response.</typeparam>
internal class PaginatedResponse<T>
{
  [JsonPropertyName("page")]
  public int Page { get; set; }

  [JsonPropertyName("pageSize")]
  public int PageSize { get; set; }

  [JsonPropertyName("sortKey")]
  public string? SortKey { get; set; }

  [JsonPropertyName("sortDirection")]
  public string? SortDirection { get; set; }

  [JsonPropertyName("totalRecords")]
  public int TotalRecords { get; set; }

  [JsonPropertyName("records")]
  public List<T> Records { get; set; } = new();
}
