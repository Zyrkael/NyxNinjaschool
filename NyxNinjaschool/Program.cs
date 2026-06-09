using Microsoft.Extensions.Configuration;
using System;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

int port = config.GetValue<int>("ServerConfig:Port");
int maxPlayers = config.GetValue<int>("ServerConfig:MaxPlayers");

Console.WriteLine("========================");
Console.WriteLine("Server Configuration");
Console.WriteLine($"Port: {port}");
Console.WriteLine($"Max Players: {maxPlayers}");
Console.WriteLine("========================");
