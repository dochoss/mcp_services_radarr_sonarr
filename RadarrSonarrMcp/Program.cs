using System.Text.Json;

namespace RadarrSonarrMcp;

/// <summary>
/// Main entry point for the Radarr/Sonarr MCP Server.
/// Supports both stdio (for Claude Desktop) and HTTP transport modes.
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
      // Determine transport mode from command-line arguments
      bool useHttp = args.Contains("--http") ||
                     args.Contains("--mode=http") ||
                     args.Any(a => a.StartsWith("--mode=") && a.EndsWith("http"));

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
      Console.Error.WriteLine($"  Server IP: {settings.NasConfig.Ip}");
      Console.Error.WriteLine($"  Sonarr URL: {settings.SonarrConfig.GetBaseUrl(settings.NasConfig.Ip)}");
      Console.Error.WriteLine($"  Radarr URL: {settings.RadarrConfig.GetBaseUrl(settings.NasConfig.Ip)}");
      Console.Error.WriteLine($"  Transport Mode: {(useHttp ? "HTTP" : "stdio")}");
      if (useHttp)
      {
        Console.Error.WriteLine($"  HTTP Port: {settings.ServerConfig.Port}");
      }
      Console.Error.WriteLine();

      if (useHttp)
      {
        // HTTP transport mode using WebApplication
        return await RunHttpServerAsync(args, settings);
      }
      else
      {
        // stdio transport mode using Host
        return await RunStdioServerAsync(args, settings);
      }
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error starting server: {ex.Message}");
      Console.Error.WriteLine(ex.StackTrace);
      return 1;
    }
  }

  /// <summary>
  /// Run the MCP server in stdio mode (for Claude Desktop).
  /// </summary>
  private static async Task<int> RunStdioServerAsync(string[] args, AppSettings settings)
  {
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
    RegisterServices(builder.Services, settings);

    // Configure MCP server with stdio transport
    builder.Services
        .AddMcpServer()
        .WithStdioServerTransport()
        .WithToolsFromAssembly();

    Console.Error.WriteLine("Starting stdio MCP server for Claude Desktop...");
    PrintRegisteredTools();

    await builder.Build().RunAsync();

    return 0;
  }

  /// <summary>
  /// Run the MCP server in HTTP mode (for web-based clients).
  /// </summary>
  private static async Task<int> RunHttpServerAsync(string[] args, AppSettings settings)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Configure logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Register services
    RegisterServices(builder.Services, settings);

    // Configure MCP server with HTTP transport
    builder.Services
        .AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly();

    var app = builder.Build();

    // Map MCP endpoints
    app.MapMcp();

    Console.Error.WriteLine("Starting HTTP MCP server...");
    PrintRegisteredTools();

    var url = $"http://localhost:{settings.ServerConfig.Port}";
    Console.Error.WriteLine($"Server listening on: {url}");
    Console.Error.WriteLine();

    await app.RunAsync(url);

    return 0;
  }

  /// <summary>
  /// Register common services used by both transport modes.
  /// </summary>
  private static void RegisterServices(IServiceCollection services, AppSettings settings)
  {
    // Register configuration
    services.AddSingleton(settings);
    services.AddSingleton(settings.SonarrConfig);
    services.AddSingleton(settings.RadarrConfig);

    // Register Sonarr service
    services.AddHttpClient<ISonarrService, SonarrService>((sp, client) =>
    {
      client.Timeout = TimeSpan.FromSeconds(30);
    });
    services.AddSingleton<ISonarrService>(sp =>
    {
      var httpClient = sp.GetRequiredService<HttpClient>();
      var config = sp.GetRequiredService<SonarrConfig>();
      var logger = sp.GetRequiredService<ILogger<SonarrService>>();
      return new SonarrService(httpClient, config, settings.NasConfig.Ip, logger);
    });

    // Register Radarr service
    services.AddHttpClient<IRadarrService, RadarrService>((sp, client) =>
    {
      client.Timeout = TimeSpan.FromSeconds(30);
    });
    services.AddSingleton<IRadarrService>(sp =>
    {
      var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
      var config = sp.GetRequiredService<RadarrConfig>();
      var logger = sp.GetRequiredService<ILogger<RadarrService>>();
      return new RadarrService(httpClient, logger, config, settings.NasConfig.Ip);
    });
  }

  /// <summary>
  /// Print list of registered MCP tools.
  /// </summary>
  private static void PrintRegisteredTools()
  {
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
