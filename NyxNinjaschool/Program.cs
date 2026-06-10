using Microsoft.Extensions.Configuration;
using NyxNinjaschool.Config;
using Serilog;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

IConfiguration config = builder.Build();

// Configure Serilog to separate logs by level
LoggerConfig.Configure(LogMode.Text);

try
{
    Log.Information("Starting Nyx Ninjaschool Server...");

    // Class binding
    ServerConfig serverConfig = config.GetSection("ServerConfig").Get<ServerConfig>() ?? new ServerConfig();

    Log.Information("Server Configuration Loaded");
    Log.Information("Port: {Port}", serverConfig.Port);
    Log.Information("Max Players: {MaxPlayers}", serverConfig.MaxPlayers);

    Log.Information("Initialization complete.");

    Log.Information("Press Enter to exit...");
    Console.ReadLine();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
