using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MGS2_MC.ControllerInterface;
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_MC
{
    internal static class MGS2Monitor
    {
        #region Native Methods
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        /*[DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);*/ //this may be useful for slapping the GUI on top of MGS2
        #endregion

        static MGS2Monitor()
        {
            logger = Logging.InitializeNewLogger(loggerName);
            logger.Information($"MGS2 Monitor for version {Program.AppVersion} initialized...");
            logger.Verbose($"Instance ID: {Program.InstanceID}");
        }

        public static Process MGS2Process;
        internal static TrainerConfig TrainerConfig;
        private const string loggerName = "MGS2MonitorDebuglog.log";
        private static readonly ILogger logger;
        private static bool initialLaunch = true;

        internal static TrainerConfig LoadConfig()
        {
            try
            {
                return JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText("TrainerConfig.json"));
            }
            catch (Exception e)
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
            try
            {
                MGS2Process?.CloseMainWindow();
                MGS2Process?.Dispose();
            }
            catch(Exception ex)
            {
                logger.Error($"Failed to close MGS2: {ex}");
            }
        }

        private static bool IsDirectLaunchEnabled(FileInfo steamAppIdFile)
        {
            if (steamAppIdFile.Exists)
            {
                try
                {
                    string steamAppIdFileContents = File.ReadAllText(steamAppIdFile.FullName);
                    if (steamAppIdFileContents.Equals(Constants.SteamAppId))
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
                    writer.WriteLine(Constants.SteamAppId);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Failed to enable direct launch capabilities: {e}");
                //as of right now, I don't know if it will work without this shortcut, so i won't throw yet.
            }
        }

        private static void RunMGS2(string mgs2Location, string mgs2Directory)
        {
            //TODO: it seems like this is resulting in MGS2 taking up TWICE the CPU load, which does kind of make sense...
            //the BIG problem though is that Windows Antivirus is now flagging the Cheat Trainer as malware and also wrapping
            //the cheat trainer & mgs2 up in CPU cycles... which essentially means starting MGS2 through the CT is resulting
            //in your processor going under the load of 3 copies of MGS2... Not great. Can be fixed through adjusting malware
            //settings, but that's not a great solution. i need to reevaluate whether or not i absolutely NEED to be monitoring
            //MGS2 like a hawk like we are now, simply for the closing events...
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

            if (TrainerConfig.CloseGameWithTrainer)
            {
                Application.ApplicationExit += (sender, args) => CloseMGS2EventHandler(sender, args);
            }
            while(GUI.GuiLoaded != true)
            {
                //if the GUI is loading, wait until it is fully loaded until we continue as we need to modify it
            }
            try
            {
                GUI.ToggleLaunchMGS2Option();
                while (!MGS2Process.HasExited)
                {
                    //this thread loops forever while MGS2 is running
                }
            }
            catch (Exception e)
            {
                logger.Error($"Something went wrong with the MGS2 process: {e}");
            }

            if (TrainerConfig.CloseTrainerWithGame)
            {
                Application.Exit();
            }

            try
            {
                GUI.ToggleLaunchMGS2Option();
            }
            catch(Exception e)
            {
                logger.Error($"Failed to toggle Launch MGS2 menu option: {e}");
            }
        }

        internal static void EnableMonitor()
        {
            TrainerConfig = LoadConfig();

            if (!TrainerConfig.AutoLaunchGame && initialLaunch)
            {
                initialLaunch = false;
                return;
            }
            initialLaunch = false;

            MGS2Process = new Process();
            try
            {
                //mgs2.StartInfo = new ProcessStartInfo("steam://rungameid/2131640"); //leaving this here in case it makes sense to use this method instead for some reason
                string mgs2Executable = TrainerConfig.Mgs2ExePath;
                string mgs2Directory = new FileInfo(mgs2Executable).DirectoryName;
                FileInfo steamAppIdFile = new FileInfo(mgs2Directory + Constants.SteamAppIdFileName);
                bool directLaunchEnabled = IsDirectLaunchEnabled(steamAppIdFile);

                if (!directLaunchEnabled)
                {
                    EnableDirectLaunch(steamAppIdFile);
                }

                RunMGS2(mgs2Executable, mgs2Directory);
            }
            catch (Exception e)
            {
                logger.Error($"Something went wrong inside the MGS2 monitor: {e}");
            }
        }

        internal static void SuspendMGS2()
        {
            //https://stackoverflow.com/a/71457 for how to do this
            try
            {
                foreach (ProcessThread mgs2Thread in MGS2Process?.Threads)
                {
                    IntPtr mgs2OpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)mgs2Thread.Id);

                    if (mgs2OpenThread == IntPtr.Zero)
                    {
                        continue;
                    }

                    SuspendThread(mgs2OpenThread);
                    CloseHandle(mgs2OpenThread);
                }
            }
            catch(Exception e)
            {
                logger.Error($"Failed to suspend MGS2: {e}");
            }
        }

        internal static void ResumeMGS2()
        {
            try
            {
                //https://stackoverflow.com/a/71457 for how to do this
                foreach (ProcessThread mgs2Thread in MGS2Process?.Threads)
                {
                    IntPtr mgs2OpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)mgs2Thread.Id);

                    if (mgs2OpenThread == IntPtr.Zero)
                    {
                        continue;
                    }

                    int suspendCount;
                    do
                    {
                        suspendCount = ResumeThread(mgs2OpenThread);
                    } while (suspendCount > 0);

                    CloseHandle(mgs2OpenThread);
                }
            }
            catch(Exception e)
            {
                logger.Error($"Failed to resume MGS2: {e}");
            }
        }
    }
}
