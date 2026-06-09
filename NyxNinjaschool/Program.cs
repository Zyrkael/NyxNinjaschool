using Microsoft.Extensions.Configuration;
using NyxNinjaschool.Config;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

// Configure Serilog to separate logs by level
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File("logs/info/info-.txt", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File("logs/warning/warning-.txt", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
        .WriteTo.File("logs/error/error-.txt", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
        .WriteTo.File("logs/debug/debug-.txt", rollingInterval: RollingInterval.Day))
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
