using System.Text.Json.Serialization;

namespace RadarrSonarrMcp.Models;

/// <summary>
/// Represents a quality profile in Sonarr.
/// </summary>
public class QualityProfile
{
  /// <summary>
  /// Unique identifier for the quality profile.
  /// </summary>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Name of the quality profile.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Whether upgrades are allowed.
  /// </summary>
  [JsonPropertyName("upgradeAllowed")]
  public bool UpgradeAllowed { get; set; }

  /// <summary>
  /// Cutoff quality ID (stop upgrading after this quality).
  /// </summary>
  [JsonPropertyName("cutoff")]
  public int Cutoff { get; set; }

  /// <summary>
  /// List of quality items in this profile.
  /// </summary>
  [JsonPropertyName("items")]
  public List<QualityProfileItem> Items { get; set; } = new();
}

/// <summary>
/// Represents a quality item within a profile.
/// </summary>
public class QualityProfileItem
{
  /// <summary>
  /// Quality definition.
  /// </summary>
  [JsonPropertyName("quality")]
  public QualityDefinition? Quality { get; set; }

  /// <summary>
  /// List of items (for grouped qualities).
  /// </summary>
  [JsonPropertyName("items")]
  public List<QualityProfileItem>? Items { get; set; }

  /// <summary>
  /// Whether this quality is allowed.
  /// </summary>
  [JsonPropertyName("allowed")]
  public bool Allowed { get; set; }
}
