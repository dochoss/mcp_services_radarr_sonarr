using System.Text.Json;
using FluentAssertions;
using RadarrSonarrMcp.Models;
using Xunit;

namespace RadarrSonarrMcp.Tests.Models;

/// <summary>
/// Unit tests for Sonarr model JSON serialization/deserialization.
/// </summary>
public class SonarrModelsTests
{
  [Fact]
  public void Series_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "title": "Ozark",
      "alternateTitles": [],
      "sortTitle": "ozark",
      "status": "ended",
      "ended": true,
      "overview": "A financial adviser drags his family from Chicago to Missouri.",
      "network": "Netflix",
      "airTime": "",
      "images": [],
      "remotePoster": "",
      "seasons": [],
      "year": 2017,
      "path": "/data/media/tv/Ozark",
      "qualityProfileId": 4,
      "seasonFolder": true,
      "monitored": true,
      "useSceneNumbering": false,
      "runtime": 60,
      "tvdbId": 318056,
      "tvRageId": 0,
      "tvMazeId": 17376,
      "firstAired": "2017-07-21T07:00:00Z",
      "seriesType": "standard",
      "cleanTitle": "ozark",
      "imdbId": "tt5071412",
      "titleSlug": "ozark",
      "certification": "TV-MA",
      "genres": ["Drama", "Crime"],
      "tags": [1, 2],
      "added": "2024-01-15T10:30:00Z",
      "ratings": {
        "votes": 100000,
        "value": 8.5
      },
      "statistics": {
        "seasonCount": 4,
        "episodeFileCount": 44,
        "episodeCount": 44,
        "totalEpisodeCount": 44,
        "sizeOnDisk": 20000000000,
        "percentOfEpisodes": 100.0
      }
    }
    """;

    // Act
    var series = JsonSerializer.Deserialize<Series>(json);

    // Assert
    series.Should().NotBeNull();
    series!.Id.Should().Be(1);
    series.Title.Should().Be("Ozark");
    series.Year.Should().Be(2017);
    series.TvdbId.Should().Be(318056);
    series.ImdbId.Should().Be("tt5071412");
    series.Runtime.Should().Be(60);
    series.Monitored.Should().BeTrue();
    series.Status.Should().Be("ended");
    series.Network.Should().Be("Netflix");
    series.Genres.Should().Contain("Drama");
    series.Genres.Should().Contain("Crime");
  }

  [Fact]
  public void Episode_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "seriesId": 5,
      "tvdbId": 12345,
      "episodeFileId": 100,
      "seasonNumber": 1,
      "episodeNumber": 5,
      "title": "Test Episode",
      "airDate": "2024-01-15",
      "airDateUtc": "2024-01-15T20:00:00Z",
      "overview": "Episode description",
      "monitored": true,
      "absoluteEpisodeNumber": null,
      "sceneAbsoluteEpisodeNumber": null,
      "sceneSeasonNumber": null,
      "sceneEpisodeNumber": null,
      "hasFile": true,
      "episodeFile": {
        "id": 100,
        "seriesId": 5,
        "seasonNumber": 1,
        "relativePath": "Season 01/Episode 05.mkv",
        "path": "/data/media/tv/Test Series/Season 01/Episode 05.mkv",
        "size": 2000000000,
        "dateAdded": "2024-01-15T10:00:00Z"
      }
    }
    """;

    // Act
    var episode = JsonSerializer.Deserialize<Episode>(json);

    // Assert
    episode.Should().NotBeNull();
    episode!.Id.Should().Be(1);
    episode.SeriesId.Should().Be(5);
    episode.SeasonNumber.Should().Be(1);
    episode.EpisodeNumber.Should().Be(5);
    episode.Title.Should().Be("Test Episode");
    episode.HasFile.Should().BeTrue();
    episode.Monitored.Should().BeTrue();
    episode.EpisodeFile.Should().NotBeNull();
    episode.EpisodeFile!.Size.Should().Be(2000000000);
  }

  [Fact]
  public void QualityProfile_ShouldDeserialize_FromSonarrApiJson()
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
    var profile = JsonSerializer.Deserialize<QualityProfile>(json);

    // Assert
    profile.Should().NotBeNull();
    profile!.Id.Should().Be(4);
    profile.Name.Should().Be("HD-1080p");
    profile.UpgradeAllowed.Should().BeFalse();
    profile.Cutoff.Should().Be(7);
  }

  [Fact]
  public void RootFolder_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "path": "/data/media/tv",
      "accessible": true,
      "freeSpace": 60179755008,
      "totalSpace": 1000000000000
    }
    """;

    // Act
    var folder = JsonSerializer.Deserialize<RootFolder>(json);

    // Assert
    folder.Should().NotBeNull();
    folder!.Id.Should().Be(1);
    folder.Path.Should().Be("/data/media/tv");
    folder.Accessible.Should().BeTrue();
    folder.FreeSpace.Should().Be(60179755008);
    folder.TotalSpace.Should().Be(1000000000000);
  }

  [Fact]
  public void SystemStatus_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "version": "4.0.0.1234",
      "buildTime": "2024-01-15T12:00:00Z",
      "isDebug": false,
      "isProduction": true,
      "authentication": "forms",
      "startTime": "2024-01-15T10:00:00Z",
      "appData": "/config",
      "osName": "ubuntu",
      "osVersion": "22.04",
      "isDocker": true,
      "runtimeVersion": "8.0.0",
      "runtimeName": ".NET"
    }
    """;

    // Act
    var status = JsonSerializer.Deserialize<SystemStatus>(json);

    // Assert
    status.Should().NotBeNull();
    status!.Version.Should().Be("4.0.0.1234");
    status.IsDocker.Should().BeTrue();
    status.IsProduction.Should().BeTrue();
    status.OsName.Should().Be("ubuntu");
  }

  [Fact]
  public void AddSeriesRequest_ShouldSerialize_ToSonarrApiJson()
  {
    // Arrange
    var request = new AddSeriesRequest
    {
      Title = "Test Series",
      TvdbId = 12345,
      QualityProfileId = 4,
      Path = "/data/media/tv/Test Series",
      Monitored = true,
      SeasonFolder = true,
      SeriesType = "standard",
      AddOptions = new AddSeriesOptions
      {
        SearchForMissingEpisodes = false
      }
    };

    // Act
    var json = JsonSerializer.Serialize(request);
    var deserialized = JsonSerializer.Deserialize<AddSeriesRequest>(json);

    // Assert
    deserialized.Should().NotBeNull();
    deserialized!.Title.Should().Be("Test Series");
    deserialized.TvdbId.Should().Be(12345);
    deserialized.QualityProfileId.Should().Be(4);
    deserialized.Monitored.Should().BeTrue();
    deserialized.SeasonFolder.Should().BeTrue();
  }

  [Fact]
  public void CommandRequest_ShouldSerialize_ToSonarrApiJson()
  {
    // Arrange
    var request = new CommandRequest
    {
      Name = "SeriesSearch",
      SeriesId = 5
    };

    // Act
    var json = JsonSerializer.Serialize(request);

    // Assert
    json.Should().Contain("\"name\":\"SeriesSearch\"");
    json.Should().Contain("\"seriesId\":5");
  }

  [Fact]
  public void QueueItem_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "seriesId": 5,
      "episodeId": 100,
      "title": "Test Series - S01E05",
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
      "outputPath": "/downloads/Test Series"
    }
    """;

    // Act
    var item = JsonSerializer.Deserialize<QueueItem>(json);

    // Assert
    item.Should().NotBeNull();
    item!.Id.Should().Be(1);
    item.SeriesId.Should().Be(5);
    item.EpisodeId.Should().Be(100);
    item.Title.Should().Be("Test Series - S01E05");
    item.Status.Should().Be("downloading");
    item.Protocol.Should().Be("torrent");
    item.DownloadClient.Should().Be("qBittorrent");
  }

  [Fact]
  public void Command_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "id": 100,
      "name": "SeriesSearch",
      "commandName": "SeriesSearch",
      "status": "queued",
      "queued": "2024-01-15T10:00:00Z",
      "started": null,
      "ended": null,
      "duration": null,
      "trigger": "manual",
      "message": null
    }
    """;

    // Act
    var command = JsonSerializer.Deserialize<Command>(json);

    // Assert
    command.Should().NotBeNull();
    command!.Id.Should().Be(100);
    command.Name.Should().Be("SeriesSearch");
    command.Status.Should().Be("queued");
    command.Trigger.Should().Be("manual");
  }

  [Fact]
  public void Series_WithNullValues_ShouldDeserialize_Correctly()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "title": "Test Series",
      "alternateTitles": null,
      "sortTitle": "testseries",
      "status": "continuing",
      "ended": false,
      "overview": null,
      "network": null,
      "airTime": null,
      "images": null,
      "remotePoster": null,
      "seasons": null,
      "year": 2023,
      "path": "/data/media/tv/Test Series",
      "qualityProfileId": 4,
      "seasonFolder": true,
      "monitored": true,
      "useSceneNumbering": false,
      "runtime": 0,
      "tvdbId": 12345,
      "tvRageId": 0,
      "tvMazeId": 0,
      "firstAired": null,
      "seriesType": "standard",
      "cleanTitle": "testseries",
      "imdbId": null,
      "titleSlug": "test-series",
      "certification": null,
      "genres": null,
      "tags": [],
      "added": "2024-01-15T10:30:00Z",
      "ratings": null,
      "statistics": null
    }
    """;

    // Act
    var series = JsonSerializer.Deserialize<Series>(json);

    // Assert
    series.Should().NotBeNull();
    series!.Id.Should().Be(1);
    series.Title.Should().Be("Test Series");
    series.Overview.Should().BeNull();
    series.Network.Should().BeNull();
    series.ImdbId.Should().BeNull();
  }

  [Fact]
  public void Episode_WithoutFile_ShouldDeserialize_Correctly()
  {
    // Arrange
    var json = """
    {
      "id": 1,
      "seriesId": 5,
      "tvdbId": 12345,
      "episodeFileId": 0,
      "seasonNumber": 1,
      "episodeNumber": 5,
      "title": "Missing Episode",
      "airDate": "2024-01-15",
      "airDateUtc": "2024-01-15T20:00:00Z",
      "overview": "This episode is not downloaded",
      "monitored": true,
      "hasFile": false,
      "episodeFile": null
    }
    """;

    // Act
    var episode = JsonSerializer.Deserialize<Episode>(json);

    // Assert
    episode.Should().NotBeNull();
    episode!.Id.Should().Be(1);
    episode.HasFile.Should().BeFalse();
    episode.EpisodeFile.Should().BeNull();
    episode.EpisodeFileId.Should().Be(0);
  }

  [Fact]
  public void EpisodeFile_ShouldDeserialize_WithQualityAndMediaInfo()
  {
    // Arrange
    var json = """
    {
      "id": 100,
      "seriesId": 5,
      "seasonNumber": 1,
      "relativePath": "Season 01/Episode 05.mkv",
      "path": "/data/media/tv/Test Series/Season 01/Episode 05.mkv",
      "size": 2000000000,
      "dateAdded": "2024-01-15T10:00:00Z",
      "quality": {
        "quality": {
          "id": 7,
          "name": "Bluray-1080p"
        },
        "revision": {
          "version": 1,
          "real": 0,
          "isRepack": false
        }
      },
      "mediaInfo": {
        "videoCodec": "x264",
        "videoBitrate": 5000,
        "videoBitDepth": 8,
        "width": 1920,
        "height": 1080,
        "audioCodec": "AAC",
        "audioBitrate": 320,
        "audioChannels": 5.1,
        "runTime": "00:45:30",
        "subtitles": "English"
      }
    }
    """;

    // Act
    var episodeFile = JsonSerializer.Deserialize<EpisodeFile>(json);

    // Assert
    episodeFile.Should().NotBeNull();
    episodeFile!.Id.Should().Be(100);
    episodeFile.Size.Should().Be(2000000000);
    episodeFile.Quality.Should().NotBeNull();
    episodeFile.Quality!.Quality!.Name.Should().Be("Bluray-1080p");
    episodeFile.MediaInfo.Should().NotBeNull();
    episodeFile.MediaInfo!.VideoCodec.Should().Be("x264");
    episodeFile.MediaInfo.Width.Should().Be(1920);
    episodeFile.MediaInfo.Height.Should().Be(1080);
  }

  [Fact]
  public void Season_ShouldDeserialize_FromSonarrApiJson()
  {
    // Arrange
    var json = """
    {
      "seasonNumber": 1,
      "monitored": true,
      "statistics": {
        "episodeFileCount": 10,
        "episodeCount": 10,
        "totalEpisodeCount": 10,
        "sizeOnDisk": 20000000000,
        "percentOfEpisodes": 100.0
      }
    }
    """;

    // Act
    var season = JsonSerializer.Deserialize<Season>(json);

    // Assert
    season.Should().NotBeNull();
    season!.SeasonNumber.Should().Be(1);
    season.Monitored.Should().BeTrue();
  }
}
