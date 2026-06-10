using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;

namespace NyxNinjaschool.Config
{
    public enum LogMode
    {
        Text,
        Json
    }

    public static class LoggerConfig
    {
        public static void Configure(LogMode logMode = LogMode.Text)
        {
            string logDirectory = "logs";
#if DEBUG
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDir.Contains("bin\\Debug") || baseDir.Contains("bin/Debug"))
            {
                // Go up 3 levels from bin/Debug/net8.0 to get to the project root
                logDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", "logs"));
            }
#endif

            ITextFormatter formatter = logMode == LogMode.Json 
                ? new CompactJsonFormatter() 
                : new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Map(le => le.Timestamp.ToString("yyyy-MM-dd"), (date, wt) => 
                {
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                        .WriteTo.File(formatter, System.IO.Path.Combine(logDirectory, date, "info.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                        .WriteTo.File(formatter, System.IO.Path.Combine(logDirectory, date, "warning.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
                        .WriteTo.File(formatter, System.IO.Path.Combine(logDirectory, date, "error.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                        .WriteTo.File(formatter, System.IO.Path.Combine(logDirectory, date, "debug.log")));
                })
                .CreateLogger();
        }
    }
}
