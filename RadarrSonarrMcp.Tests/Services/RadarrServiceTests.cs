using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using RadarrSonarrMcp.Configuration;
using RadarrSonarrMcp.Models;
using RadarrSonarrMcp.Services;
using Xunit;

namespace RadarrSonarrMcp.Tests.Services;

/// <summary>
/// Unit tests for RadarrService.
/// </summary>
public class RadarrServiceTests
{
  private readonly Mock<ILogger<RadarrService>> _mockLogger;
  private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
  private readonly HttpClient _httpClient;
  private readonly RadarrConfig _config;
  private readonly string _nasIp;

  public RadarrServiceTests()
  {
    _mockLogger = new Mock<ILogger<RadarrService>>();
    _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
    _nasIp = "10.0.0.23";
    _config = new RadarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "7878"
    };
  }

  [Fact]
  public async Task GetAllMoviesAsync_ShouldReturnMovies_WhenApiReturnsData()
  {
    // Arrange
    var movies = new List<Movie>
    {
      new Movie
      {
        Id = 1,
        Title = "Test Movie",
        Year = 2023,
        TmdbId = 12345,
        TitleSlug = "test-movie-2023",
        Status = "released",
        HasFile = true,
        Monitored = true,
        QualityProfileId = 4
      }
    };
    var json = JsonSerializer.Serialize(movies);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetAllMoviesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result[0].Title.Should().Be("Test Movie");
    result[0].Year.Should().Be(2023);
    result[0].HasFile.Should().BeTrue();
  }

  [Fact]
  public async Task GetAllMoviesAsync_ShouldReturnEmptyList_WhenApiCallFails()
  {
    // Arrange
    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ThrowsAsync(new HttpRequestException("API unavailable"));

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetAllMoviesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenMovieExists()
  {
    // Arrange
    var movie = new Movie
    {
      Id = 1,
      Title = "Team America: World Police",
      Year = 2004,
      TmdbId = 3989,
      TitleSlug = "3989",
      Status = "released",
      HasFile = false,
      Monitored = true,
      QualityProfileId = 4,
      Runtime = 98
    };
    var json = JsonSerializer.Serialize(movie);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetMovieByIdAsync(1);

    // Assert
    result.Should().NotBeNull();
    result!.Title.Should().Be("Team America: World Police");
    result.Year.Should().Be(2004);
    result.Runtime.Should().Be(98);
  }

  [Fact]
  public async Task GetMovieByIdAsync_ShouldReturnNull_WhenMovieNotFound()
  {
    // Arrange
    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.NotFound
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetMovieByIdAsync(999);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task SearchMoviesAsync_ShouldReturnMovies_WhenSearchTermMatches()
  {
    // Arrange
    var movies = new List<Movie>
    {
      new Movie
      {
        Id = 0,
        Title = "Team America: World Police",
        Year = 2004,
        TmdbId = 3989,
        TitleSlug = "3989",
        Status = "released",
        Runtime = 98
      },
      new Movie
      {
        Id = 0,
        Title = "Team America: Building the World",
        Year = 2005,
        TmdbId = 1038759,
        TitleSlug = "1038759",
        Status = "released",
        Runtime = 13
      }
    };
    var json = JsonSerializer.Serialize(movies);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.SearchMoviesAsync("Team America");

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result[0].Title.Should().Contain("Team America");
    result[1].Title.Should().Contain("Team America");
  }

  [Fact]
  public async Task AddMovieAsync_ShouldReturnMovie_WhenAddSucceeds()
  {
    // Arrange
    var request = new AddMovieRequest
    {
      Title = "Test Movie",
      TmdbId = 12345,
      QualityProfileId = 4,
      RootFolderPath = "/data/media/movies",
      TitleSlug = "test-movie-2023",
      Year = 2023,
      Monitored = true
    };

    var addedMovie = new Movie
    {
      Id = 1,
      Title = "Test Movie",
      Year = 2023,
      TmdbId = 12345,
      TitleSlug = "test-movie-2023",
      Status = "announced",
      HasFile = false,
      Monitored = true,
      QualityProfileId = 4,
      Path = "/data/media/movies/Test Movie (2023)"
    };
    var json = JsonSerializer.Serialize(addedMovie);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.Created,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.AddMovieAsync(request);

    // Assert
    result.Should().NotBeNull();
    result!.Title.Should().Be("Test Movie");
    result.Year.Should().Be(2023);
    result.Id.Should().Be(1);
  }

  [Fact]
  public async Task GetQualityProfilesAsync_ShouldReturnProfiles_WhenApiReturnsData()
  {
    // Arrange
    var profiles = new List<RadarrQualityProfile>
    {
      new RadarrQualityProfile
      {
        Id = 1,
        Name = "Any",
        UpgradeAllowed = true,
        Cutoff = 7
      },
      new RadarrQualityProfile
      {
        Id = 4,
        Name = "HD-1080p",
        UpgradeAllowed = false,
        Cutoff = 7
      }
    };
    var json = JsonSerializer.Serialize(profiles);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetQualityProfilesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result[0].Name.Should().Be("Any");
    result[1].Name.Should().Be("HD-1080p");
  }

  [Fact]
  public async Task GetRootFoldersAsync_ShouldReturnFolders_WhenApiReturnsData()
  {
    // Arrange
    var folders = new List<RadarrRootFolder>
    {
      new RadarrRootFolder
      {
        Id = 1,
        Path = "/data/media/movies",
        Accessible = true,
        FreeSpace = 1000000000000,
        TotalSpace = 2000000000000
      }
    };
    var json = JsonSerializer.Serialize(folders);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetRootFoldersAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result[0].Path.Should().Be("/data/media/movies");
    result[0].Accessible.Should().BeTrue();
  }

  [Fact]
  public async Task GetSystemStatusAsync_ShouldReturnStatus_WhenApiReturnsData()
  {
    // Arrange
    var status = new RadarrSystemStatus
    {
      Version = "5.0.0",
      OsName = "ubuntu",
      IsDocker = true,
      IsLinux = true,
      IsProduction = true,
      Branch = "master"
    };
    var json = JsonSerializer.Serialize(status);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetSystemStatusAsync();

    // Assert
    result.Should().NotBeNull();
    result!.Version.Should().Be("5.0.0");
    result.IsDocker.Should().BeTrue();
    result.IsLinux.Should().BeTrue();
  }

  [Fact]
  public async Task DeleteMovieAsync_ShouldReturnTrue_WhenDeleteSucceeds()
  {
    // Arrange
    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.DeleteMovieAsync(1, false, false);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public async Task ExecuteCommandAsync_ShouldReturnCommand_WhenExecuteSucceeds()
  {
    // Arrange
    var request = new RadarrCommandRequest
    {
      Name = "MovieSearch",
      MovieId = 1
    };

    var command = new RadarrCommand
    {
      Id = 100,
      Name = "MovieSearch",
      Status = "queued",
      Queued = DateTime.UtcNow
    };
    var json = JsonSerializer.Serialize(command);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.Created,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.ExecuteCommandAsync(request);

    // Assert
    result.Should().NotBeNull();
    result!.Name.Should().Be("MovieSearch");
    result.Status.Should().Be("queued");
  }

  [Theory]
  [InlineData(1, 10)]
  [InlineData(2, 20)]
  [InlineData(3, 50)]
  public async Task GetQueueAsync_ShouldReturnQueueWithCorrectPagination(int page, int pageSize)
  {
    // Arrange
    var queueResponse = new RadarrQueueResponse
    {
      Page = page,
      PageSize = pageSize,
      TotalRecords = 100,
      Records = new List<RadarrQueueItem>()
    };
    var json = JsonSerializer.Serialize(queueResponse);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(json)
      });

    var service = new RadarrService(_httpClient, _mockLogger.Object, _config, _nasIp);

    // Act
    var result = await service.GetQueueAsync(page, pageSize);

    // Assert
    result.Should().NotBeNull();
    result.Page.Should().Be(page);
    result.PageSize.Should().Be(pageSize);
    result.TotalRecords.Should().Be(100);
  }
}
