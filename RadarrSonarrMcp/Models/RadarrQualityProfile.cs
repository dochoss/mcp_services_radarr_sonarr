using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a quality profile in Radarr.
/// </summary>
public record RadarrQualityProfile
{
  /// <summary>
  /// Quality profile ID.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; init; }

  /// <summary>
  /// Name of the quality profile.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; init; } = string.Empty;

  /// <summary>
  /// Whether upgrades are allowed.
  /// </summary>
  [JsonPropertyName("upgradeAllowed")]
  public bool UpgradeAllowed { get; init; }

  /// <summary>
  /// Cutoff quality ID.
  /// </summary>
  [JsonPropertyName("cutoff")]
  public int Cutoff { get; init; }
}