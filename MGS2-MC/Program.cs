using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using Serilog;
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_MC
{
    internal static class Program
    {
        private static ILogger logger;
        private const string loggerName = "MGS2CheatTrainerMainDebuglog.log";
        internal static Process MGS2Process;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logging.LogLocation = new FileInfo(Application.ExecutablePath).DirectoryName;
            logger = Logging.InitializeLogger(loggerName);
            MGS2MemoryManager.StartLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                TrainerConfig trainerConfig = LoadConfig();
                if (trainerConfig.AutoLaunchGame)
                {
                    Thread mgs2Thread = new Thread(() => Mgs2Monitor(trainerConfig));
                    mgs2Thread.Start();
                }
            } 
            catch (Exception e) 
            {
                logger.Error($"Could not start MGS2 monitor: {e}");
            }

            try
            {
                Thread controllerThread = new Thread(() => MGS2Injector.EnableInjector());
                controllerThread.Start();
            }
            catch(Exception e)
            {
                logger.Error($"Could not start controller monitor: {e}");
            }

            Application.Run(new GUI());            
        }

        private static TrainerConfig LoadConfig()
        {
            try
            {
                return JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText("TrainerConfig.json"));
            }
            catch(Exception e)
            {
                logger.Error($"Failed to load TrainerConfig.json: {e}");
                return null;
            }
        }

        /// <summary>
        /// !!!NOTE!!! This WILL NOT WORK if you are running this program in a debugger and use the "Stop Debugging" feature.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseMGS2EventHandler(object sender, EventArgs e)
        {
            MGS2Process?.CloseMainWindow();
            MGS2Process?.Dispose();
        }


        private static bool IsDirectLaunchEnabled(FileInfo steamAppIdFile)
        {
            if (steamAppIdFile.Exists)
            {
                try
                {
                    string steamAppIdFileContents = File.ReadAllText(steamAppIdFile.FullName);
                    if (steamAppIdFileContents.Equals(MGS2Constants.SteamAppId))
                    {
                        //the steam_appid file exists and has the appropriate content
                        return true;
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"Failed to read the {steamAppIdFile.Name} file in your MGS2 directory, cannot guarantee direct launch will work: {e}");
                    return false;
                }
            }

            return false;
        }

        private static void EnableDirectLaunch(FileInfo steamAppIdFile)
        {
            try
            {
                //the steam_appid file is either missing, or has incorrect contents. overwrite it so we can direct launch w/o issue.
                using (StreamWriter writer = new StreamWriter(File.Open(steamAppIdFile.FullName, FileMode.Create)))
                {
                    writer.WriteLine(MGS2Constants.SteamAppId);
                }
            }
            catch(Exception e)
            {
                logger.Error($"Failed to enable direct launch capabilities: {e}");
                //as of right now, I don't know if it will work without this shortcut, so i won't throw yet.
            }
        }

        private static void StartMgs2(string mgs2Location, string mgs2Directory, TrainerConfig config)
        {
            
            ProcessStartInfo mgs2StartInfo = new ProcessStartInfo(mgs2Location)
            {
                WorkingDirectory = mgs2Directory,
                UseShellExecute = false,
                CreateNoWindow = false,
            };
            MGS2Process.StartInfo = mgs2StartInfo;
            try
            {
                MGS2Process.Start();
            }
            catch (Exception e)
            {
                logger.Error($"Failed to start MGS2: {e}");
                throw new AggregateException("Failed to start MGS2.", e);
            }

            if (config.CloseGameWithTrainer)
            {
                Application.ApplicationExit += (sender, args) => CloseMGS2EventHandler(sender, args);
            }

            try
            {
                while (!MGS2Process.HasExited)
                {
                    //this thread loops forever while MGS2 is running
                }
            }
            catch(Exception e)
            {
                logger.Error($"Something went wrong with the MGS2 process: {e}");
            }

            if (config.CloseTrainerWithGame)
            {
                Application.Exit();
            }
            
        }

        private static void Mgs2Monitor(TrainerConfig trainerConfig)
        {
            MGS2Process = new Process();
            try
            {
                //mgs2.StartInfo = new ProcessStartInfo("steam://rungameid/2131640"); //leaving this here in case it makes sense to use this method instead for some reason
                string mgs2Executable = trainerConfig.Mgs2ExePath;
                string mgs2Directory = new FileInfo(mgs2Executable).DirectoryName;
                FileInfo steamAppIdFile = new FileInfo(mgs2Directory + MGS2Constants.SteamAppIdFileName);
                bool directLaunchEnabled = IsDirectLaunchEnabled(steamAppIdFile);                

                if (!directLaunchEnabled)
                {
                    EnableDirectLaunch(steamAppIdFile);   
                }

                StartMgs2(mgs2Executable, mgs2Directory, trainerConfig);
            }
            catch(Exception e)
            {
                logger.Error($"Something went wrong inside the MGS2 monitor: {e}");
            }
        }
    }
}
