using Microsoft.Extensions.Configuration;
using NyxNinjaschool.Config;
using Serilog;
using System;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/nyx-ninja-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Nyx Ninjaschool Server...");

    // Class binding
    ServerConfig serverConfig = config.GetSection("ServerConfig").Get<ServerConfig>() ?? new ServerConfig();

    Log.Information("Server Configuration Loaded");
    Log.Information("Port: {Port}", serverConfig.Port);
    Log.Information("Max Players: {MaxPlayers}", serverConfig.MaxPlayers);
    
    Log.Information("Initialization complete.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
