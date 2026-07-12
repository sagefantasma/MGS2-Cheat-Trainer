using System;
using Serilog.Events;
using Serilog;
using System.IO;

namespace MGS2_CheatTrainer_V2
{
    //REWRITE STATUS: Need to update to work with new service structure.
    internal static class Logging
    {
        private const int KilobyteInBytes = 1000;
        private const int MegabyteInKilobytes = 1000 * KilobyteInBytes;
        private static string? LogLocation { get; set; }
        private static LogEventLevel MainLogEventLevel { get; set; } = LogEventLevel.Information;
        public static ILogger? Logger;

        public static void StartLogger()
        {
            LogLocation = Environment.CurrentDirectory;
            Logger = InitializeNewLogger("MGS2CheatTrainerDebuglog.log", LogEventLevel.Debug);
            Logger.Information("Logging started");
        }

        internal static ILogger InitializeNewLogger(string logFileName)
        {
            return InitializeNewLogger(logFileName, MainLogEventLevel);
        }

        internal static ILogger InitializeNewLogger(string logFileName, LogEventLevel loggingLevel)
        {
            return new LoggerConfiguration().WriteTo.File(Path.Combine(LogLocation, logFileName), rollOnFileSizeLimit: false, fileSizeLimitBytes: 50 * MegabyteInKilobytes)
                                              .MinimumLevel.Is(loggingLevel).CreateLogger();
        }
    }
}
