using System;
using Serilog.Events;
using Serilog;
using System.IO;

namespace MGS2_CheatTrainer_V2
{
    internal static class Logging
    {
        private const int KilobyteInBytes = 1000;
        private const int MegabyteInKilobytes = 1000 * KilobyteInBytes;
        public static string? LogLocation { get; private set; }
        private static LogEventLevel MainLogEventLevel { get; set; } = LogEventLevel.Information;
        public static ILogger? Logger;
        private static readonly string AppLogFolder = "MGS Mod Manager and Trainer";
        private static readonly string Game = "MGS2";

        public static void StartLogger()
        {
            LogLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppLogFolder, Game);
            Logger = InitializeNewLogger("MGS2_MC_Cheat_Trainer_Log.log", LogEventLevel.Debug);
            Logger?.Information("Logging started");
        }

        private static ILogger? InitializeNewLogger(string logFileName, LogEventLevel loggingLevel)
        {
            if (LogLocation == null) throw new Exception("Failed to initialize logs!");
            if (!Directory.Exists(LogLocation))
            {
                Directory.CreateDirectory(LogLocation);
            }
            return new LoggerConfiguration().WriteTo.File(Path.Combine(LogLocation, logFileName),
                    rollOnFileSizeLimit: false, fileSizeLimitBytes: 50 * MegabyteInKilobytes)
                .MinimumLevel.Is(loggingLevel).CreateLogger();

        }
    }
}
