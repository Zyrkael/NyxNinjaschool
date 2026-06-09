# Nyx Ninjaschool Server

Nyx Ninjaschool Server is a .NET 8.0 based server application for Ninjaschool.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker (optional, for containerized deployment)

## Configuration

Server configuration can be found and modified in `appsettings.json` under the `ServerConfig` section:

- `Port`: The port the server will listen on.
- `MaxPlayers`: Maximum number of players allowed.

## Running the Server

To build and run the server locally, execute the following commands in the project directory:

```bash
cd NyxNinjaschool
dotnet build
dotnet run
```

## Logging

The project uses Serilog. Logs are output to the console and also separated by log level into daily rolling files under the `logs/` directory:
- `info/`
- `warning/`
- `error/`
- `debug/`
