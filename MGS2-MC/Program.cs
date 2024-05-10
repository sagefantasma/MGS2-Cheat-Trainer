using System;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
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
            Task.Run(() =>
            {
                if (IsRunAsAdministrator())
                {
                    while (GUI.GuiLoaded == false)
                    {
                        //wait for the GUI to load
                    }
                    _logger.Debug("Cheat Trainer started in admin mode! Starting on cheats tab.");
                    (GUI.StaticGuiReference.Controls["mgs2TabControl"] as TabControl).SelectTab("tabPageCheats");
                    GUI.StaticGuiReference.Name += " (ADMIN MODE)";
                }
            });
            InitializeTrainer();
        }

        public static void RestartInAdminMode()
        {
            _logger.Debug("User isn't in admin mode. Asking to restart in admin mode");

            string message = "Not all cheats work without admin mode. Would you like to restart in Admin mode?";
            string caption = "Limited functionality ahead!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult userChoice = MessageBox.Show(message, caption, buttons);
            switch (userChoice)
            {
                case DialogResult.Yes:
                    var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase)
                    {
                        UseShellExecute = true,
                        Verb = "runas"
                    };

                    try
                    {
                        _logger.Debug("Restarting process with UAC prompt");
                        Process.Start(processInfo);
                    }
                    catch
                    {
                        _logger.Error("User declined UAC prompt.");
                    }
                    Application.Exit();
                    break;
                default:
                case DialogResult.No:
                    _logger.Debug("User chose to not restart in admin mode.");
                    break;
            }
        }

        private static void InitializeTrainer()
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

        public static bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
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
