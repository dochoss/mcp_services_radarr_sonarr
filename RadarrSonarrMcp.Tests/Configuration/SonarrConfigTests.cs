using FluentAssertions;
using RadarrSonarrMcp.Configuration;
using Xunit;

namespace RadarrSonarrMcp.Tests.Configuration;

/// <summary>
/// Unit tests for SonarrConfig.
/// </summary>
public class SonarrConfigTests
{
  [Fact]
  public void IsValid_ShouldReturnTrue_WhenAllRequiredFieldsAreSet()
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key-12345",
      BasePath = "/api/v3",
      Port = "8989"
    };

    // Act
    var result = config.IsValid();

    // Assert
    result.Should().BeTrue();
  }

  [Theory]
  [InlineData(null, "/api/v3", "8989")]
  [InlineData("", "/api/v3", "8989")]
  [InlineData("test-key", null, "8989")]
  [InlineData("test-key", "", "8989")]
  [InlineData("test-key", "/api/v3", null)]
  [InlineData("test-key", "/api/v3", "")]
  public void IsValid_ShouldReturnFalse_WhenRequiredFieldsAreMissing(
    string? apiKey,
    string? basePath,
    string? port)
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = apiKey,
      BasePath = basePath,
      Port = port
    };

    // Act
    var result = config.IsValid();

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void GetBaseUrl_ShouldReturnCorrectUrl_WithDefaultPort()
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "8989"
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().Be("http://10.0.0.23:8989/api/v3");
  }

  [Theory]
  [InlineData("8080", "http://10.0.0.23:8080/api/v3")]
  [InlineData("8989", "http://10.0.0.23:8989/api/v3")]
  [InlineData("9090", "http://10.0.0.23:9090/api/v3")]
  public void GetBaseUrl_ShouldReturnCorrectUrl_WithCustomPort(string port, string expectedUrl)
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = port
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().Be(expectedUrl);
  }

  [Fact]
  public void GetBaseUrl_ShouldHandleDifferentIpAddresses()
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "8989"
    };

    // Act & Assert
    config.GetBaseUrl("192.168.1.100").Should().Be("http://192.168.1.100:8989/api/v3");
    config.GetBaseUrl("localhost").Should().Be("http://localhost:8989/api/v3");
    config.GetBaseUrl("sonarr.local").Should().Be("http://sonarr.local:8989/api/v3");
  }

  [Theory]
  [InlineData("/api/v3")]
  [InlineData("/api/v4")]
  [InlineData("/custom/api")]
  public void GetBaseUrl_ShouldHandleDifferentBasePaths(string basePath)
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = basePath,
      Port = "8989"
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().Be($"http://10.0.0.23:8989{basePath}");
  }

  [Fact]
  public void SonarrConfig_ShouldHaveDefaultValues()
  {
    // Arrange & Act
    var config = new SonarrConfig();

    // Assert
    config.BasePath.Should().Be("/api/v3");
    config.Port.Should().Be("8989");
  }

  [Fact]
  public void SonarrConfig_DefaultPort_ShouldBeDifferentFromRadarr()
  {
    // Arrange & Act
    var sonarrConfig = new SonarrConfig();
    var radarrConfig = new RadarrConfig();

    // Assert
    sonarrConfig.Port.Should().Be("8989");
    radarrConfig.Port.Should().Be("7878");
    sonarrConfig.Port.Should().NotBe(radarrConfig.Port);
  }

  [Fact]
  public void GetBaseUrl_ShouldConstructValidHttpUrl()
  {
    // Arrange
    var config = new SonarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "8989"
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().StartWith("http://");
    url.Should().Contain(nasIp);
    url.Should().Contain(config.Port);
    url.Should().EndWith(config.BasePath);
  }
}
