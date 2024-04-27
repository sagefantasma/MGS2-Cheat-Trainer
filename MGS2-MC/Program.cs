using System;
using System.Configuration;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Serilog;
using Serilog.Events;

namespace MGS2_MC
{
    internal static class Program
    {
        private const string loggerName = "MainDebuglog.log";

        public static string InstanceID { get; } = InstanceIdentifier.CreateInstanceIdentifier();
        internal static Thread MGS2Thread { get; set; } = new Thread(() => MGS2Monitor.EnableMonitor(_cancellationTokenSource.Token));
        internal static Thread ControllerThread { get; set; }
        public static string AppVersion { get; set; }
        private static ILogger _logger { get; set; }
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

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

            Application.ApplicationExit += AppEnding;
            Application.Run(new GUI(_logger));
        }

        private static void AppEnding(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
            if (MGS2Monitor.TrainerConfig.CloseGameWithTrainer)
            {
                MGS2Thread.Abort();
            }
            ControllerThread.Abort();
        }

        private static void StartMonitoringThread()
        {
            try
            {
                MGS2Thread.Name = "Initial MGS2 Monitor";
                _logger.Information("Initializing MGS2 monitor for the first time");
                MGS2Thread.Start();
            }
            catch (Exception e)
            {
                _logger.Error($"Could not start MGS2 monitor: {e}");
            }
        }

        public static void RestartMonitoringThread()
        {
            try
            {
                MGS2Thread = new Thread(() => MGS2Monitor.EnableMonitor(_cancellationTokenSource.Token))
                {
                    Name = "Restarted MGS2 Monitor"
                };
                _logger.Information("Re-initializing MGS2 monitor");
                MGS2Thread.Start();
            }
            catch(Exception e)
            {
                _logger.Error($"Could not restart MGS2 monitor: {e}");
            }
        }

        private static void StartControllerThread()
        {
            try
            {
                ControllerThread = new Thread(() => ControllerInterface.EnableInjector(_cancellationTokenSource.Token));
                ControllerThread.Start();
            }
            catch (Exception e)
            {
                _logger.Error($"Could not start controller monitor: {e}");
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
                AppVersion = $"Manual install of {Assembly.GetExecutingAssembly().GetName().Version}";
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
                logLevel = "Debug";
            }
            Logging.MainLogEventLevel = ParseLogEventLevel(logLevel);
            Logging.LogLocation = Path.Combine(new FileInfo(Application.ExecutablePath).DirectoryName, "logs");
            _logger = Logging.InitializeNewLogger(loggerName);
            _logger.Information($"MGS2 MC Cheat Trainer v.{AppVersion} initialized...");
            _logger.Verbose($"Instance ID: {InstanceID}");
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
