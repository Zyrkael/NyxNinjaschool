using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace NyxNinjaschool.Config
{
    public static class LoggerConfig
    {
        public static void Configure()
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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Map(le => le.Timestamp.ToString("yyyy-MM-dd"), (date, wt) => 
                {
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                        .WriteTo.File(new CompactJsonFormatter(), System.IO.Path.Combine(logDirectory, date, "info.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                        .WriteTo.File(new CompactJsonFormatter(), System.IO.Path.Combine(logDirectory, date, "warning.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
                        .WriteTo.File(new CompactJsonFormatter(), System.IO.Path.Combine(logDirectory, date, "error.log")));
                    
                    wt.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                        .WriteTo.File(new CompactJsonFormatter(), System.IO.Path.Combine(logDirectory, date, "debug.log")));
                })
                .CreateLogger();
        }
    }
}
