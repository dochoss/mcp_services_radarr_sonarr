using FluentAssertions;
using RadarrSonarrMcp.Configuration;
using Xunit;

namespace RadarrSonarrMcp.Tests.Configuration;

/// <summary>
/// Unit tests for RadarrConfig.
/// </summary>
public class RadarrConfigTests
{
  [Fact]
  public void IsValid_ShouldReturnTrue_WhenAllRequiredFieldsAreSet()
  {
    // Arrange
    var config = new RadarrConfig
    {
      ApiKey = "test-api-key-12345",
      BasePath = "/api/v3",
      Port = "7878"
    };

    // Act
    var result = config.IsValid();

    // Assert
    result.Should().BeTrue();
  }

  [Theory]
  [InlineData(null, "/api/v3", "7878")]
  [InlineData("", "/api/v3", "7878")]
  [InlineData("test-key", null, "7878")]
  [InlineData("test-key", "", "7878")]
  [InlineData("test-key", "/api/v3", null)]
  [InlineData("test-key", "/api/v3", "")]
  public void IsValid_ShouldReturnFalse_WhenRequiredFieldsAreMissing(
    string? apiKey,
    string? basePath,
    string? port)
  {
    // Arrange
    var config = new RadarrConfig
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
    var config = new RadarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "7878"
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().Be("http://10.0.0.23:7878/api/v3");
  }

  [Theory]
  [InlineData("8080", "http://10.0.0.23:8080/api/v3")]
  [InlineData("7878", "http://10.0.0.23:7878/api/v3")]
  [InlineData("9090", "http://10.0.0.23:9090/api/v3")]
  public void GetBaseUrl_ShouldReturnCorrectUrl_WithCustomPort(string port, string expectedUrl)
  {
    // Arrange
    var config = new RadarrConfig
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
    var config = new RadarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = "/api/v3",
      Port = "7878"
    };

    // Act & Assert
    config.GetBaseUrl("192.168.1.100").Should().Be("http://192.168.1.100:7878/api/v3");
    config.GetBaseUrl("localhost").Should().Be("http://localhost:7878/api/v3");
    config.GetBaseUrl("radarr.local").Should().Be("http://radarr.local:7878/api/v3");
  }

  [Theory]
  [InlineData("/api/v3")]
  [InlineData("/api/v4")]
  [InlineData("/custom/api")]
  public void GetBaseUrl_ShouldHandleDifferentBasePaths(string basePath)
  {
    // Arrange
    var config = new RadarrConfig
    {
      ApiKey = "test-api-key",
      BasePath = basePath,
      Port = "7878"
    };
    var nasIp = "10.0.0.23";

    // Act
    var url = config.GetBaseUrl(nasIp);

    // Assert
    url.Should().Be($"http://10.0.0.23:7878{basePath}");
  }

  [Fact]
  public void RadarrConfig_ShouldHaveDefaultValues()
  {
    // Arrange & Act
    var config = new RadarrConfig();

    // Assert
    config.BasePath.Should().Be("/api/v3");
    config.Port.Should().Be("7878");
  }
}
