using System.Text.Json;
using FluentAssertions;
using RadarrSonarrMcp.Models;
using Xunit;

namespace RadarrSonarrMcp.Tests.Models;

/// <summary>
/// Unit tests for Radarr model JSON serialization/deserialization.
/// </summary>
public class RadarrModelsTests
{
  [Fact]
  public void Movie_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "title": "Team America: World Police",
      "originalTitle": "Team America: World Police",
      "year": 2004,
      "path": "/data/media/movies/Team America - World Police (2004)",
      "qualityProfileId": 4,
      "monitored": true,
      "overview": "A satirical comedy about a special forces team.",
      "runtime": 98,
      "imdbId": "tt0372588",
      "tmdbId": 3989,
      "titleSlug": "3989",
      "rootFolderPath": "/data/media/movies",
      "certification": "R",
      "genres": ["Action", "Comedy"],
      "tags": [1, 2],
      "added": "2024-01-15T10:30:00Z",
      "status": "released",
      "hasFile": false,
      "movieFileId": null,
      "sizeOnDisk": 0,
      "inCinemas": "2004-10-10T00:00:00Z",
      "physicalRelease": null,
      "digitalRelease": null,
      "ratings": {
        "imdb": {
          "votes": 100000,
          "value": 7.2
        },
        "tmdb": {
          "votes": 50000,
          "value": 7.0
        }
      },
      "statistics": {
        "movieFileCount": 0,
        "sizeOnDisk": 0,
        "releaseGroups": []
      }
    }
    """;

    // Act
    var movie = JsonSerializer.Deserialize<Movie>(json);

    // Assert
    movie.Should().NotBeNull();
    movie!.Id.Should().Be(1);
    movie.Title.Should().Be("Team America: World Police");
    movie.Year.Should().Be(2004);
    movie.TmdbId.Should().Be(3989);
    movie.ImdbId.Should().Be("tt0372588");
    movie.Runtime.Should().Be(98);
    movie.Monitored.Should().BeTrue();
    movie.HasFile.Should().BeFalse();
    movie.Genres.Should().Contain("Action");
    movie.Genres.Should().Contain("Comedy");
    movie.Ratings.Should().NotBeNull();
    movie.Ratings!.Imdb!.Value.Should().Be(7.2);
  }

  [Fact]
  public void RadarrQualityProfile_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 4,
      "name": "HD-1080p",
      "upgradeAllowed": false,
      "cutoff": 7
    }
    """;

    // Act
    var profile = JsonSerializer.Deserialize<RadarrQualityProfile>(json);

    // Assert
    profile.Should().NotBeNull();
    profile!.Id.Should().Be(4);
    profile.Name.Should().Be("HD-1080p");
    profile.UpgradeAllowed.Should().BeFalse();
    profile.Cutoff.Should().Be(7);
  }

  [Fact]
  public void RadarrRootFolder_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "path": "/data/media/movies",
      "accessible": true,
      "freeSpace": 60179755008,
      "totalSpace": 1000000000000
    }
    """;

    // Act
    var folder = JsonSerializer.Deserialize<RadarrRootFolder>(json);

    // Assert
    folder.Should().NotBeNull();
    folder!.Id.Should().Be(1);
    folder.Path.Should().Be("/data/media/movies");
    folder.Accessible.Should().BeTrue();
    folder.FreeSpace.Should().Be(60179755008);
    folder.TotalSpace.Should().Be(1000000000000);
  }

  [Fact]
  public void RadarrSystemStatus_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "version": "5.0.0.8366",
      "buildTime": "2024-01-15T12:00:00Z",
      "isDebug": false,
      "isProduction": true,
      "isAdmin": false,
      "isUserInteractive": false,
      "startupPath": "/app/radarr/bin",
      "appData": "/config",
      "osName": "ubuntu",
      "osVersion": "22.04",
      "isNetCore": true,
      "isLinux": true,
      "isOsx": false,
      "isWindows": false,
      "isDocker": true,
      "mode": "console",
      "branch": "master",
      "authentication": "forms",
      "sqliteVersion": "3.45.0",
      "urlBase": "",
      "runtimeVersion": "8.0.0",
      "runtimeName": ".NET",
      "startTime": "2024-01-15T10:00:00Z"
    }
    """;

    // Act
    var status = JsonSerializer.Deserialize<RadarrSystemStatus>(json);

    // Assert
    status.Should().NotBeNull();
    status!.Version.Should().Be("5.0.0.8366");
    status.IsDocker.Should().BeTrue();
    status.IsLinux.Should().BeTrue();
    status.IsWindows.Should().BeFalse();
    status.OsName.Should().Be("ubuntu");
    status.Branch.Should().Be("master");
  }

  [Fact]
  public void AddMovieRequest_ShouldSerialize_ToRadarrApiJson()
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
      Monitored = true,
      MinimumAvailability = "released",
      SearchForMovie = false,
      Tags = new List<int> { 1, 2 }
    };

    // Act
    var json = JsonSerializer.Serialize(request);
    var deserialized = JsonSerializer.Deserialize<AddMovieRequest>(json);

    // Assert
    deserialized.Should().NotBeNull();
    deserialized!.Title.Should().Be("Test Movie");
    deserialized.TmdbId.Should().Be(12345);
    deserialized.QualityProfileId.Should().Be(4);
    deserialized.Year.Should().Be(2023);
    deserialized.Monitored.Should().BeTrue();
    deserialized.SearchForMovie.Should().BeFalse();
  }

  [Fact]
  public void RadarrCommandRequest_ShouldSerialize_ToRadarrApiJson()
  {
    // Arrange
    var request = new RadarrCommandRequest
    {
      Name = "MovieSearch",
      MovieId = 5
    };

    // Act
    var json = JsonSerializer.Serialize(request);

    // Assert
    json.Should().Contain("\"name\":\"MovieSearch\"");
    json.Should().Contain("\"movieId\":5");
  }

  [Fact]
  public void RadarrQueueItem_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "movieId": 5,
      "title": "Test Movie - 1080p",
      "size": 2000000000,
      "sizeleft": 1000000000,
      "status": "downloading",
      "trackedDownloadStatus": "ok",
      "trackedDownloadState": "downloading",
      "timeleft": "00:30:00",
      "estimatedCompletionTime": "2024-01-15T14:00:00Z",
      "protocol": "torrent",
      "downloadClient": "qBittorrent",
      "downloadId": "abc123",
      "indexer": "Test Indexer",
      "outputPath": "/downloads/Test Movie",
      "statusMessages": []
    }
    """;

    // Act
    var item = JsonSerializer.Deserialize<RadarrQueueItem>(json);

    // Assert
    item.Should().NotBeNull();
    item!.Id.Should().Be(1);
    item.MovieId.Should().Be(5);
    item.Title.Should().Be("Test Movie - 1080p");
    item.Status.Should().Be("downloading");
    item.Protocol.Should().Be("torrent");
    item.DownloadClient.Should().Be("qBittorrent");
  }

  [Fact]
  public void RadarrQueueResponse_ShouldDeserialize_FromRadarrApiJson()
  {
    // Arrange
    var json = """
    {
      "page": 1,
      "pageSize": 20,
      "sortKey": "timeleft",
      "sortDirection": "ascending",
      "totalRecords": 5,
      "records": []
    }
    """;

    // Act
    var response = JsonSerializer.Deserialize<RadarrQueueResponse>(json);

    // Assert
    response.Should().NotBeNull();
    response!.Page.Should().Be(1);
    response.PageSize.Should().Be(20);
    response.TotalRecords.Should().Be(5);
    response.SortKey.Should().Be("timeleft");
  }

  [Fact]
  public void Movie_WithNullValues_ShouldDeserialize_Correctly()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "title": "Test Movie",
      "originalTitle": null,
      "year": 2023,
      "path": null,
      "qualityProfileId": 4,
      "monitored": true,
      "overview": null,
      "runtime": 0,
      "imdbId": null,
      "tmdbId": 12345,
      "titleSlug": "test-movie-2023",
      "rootFolderPath": null,
      "certification": null,
      "genres": null,
      "tags": [],
      "added": "2024-01-15T10:30:00Z",
      "status": "announced",
      "hasFile": false,
      "movieFileId": null,
      "sizeOnDisk": 0,
      "inCinemas": null,
      "physicalRelease": null,
      "digitalRelease": null,
      "ratings": null,
      "statistics": null
    }
    """;

    // Act
    var movie = JsonSerializer.Deserialize<Movie>(json);

    // Assert
    movie.Should().NotBeNull();
    movie!.Id.Should().Be(1);
    movie.Title.Should().Be("Test Movie");
    movie.OriginalTitle.Should().BeNull();
    movie.Overview.Should().BeNull();
    movie.Ratings.Should().BeNull();
    movie.Statistics.Should().BeNull();
  }

  [Fact]
  public void MovieRatings_ShouldDeserialize_WithAllSources()
  {
    // Arrange
    var json = """
    {
      "imdb": {
        "votes": 100000,
        "value": 7.2
      },
      "tmdb": {
        "votes": 50000,
        "value": 7.0
      },
      "rottenTomatoes": {
        "votes": 200,
        "value": 75.0
      }
    }
    """;

    // Act
    var ratings = JsonSerializer.Deserialize<MovieRatings>(json);

    // Assert
    ratings.Should().NotBeNull();
    ratings!.Imdb.Should().NotBeNull();
    ratings.Imdb!.Votes.Should().Be(100000);
    ratings.Imdb.Value.Should().Be(7.2);
    ratings.Tmdb.Should().NotBeNull();
    ratings.Tmdb!.Value.Should().Be(7.0);
    ratings.RottenTomatoes.Should().NotBeNull();
    ratings.RottenTomatoes!.Value.Should().Be(75.0);
  }
}
