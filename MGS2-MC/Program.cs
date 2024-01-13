using System;
using System.Configuration;
using System.Deployment.Application;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Serilog;
using Serilog.Events;

namespace MGS2_MC
{
    internal static class Program
    {
        internal static Thread MGS2Thread = new Thread(MGS2Monitor.EnableMonitor);
        private static ILogger logger;
        private const string loggerName = "MainDebuglog.log";
        public static string AppVersion;
        public static string InstanceID = InstanceIdentifier.CreateInstanceIdentifier();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetAppVersion();
            InitializeLogs();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            StartMonitoringThread();
            StartControllerThread();

            Application.Run(new GUI(logger));            
        }

        private static void StartMonitoringThread()
        {
            try
            {
                MGS2Thread.Start();
            }
            catch (Exception e)
            {
                logger.Error($"Could not start MGS2 monitor: {e}");
            }
        }

        public static void RestartMonitoringThread()
        {
            try
            {
                MGS2Thread = new Thread(MGS2Monitor.EnableMonitor);
                MGS2Thread.Start();
            }
            catch(Exception e)
            {
                logger.Error($"Could not restart MGS2 monitor: {e}");
            }
        }

        private static void StartControllerThread()
        {
            try
            {
                Thread controllerThread = new Thread(ControllerInterface.EnableInjector);
                controllerThread.Start();
            }
            catch (Exception e)
            {
                logger.Error($"Could not start controller monitor: {e}");
            }
        }

        private static void SetAppVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                AppVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                AppVersion = $"MANUAL_INSTALL-{Application.ProductVersion}";
            }
        }

        private static void InitializeLogs()
        {
            string logLevel;
            try
            {
                logLevel = ConfigurationManager.AppSettings.Get("LogLevel");
            }
            catch
            {
                logLevel = "Information";
            }
            Logging.MainLogEventLevel = ParseLogEventLevel(logLevel);
            Logging.LogLocation = Path.Combine(new FileInfo(Application.ExecutablePath).DirectoryName, "logs");
            logger = Logging.InitializeNewLogger(loggerName);
            logger.Information($"MGS2 MC Cheat Trainer v.{AppVersion} initialized...");
            logger.Verbose($"Instance ID: {InstanceID}");
            MGS2MemoryManager.StartLogger();
        }

        private static LogEventLevel ParseLogEventLevel(string logLevel)
        {
            switch (logLevel)
            {
                case "Verbose":
                    return LogEventLevel.Verbose;
                default:
                case "Information":
                    return LogEventLevel.Information;
                case "Debug":
                    return LogEventLevel.Debug;
                case "Error":
                    return LogEventLevel.Error;
                case "Warning":
                    return LogEventLevel.Warning;
                case "Fatal":
                    return LogEventLevel.Fatal;
            }
        }
    }
}
