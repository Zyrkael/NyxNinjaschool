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

The project uses Serilog. Logs are output to the console and also separated by date and log level into daily rolling files under the `logs/` directory. The structure is `logs/yyyy-MM-dd/<level>.log` (e.g., `info.log`, `warning.log`, `error.log`, `debug.log`).

Logging configuration supports both Text and JSON modes via `LoggerConfig`. The `NinjaLogUtils` class is provided as a static wrapper for easy and standardized logging across the system.

## Utilities

The project includes several utility classes under the `NyxNinjaschool.Utils` namespace:

- **`NinjaLogUtils`**: A static wrapper for Serilog, providing simple and consistent logging methods (`Info`, `Warn`, `Error`, `Debug`, `Fatal`).
- **`TimeUtils`**: Handles time-related logic, such as getting the current time in milliseconds since the Unix epoch and checking elapsed time for cooldowns or delays.
- **`ProgressBarUtils`**: Renders an animated and interactive console progress bar.
- **`StringUtils`**: Provides string manipulation methods, including string formatting with reflection caching (`${Property.SubProperty}`), Vietnamese accent removal, character repetition, and BCrypt password verification.
- **`NinjaUtils`**: Provides thread-safe random generation methods (`NextInt`, `NextDouble`, `RandomElement`) utilizing `Random.Shared`.
