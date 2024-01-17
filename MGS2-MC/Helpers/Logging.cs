using Serilog.Events;
using Serilog;
using System.IO;

namespace MGS2_MC
{
    internal abstract class Logging
    {
        private const int KilobyteInBytes = 1000;
        private const int MegabyteInKilobytes = 1000 * KilobyteInBytes;
        public static string LogLocation;
        public static LogEventLevel MainLogEventLevel = LogEventLevel.Information;

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
