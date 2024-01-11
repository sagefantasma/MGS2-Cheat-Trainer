using System;
using System.Deployment.Application;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Serilog;

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
            if(ApplicationDeployment.IsNetworkDeployed)
            {
                AppVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                AppVersion = $"MANUAL_INSTALL-{Application.ProductVersion}";
            }

            Logging.MainLogEventLevel = Serilog.Events.LogEventLevel.Information;
            Logging.LogLocation = Path.Combine(new FileInfo(Application.ExecutablePath).DirectoryName, "logs");
            logger = Logging.InitializeNewLogger(loggerName);
            logger.Information($"MGS2 MC Cheat Trainer v.{AppVersion} initialized...");
            logger.Verbose($"Instance ID: {InstanceID}");
            MGS2MemoryManager.StartLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                MGS2Thread.Start();
            } 
            catch (Exception e) 
            {
                logger.Error($"Could not start MGS2 monitor: {e}");
            }

            try
            {
                Thread controllerThread = new Thread(ControllerInterface.EnableInjector);
                controllerThread.Start();
            }
            catch(Exception e)
            {
                logger.Error($"Could not start controller monitor: {e}");
            }

            Application.Run(new GUI());            
        }
    }
}
