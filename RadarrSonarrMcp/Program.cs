using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using RadarrSonarrMcp.Configuration;
using RadarrSonarrMcp.Models;
using RadarrSonarrMcp.Services;

namespace RadarrSonarrMcp;

/// <summary>
/// Main entry point for the Radarr/Sonarr MCP Server.
/// </summary>
class Program
{
  static async Task<int> Main(string[] args)
  {
    Console.Error.WriteLine("Radarr/Sonarr MCP Server");
    Console.Error.WriteLine("========================");
    Console.Error.WriteLine();

    try
    {
      // Load configuration
      var settings = LoadConfiguration();
      if (settings == null)
      {
        Console.Error.WriteLine("Failed to load configuration. Exiting.");
        return 1;
      }

      // Validate Sonarr configuration
      if (!settings.SonarrConfig.IsValid())
      {
        Console.Error.WriteLine("Sonarr configuration is invalid. Please check your config.json file.");
        return 1;
      }

      // Validate Radarr configuration
      if (!settings.RadarrConfig.IsValid())
      {
        Console.Error.WriteLine("Radarr configuration is invalid. Please check your config.json file.");
        return 1;
      }

      Console.Error.WriteLine($"Loaded configuration:");
      Console.Error.WriteLine($"  NAS IP: {settings.NasConfig.Ip}");
      Console.Error.WriteLine($"  Sonarr URL: {settings.SonarrConfig.GetBaseUrl(settings.NasConfig.Ip)}");
      Console.Error.WriteLine($"  Radarr URL: {settings.RadarrConfig.GetBaseUrl(settings.NasConfig.Ip)}");
      Console.Error.WriteLine();

      // Create host with MCP server
      var builder = Host.CreateApplicationBuilder(args);

      // Configure logging to stderr (MCP requirement)
      builder.Logging.ClearProviders();
      builder.Logging.AddConsole(options =>
      {
        options.LogToStandardErrorThreshold = LogLevel.Trace;
      });
      builder.Logging.SetMinimumLevel(LogLevel.Information);

      // Register services
      builder.Services.AddSingleton(settings);
      builder.Services.AddSingleton(settings.SonarrConfig);
      builder.Services.AddSingleton(settings.RadarrConfig);

      // Register Sonarr service
      builder.Services.AddHttpClient<ISonarrService, SonarrService>((sp, client) =>
      {
        client.Timeout = TimeSpan.FromSeconds(30);
      });
      builder.Services.AddSingleton<ISonarrService>(sp =>
      {
        var httpClient = sp.GetRequiredService<HttpClient>();
        var config = sp.GetRequiredService<SonarrConfig>();
        var logger = sp.GetRequiredService<ILogger<SonarrService>>();
        return new SonarrService(httpClient, config, settings.NasConfig.Ip, logger);
      });

      // Register Radarr service
      builder.Services.AddHttpClient<IRadarrService, RadarrService>((sp, client) =>
      {
        client.Timeout = TimeSpan.FromSeconds(30);
      });
      builder.Services.AddSingleton<IRadarrService>(sp =>
      {
        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
        var config = sp.GetRequiredService<RadarrConfig>();
        var logger = sp.GetRequiredService<ILogger<RadarrService>>();
        return new RadarrService(httpClient, logger, config, settings.NasConfig.Ip);
      });

      // Configure MCP server
      builder.Services
          .AddMcpServer()
          .WithStdioServerTransport()
          .WithToolsFromAssembly();

        Console.Error.WriteLine("Starting stdio MCP server for Claude Desktop...");
        Console.Error.WriteLine("Registered Sonarr tools:");
        Console.Error.WriteLine("  - sonarr_get_all_series");
        Console.Error.WriteLine("  - sonarr_get_series");
        Console.Error.WriteLine("  - sonarr_add_series");
        Console.Error.WriteLine("  - sonarr_update_series");
        Console.Error.WriteLine("  - sonarr_get_episodes");
        Console.Error.WriteLine("  - sonarr_get_missing_episodes");
        Console.Error.WriteLine("  - sonarr_execute_command");
        Console.Error.WriteLine("  - sonarr_get_queue");
        Console.Error.WriteLine("  - sonarr_get_system_status");
        Console.Error.WriteLine("  - sonarr_get_root_folders");
        Console.Error.WriteLine("  - sonarr_get_quality_profiles");
        Console.Error.WriteLine();
        Console.Error.WriteLine("Registered Radarr tools:");
        Console.Error.WriteLine("  - radarr_get_all_movies");
        Console.Error.WriteLine("  - radarr_get_movie_by_id");
        Console.Error.WriteLine("  - radarr_search_movies");
        Console.Error.WriteLine("  - radarr_add_movie");
        Console.Error.WriteLine("  - radarr_update_movie");
        Console.Error.WriteLine("  - radarr_delete_movie");
        Console.Error.WriteLine("  - radarr_get_queue");
        Console.Error.WriteLine("  - radarr_get_missing_movies");
        Console.Error.WriteLine("  - radarr_execute_command");
        Console.Error.WriteLine("  - radarr_get_system_status");
        Console.Error.WriteLine("  - radarr_get_root_folders");
        Console.Error.WriteLine("  - radarr_get_quality_profiles");
        Console.Error.WriteLine();

      await builder.Build().RunAsync();

      return 0;
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error starting server: {ex.Message}");
      Console.Error.WriteLine(ex.StackTrace);
      return 1;
    }
  }

  /// <summary>
  /// Load configuration from config.json file.
  /// </summary>
  private static AppSettings? LoadConfiguration()
  {
    try
    {
      var configPath = Path.Combine(AppContext.BaseDirectory, "config.json");
      if (!File.Exists(configPath))
      {
        Console.Error.WriteLine($"Configuration file not found at: {configPath}");
        Console.Error.WriteLine("Please create a config.json file. See config.example.json for reference.");
        return null;
      }

      var json = File.ReadAllText(configPath);
      var settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });

      return settings;
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error loading configuration: {ex.Message}");
      return null;
    }
  }
}
