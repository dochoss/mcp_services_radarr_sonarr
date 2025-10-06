using System.Text.Json;
using Microsoft.Extensions.Logging;
using RadarrSonarrMcp.Configuration;
using RadarrSonarrMcp.Models;

namespace RadarrSonarrMcp.Services;

/// <summary>
/// Service for interacting with the Radarr API
/// </summary>
public class RadarrService : IRadarrService
{
  private readonly HttpClient _httpClient;
  private readonly ILogger<RadarrService> _logger;
  private readonly RadarrConfig _config;
  private readonly string _nasIp;

  public RadarrService(HttpClient httpClient, ILogger<RadarrService> logger, RadarrConfig config, string nasIp)
  {
    _httpClient = httpClient;
    _logger = logger;
    _config = config;
    _nasIp = nasIp;
    _httpClient.Timeout = TimeSpan.FromSeconds(30);
  }

  /// <inheritdoc/>
  public async Task<List<Movie>> GetAllMoviesAsync()
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching all movies from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<List<Movie>>(response) ?? new List<Movie>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching all movies from Radarr");
      return new List<Movie>();
    }
  }

  /// <inheritdoc/>
  public async Task<Movie?> GetMovieByIdAsync(int movieId)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie/{movieId}?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching movie {MovieId} from {Url}", movieId, url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<Movie>(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching movie {MovieId} from Radarr", movieId);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<Movie?> AddMovieAsync(AddMovieRequest request)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie?apikey={_config.ApiKey}";
      _logger.LogInformation("Adding movie {Title} to Radarr", request.Title);

      var json = JsonSerializer.Serialize(request);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<Movie>(responseContent);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error adding movie {Title} to Radarr", request.Title);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<Movie?> UpdateMovieAsync(int movieId, Movie movie)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie/{movieId}?apikey={_config.ApiKey}";
      _logger.LogInformation("Updating movie {MovieId} in Radarr", movieId);

      var json = JsonSerializer.Serialize(movie);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PutAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<Movie>(responseContent);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating movie {MovieId} in Radarr", movieId);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteMovieAsync(int movieId, bool deleteFiles = false, bool addImportExclusion = false)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie/{movieId}?apikey={_config.ApiKey}&deleteFiles={deleteFiles}&addImportExclusion={addImportExclusion}";
      _logger.LogInformation("Deleting movie {MovieId} from Radarr", movieId);

      var response = await _httpClient.DeleteAsync(url);
      return response.IsSuccessStatusCode;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error deleting movie {MovieId} from Radarr", movieId);
      return false;
    }
  }

  /// <inheritdoc/>
  public async Task<List<Movie>> SearchMoviesAsync(string term)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/movie/lookup?term={Uri.EscapeDataString(term)}&apikey={_config.ApiKey}";
      _logger.LogInformation("Searching movies with term '{Term}' from {Url}", term, url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<List<Movie>>(response) ?? new List<Movie>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error searching movies with term '{Term}' from Radarr", term);
      return new List<Movie>();
    }
  }

  /// <inheritdoc/>
  public async Task<RadarrQueueResponse> GetQueueAsync(int page = 1, int pageSize = 20)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/queue?page={page}&pageSize={pageSize}&apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching queue from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<RadarrQueueResponse>(response) ?? new RadarrQueueResponse
      {
          Page = page,
          PageSize = pageSize,
          TotalRecords = 0,
          Records = new List<RadarrQueueItem>()
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching queue from Radarr");
      return new RadarrQueueResponse
      {
          Page = page,
          PageSize = pageSize,
          TotalRecords = 0,
          Records = new List<RadarrQueueItem>()
      };
    }
  }

  /// <inheritdoc/>
  public async Task<List<Movie>> GetMissingMoviesAsync()
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/wanted/missing?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching missing movies from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);

      // Parse the paginated response
      var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<Movie>>(response);
      return paginatedResponse?.Records ?? new List<Movie>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching missing movies from Radarr");
      return new List<Movie>();
    }
  }

  /// <inheritdoc/>
  public async Task<RadarrCommand?> ExecuteCommandAsync(RadarrCommandRequest command)
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/command?apikey={_config.ApiKey}";
      _logger.LogInformation("Executing command {CommandName} in Radarr", command.Name);

      var json = JsonSerializer.Serialize(command);
      var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(url, content);
      response.EnsureSuccessStatusCode();

      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<RadarrCommand>(responseContent);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error executing command {CommandName} in Radarr", command.Name);
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<RadarrSystemStatus?> GetSystemStatusAsync()
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/system/status?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching system status from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<RadarrSystemStatus>(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching system status from Radarr");
      return null;
    }
  }

  /// <inheritdoc/>
  public async Task<List<RadarrRootFolder>> GetRootFoldersAsync()
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/rootfolder?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching root folders from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<List<RadarrRootFolder>>(response) ?? new List<RadarrRootFolder>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching root folders from Radarr");
      return new List<RadarrRootFolder>();
    }
  }

  /// <inheritdoc/>
  public async Task<List<RadarrQualityProfile>> GetQualityProfilesAsync()
  {
    try
    {
      var url = $"{_config.GetBaseUrl(_nasIp)}/qualityprofile?apikey={_config.ApiKey}";
      _logger.LogInformation("Fetching quality profiles from {Url}", url);

      var response = await _httpClient.GetStringAsync(url);
      return JsonSerializer.Deserialize<List<RadarrQualityProfile>>(response) ?? new List<RadarrQualityProfile>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching quality profiles from Radarr");
      return new List<RadarrQualityProfile>();
    }
  }

  /// <summary>
  /// Internal class for handling paginated responses
  /// </summary>
  private class PaginatedResponse<T>
  {
    public List<T>? Records { get; set; }
  }
}
