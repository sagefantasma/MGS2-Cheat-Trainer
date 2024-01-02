using Serilog.Events;
using Serilog;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace MGS2_MC
{
    internal class Logging
    {
        private const int MegabyteInBytes = 1000;
        public static string LogLocation;

        internal static ILogger InitializeLogger(string logFileName, string loggingLevel = "Information")
        {
            LogEventLevel eventLevel;
            switch (loggingLevel)
            {
                case "Verbose":
                    eventLevel = LogEventLevel.Verbose;
                    break;
                default:
                case "Information":
                    eventLevel = LogEventLevel.Information;
                    break;
                case "Debug":
                    eventLevel = LogEventLevel.Debug;
                    break;
                case "Warning":
                    eventLevel = LogEventLevel.Warning;
                    break;
                case "Error":
                    eventLevel = LogEventLevel.Error;
                    break;
                case "Fatal":
                    eventLevel = LogEventLevel.Fatal;
                    break;
            }
            return new LoggerConfiguration().WriteTo.File(Path.Combine(LogLocation, logFileName), rollOnFileSizeLimit: false, fileSizeLimitBytes: 50 * MegabyteInBytes)
                                              .MinimumLevel.Is(eventLevel).CreateLogger();
        }
    }
}
