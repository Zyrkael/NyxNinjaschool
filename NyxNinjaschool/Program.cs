using Microsoft.Extensions.Configuration;
using System;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

// Class binding
ServerConfig serverConfig = config.GetSection("ServerConfig").Get<ServerConfig>() ?? new ServerConfig();

Console.WriteLine("========================");
Console.WriteLine("Server Configuration (Class Binding)");
Console.WriteLine($"Port: {serverConfig.Port}");
Console.WriteLine($"Max Players: {serverConfig.MaxPlayers}");
Console.WriteLine("========================");

public class ServerConfig
{
    public int Port { get; set; }
    public int MaxPlayers { get; set; }
}
