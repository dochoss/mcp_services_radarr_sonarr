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
/// Unit tests for SonarrService.
/// </summary>
public class SonarrServiceTests
{
  private readonly Mock<ILogger<SonarrService>> _mockLogger;
  private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
  private readonly HttpClient _httpClient;
  private readonly SonarrConfig _config;
  private readonly string _nasIp;

  public SonarrServiceTests()
  {
    _mockLogger = new Mock<ILogger<SonarrService>>();
    _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
    _nasIp = "10.0.0.23";
    _config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "8989"
    };
  }

  private SonarrService CreateService()
  {
    return new SonarrService(_httpClient, _config, _nasIp, _mockLogger.Object);
  }

  [Fact]
  public async Task GetAllSeriesAsync_ShouldReturnSeries_WhenApiReturnsData()
  {
    // Arrange
    var seriesList = new List<Series>
    {
      new Series
      {
        Id = 1,
        Title = "Ozark",
        Year = 2017,
        TvdbId = 318056,
        Path = "/data/media/tv/Ozark",
        Status = "ended",
        Monitored = true,
        QualityProfileId = 4,
        SeasonFolder = true
      }
    };
    var json = JsonSerializer.Serialize(seriesList);

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

    var service = CreateService();

    // Act
    var result = await service.GetAllSeriesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result[0].Title.Should().Be("Ozark");
    result[0].Year.Should().Be(2017);
    result[0].TvdbId.Should().Be(318056);
    result[0].Monitored.Should().BeTrue();
  }

  [Fact]
  public async Task GetAllSeriesAsync_ShouldReturnEmptyList_WhenApiCallFails()
  {
    // Arrange
    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ThrowsAsync(new HttpRequestException("API unavailable"));

    var service = CreateService();

    // Act
    var result = await service.GetAllSeriesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task GetSeriesByIdAsync_ShouldReturnSeries_WhenSeriesExists()
  {
    // Arrange
    var series = new Series
    {
      Id = 1,
      Title = "Breaking Bad",
      Year = 2008,
      TvdbId = 81189,
      Path = "/data/media/tv/Breaking Bad",
      Status = "ended",
      Monitored = true,
      QualityProfileId = 4,
      Statistics = new SeriesStatistics
      {
        SeasonCount = 5,
        EpisodeCount = 62,
        EpisodeFileCount = 62,
        SizeOnDisk = 41000000000
      }
    };
    var json = JsonSerializer.Serialize(series);

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

    var service = CreateService();

    // Act
    var result = await service.GetSeriesByIdAsync(1);

    // Assert
    result.Should().NotBeNull();
    result!.Title.Should().Be("Breaking Bad");
    result.Year.Should().Be(2008);
    result.Statistics.Should().NotBeNull();
    result.Statistics!.SeasonCount.Should().Be(5);
    result.Statistics!.EpisodeCount.Should().Be(62);
  }

  [Fact]
  public async Task GetSeriesByIdAsync_ShouldReturnNull_WhenSeriesNotFound()
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

    var service = CreateService();

    // Act
    var result = await service.GetSeriesByIdAsync(999);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetEpisodesAsync_ShouldReturnAllEpisodes_WhenNoSeriesIdProvided()
  {
    // Arrange
    var episodes = new List<Episode>
    {
      new Episode
      {
        Id = 1,
        SeriesId = 1,
        EpisodeNumber = 1,
        SeasonNumber = 1,
        Title = "Pilot",
        AirDate = "2008-01-20",
        HasFile = true
      },
      new Episode
      {
        Id = 2,
        SeriesId = 1,
        EpisodeNumber = 2,
        SeasonNumber = 1,
        Title = "Cat's in the Bag...",
        AirDate = "2008-01-27",
        HasFile = true
      }
    };
    var json = JsonSerializer.Serialize(episodes);

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

    var service = CreateService();

    // Act
    var result = await service.GetEpisodesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result[0].Title.Should().Be("Pilot");
    result[1].Title.Should().Be("Cat's in the Bag...");
  }

  [Fact]
  public async Task GetEpisodesAsync_ShouldReturnFilteredEpisodes_WhenSeriesIdProvided()
  {
    // Arrange
    var episodes = new List<Episode>
    {
      new Episode
      {
        Id = 1,
        SeriesId = 1,
        EpisodeNumber = 1,
        SeasonNumber = 1,
        Title = "Pilot",
        AirDate = "2008-01-20",
        HasFile = true
      }
    };
    var json = JsonSerializer.Serialize(episodes);

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

    var service = CreateService();

    // Act
    var result = await service.GetEpisodesAsync(1);

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result[0].SeriesId.Should().Be(1);
  }

  [Fact]
  public async Task AddSeriesAsync_ShouldReturnAddedSeries_WhenSuccessful()
  {
    // Arrange
    var request = new AddSeriesRequest
    {
      Title = "The Wire",
      TvdbId = 79126,
      QualityProfileId = 4,
      RootFolderPath = "/data/media/tv",
      Monitored = true,
      SeasonFolder = true
    };

    var addedSeries = new Series
    {
      Id = 5,
      Title = "The Wire",
      Year = 2002,
      TvdbId = 79126,
      Path = "/data/media/tv/The Wire",
      Status = "ended",
      Monitored = true,
      QualityProfileId = 4,
      SeasonFolder = true
    };
    var json = JsonSerializer.Serialize(addedSeries);

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

    var service = CreateService();

    // Act
    var result = await service.AddSeriesAsync(request);

    // Assert
    result.Should().NotBeNull();
    result!.Title.Should().Be("The Wire");
    result.TvdbId.Should().Be(79126);
    result.Id.Should().Be(5);
  }

  [Fact]
  public async Task UpdateSeriesAsync_ShouldReturnTrue_WhenSuccessful()
  {
    // Arrange
    var series = new Series
    {
      Id = 1,
      Title = "Ozark",
      Year = 2017,
      TvdbId = 318056,
      Path = "/data/media/tv/Ozark",
      Status = "ended",
      Monitored = false,
      QualityProfileId = 4,
      SeasonFolder = true
    };
    var json = JsonSerializer.Serialize(series);

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.Accepted,
        Content = new StringContent(json)
      });

    var service = CreateService();

    // Act
    var result = await service.UpdateSeriesAsync(series);

    // Assert
    result.Should().NotBeNull();
    result!.Monitored.Should().BeFalse();
  }

  [Fact]
  public async Task GetQualityProfilesAsync_ShouldReturnProfiles_WhenSuccessful()
  {
    // Arrange
    var profiles = new List<QualityProfile>
    {
      new QualityProfile
      {
        Id = 4,
        Name = "HD-1080p"
      },
      new QualityProfile
      {
        Id = 5,
        Name = "HD-720p"
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

    var service = CreateService();

    // Act
    var result = await service.GetQualityProfilesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result[0].Name.Should().Be("HD-1080p");
  }

  [Fact]
  public async Task GetRootFoldersAsync_ShouldReturnFolders_WhenSuccessful()
  {
    // Arrange
    var folders = new List<RootFolder>
    {
      new RootFolder
      {
        Id = 1,
        Path = "/data/media/tv",
        FreeSpace = 500000000000
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

    var service = CreateService();

    // Act
    var result = await service.GetRootFoldersAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(1);
    result[0].Path.Should().Be("/data/media/tv");
  }

  [Fact]
  public async Task GetSystemStatusAsync_ShouldReturnStatus_WhenSuccessful()
  {
    // Arrange
    var status = new SystemStatus
    {
      Version = "4.0.0.748",
      BuildTime = DateTime.Parse("2023-11-15T10:30:00Z"),
      IsDebug = false,
      IsProduction = true,
      Authentication = "forms",
      StartTime = DateTime.Parse("2023-11-20T08:00:00Z"),
      OsName = "ubuntu"
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

    var service = CreateService();

    // Act
    var result = await service.GetSystemStatusAsync();

    // Assert
    result.Should().NotBeNull();
    result!.Version.Should().Be("4.0.0.748");
    result.IsProduction.Should().BeTrue();
    result.OsName.Should().Be("ubuntu");
  }

  [Fact]
  public async Task ExecuteCommandAsync_ShouldReturnCommand_WhenSuccessful()
  {
    // Arrange
    var request = new CommandRequest
    {
      Name = "SeriesSearch",
      SeriesId = 1
    };

    var response = new Command
    {
      Id = 123,
      Name = "SeriesSearch",
      Status = "queued",
      Queued = DateTime.UtcNow
    };
    var json = JsonSerializer.Serialize(response);

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

    var service = CreateService();

    // Act
    var result = await service.ExecuteCommandAsync(request);

    // Assert
    result.Should().NotBeNull();
    result!.Name.Should().Be("SeriesSearch");
    result.Status.Should().Be("queued");
  }

  [Fact]
  public async Task GetQueueAsync_ShouldReturnEmptyQueue_WhenNoItemsInQueue()
  {
    // Arrange
    var queueResponse = new QueueResponse
    {
      Page = 1,
      PageSize = 20,
      TotalRecords = 0,
      Records = new List<QueueItem>()
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

    var service = CreateService();

    // Act
    var result = await service.GetQueueAsync();

    // Assert
    result.Should().NotBeNull();
    result.Records.Should().BeEmpty();
    result.TotalRecords.Should().Be(0);
  }

  [Fact]
  public async Task GetQueueAsync_ShouldReturnQueueItems_WhenItemsExist()
  {
    // Arrange
    var queueResponse = new QueueResponse
    {
      Page = 1,
      PageSize = 20,
      TotalRecords = 1,
      Records = new List<QueueItem>
      {
        new QueueItem
        {
          Id = 1,
          SeriesId = 1,
          EpisodeId = 10,
          Title = "Breaking Bad - S01E01 - Pilot",
          Status = "downloading",
          Size = 1500000000,
          SizeLeft = 500000000
        }
      }
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

    var service = CreateService();

    // Act
    var result = await service.GetQueueAsync();

    // Assert
    result.Should().NotBeNull();
    result.Records.Should().HaveCount(1);
    result.Records[0].Title.Should().Be("Breaking Bad - S01E01 - Pilot");
    result.Records[0].Status.Should().Be("downloading");
    result.Records[0].SizeLeft.Should().Be(500000000);
  }

  [Fact]
  public async Task GetAllSeriesAsync_ShouldHandleEmptyResponse_Gracefully()
  {
    // Arrange
    var json = JsonSerializer.Serialize(new List<Series>());

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

    var service = CreateService();

    // Act
    var result = await service.GetAllSeriesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task AddSeriesAsync_ShouldReturnNull_WhenApiReturnsError()
  {
    // Arrange
    var request = new AddSeriesRequest
    {
      Title = "Test Series",
      TvdbId = 12345,
      QualityProfileId = 4,
      RootFolderPath = "/data/media/tv",
      Monitored = true
    };

    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.BadRequest
      });

    var service = CreateService();

    // Act
    var result = await service.AddSeriesAsync(request);

    // Assert
    result.Should().BeNull();
  }

  [Fact]
  public async Task GetEpisodesAsync_ShouldReturnEmptyList_WhenApiCallFails()
  {
    // Arrange
    _mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>())
      .ThrowsAsync(new HttpRequestException("Connection failed"));

    var service = CreateService();

    // Act
    var result = await service.GetEpisodesAsync();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }
}
