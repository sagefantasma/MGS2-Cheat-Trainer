using MGS2_MC.Helpers;
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
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_MC
{
    internal static class MGS2Monitor
    {
        #region Internals
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
            catch (Exception e)
            {
                _logger.Error($"Failed to suspend MGS2: {e}");
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
            catch (Exception e)
            {
                _logger.Error($"Failed to resume MGS2: {e}");
            }
        }
        #endregion

        #region Private members, fields, and functions
        #region Members & fields
        private const string loggerName = "MGS2MonitorDebuglog.log";
        private const string MGS2ProcessName = "METAL GEAR SOLID2";

        private static Process _mgs2Process;

        private static bool _initialLaunch { get; set; } = true;
        private static CancellationToken _monitorCancellationToken { get; set; }
        private static CancellationTokenSource _mgs2CancellationTokenSource { get; set; } = new CancellationTokenSource();
        private static ILogger _logger { get; set; }
        private static Thread _scanningThread { get; set; }
        private static Task _updateStatsTask { get; set; }
        private static Stage _lastKnownStage { get; set; }
        #endregion

        #region Functions
        #region Event Handlers & Delegates
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
            catch (Exception ex)
            {
                _logger.Error($"Failed to close MGS2: {ex}");
            }
        }

        private static void TearDownMonitor()
        { 
            _scanningThread.Abort();
        }
        #endregion

        #region Launch-related Functions
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
                    _logger.Error($"Failed to read the {steamAppIdFile.Name} file in your MGS2 directory, cannot guarantee direct launch will work: {e}");
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
                _logger.Error($"Failed to enable direct launch capabilities: {e}");
                //as of right now, I don't know if it will work without this shortcut, so i won't throw yet.
            }
        }
        
        private static void PrepareAndRunMGS2()
        {
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
                _logger.Error($"Something went wrong inside the MGS2 monitor: {e}");
            }
        }

        private static void RunMGS2(string mgs2Location, string mgs2Directory)
        {
            ProcessStartInfo mgs2StartInfo = new ProcessStartInfo(mgs2Location)
            {
                WorkingDirectory = mgs2Directory,
                UseShellExecute = false,
                CreateNoWindow = false,
            };
            
            try
            {
                MGS2Process = Process.Start(mgs2StartInfo);
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to start MGS2: {e}");
                throw new AggregateException("Failed to start MGS2.", e);
            }

            if (TrainerConfig.CloseGameWithTrainer)
            {
                Application.ApplicationExit += (sender, args) => CloseMGS2EventHandler(sender, args);
            }
            while (GUI.GuiLoaded != true)
            {
                //if the GUI is loading, wait until it is fully loaded until we continue as we need to modify it
            }
            try
            {
                GUI.EnableLaunchMGS2Option(false);
                while (!MGS2Process.HasExited || !_monitorCancellationToken.IsCancellationRequested)
                {
                    //this thread loops forever while MGS2 is running, but can be cancelled by the master cancellation token
                }
            }
            catch (NullReferenceException)
            {
                _logger.Error($"The MGS2 process was successfully launched by the trainer, but seems to have exited.");
            }
            catch (Exception e)
            {
                _logger.Error($"Something went wrong with the MGS2 process: {e}");
            }

            if (TrainerConfig.CloseTrainerWithGame)
            {
                Application.Exit();
            }

            try
            {
                if (MGS2Process == null || MGS2Process.HasExited)
                    GUI.EnableLaunchMGS2Option(true);
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to enable Launch MGS2 menu option: {e}");
            }
        }
        #endregion

        #region Threads
        private static void ScanForMGS2()
        {
            while (!_monitorCancellationToken.IsCancellationRequested) //this loop should only end when the program ends.
            {
                Process process = Process.GetProcessesByName(MGS2ProcessName).FirstOrDefault();
                if (process != null)
                {
                    if (MGS2Process != process)
                        MGS2Process = process;
                    Thread.Sleep(60 * Constants.MillisecondsInSecond); //scan every 60 seconds to see if MGS2 is still running
                }
                else
                {
                    MGS2Process = null;
                    Thread.Sleep(10 * Constants.MillisecondsInSecond); //scan every 10 seconds if we know MGS2 IS NOT running
                }
            }
        }

        #region In-game Stats
        
        private static async Task MonitorScoringStats()
        {
            _mgs2CancellationTokenSource = new CancellationTokenSource();
            CancellationToken mgs2CancellationToken = _mgs2CancellationTokenSource.Token;
            await PeriodicTask.Run(UpdateScoringStats, TimeSpan.FromSeconds(1), mgs2CancellationToken);   
        }

        private static void UpdateScoringStats()
        {
            try
            {
                if (EnableGameStats)
                {
                    Stage currentStage = MGS2MemoryManager.GetStage();
                    if(currentStage?.Name != _lastKnownStage?.Name)
                    {
                        _logger.Debug($"User is now in stage: {currentStage}");
                        _lastKnownStage = currentStage;
                    }
                    //if we're in a main menu, we shouldn't try to find stats right now.
                    if (!StageNames.MenuStages.StageList.Contains(currentStage))
                    {
                        MGS2MemoryManager.GameStats currentGameStats = MGS2MemoryManager.ReadGameStats();
                        Difficulty currentDifficulty = MGS2MemoryManager.ReadCurrentDifficulty();
                        //GameType currentGameType = MGS2MemoryManager.ReadGameType(); //TODO: finish determining how to determine what gametype we're in
                        GUI.StaticGuiReference.UpdateGameStats(currentGameStats, currentDifficulty);
                    }
                }
            }
            catch(Exception e)
            {
                if (_mgs2Process != null)
                {
                    //only write to log when we are actually in a game, and should have some stats to grab
                    _logger.Error($"Failed to update scoring stats! Error encountered: {e}");
                }
            }
        }
        #endregion
        #endregion
        #endregion
        #endregion

        #region Constructor & Process Encapsulator
        static MGS2Monitor()
        {
            _logger = Logging.InitializeNewLogger(loggerName);
            _logger.Information($"MGS2 Monitor for version {Program.AppVersion} initialized...");
            _logger.Verbose($"Instance ID: {Program.InstanceID}");
        }

        public static bool EnableGameStats { get; set; } = true;

        public static Process MGS2Process
        {
            get 
            {
                if (_mgs2Process != null && _mgs2Process.HasExited == false)
                {
                    return _mgs2Process;
                }

                try
                {
                    _mgs2CancellationTokenSource.Cancel();
                }
                catch 
                {
                    //if this fails, its not a big deal
                }
                _mgs2Process = null;
                return null;
            }
            set
            {
                //start tasks to monitor in-game values
                _updateStatsTask = Task.Factory.StartNew(MonitorScoringStats);
                if (GUI.GuiLoaded == true)
                    GUI.EnableLaunchMGS2Option(value == null); //enable when process is null, disable otherwise
                _mgs2Process = value;
            }
        }
        #endregion

        #region Config
        internal static TrainerConfig TrainerConfig { get; set; }        

        internal static TrainerConfig LoadConfig()
        {
            try
            {
                return JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText(TrainerConfigFileLocation));
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to load TrainerConfig.json: {e}");
                return null;
            }
        }
        #endregion
        #endregion

        internal static void EnableMonitor(CancellationToken cancellationToken)
        {
            _monitorCancellationToken = cancellationToken;
            _monitorCancellationToken.Register(TearDownMonitor);
            _logger.Information("Starting MGS2 scanning thread...");
            _scanningThread = new Thread(() => ScanForMGS2())
            {
                Name = "MGS2 Scanning Thread"
            };
            _scanningThread.Start();
            TrainerConfig = LoadConfig();

            if ((TrainerConfig.AutoLaunchGame && _initialLaunch) || (!_initialLaunch))
            {
                //Launch if first launch and AutoLaunch is enabled
                //Otherwise, launch if this is not the initial launch.
                PrepareAndRunMGS2();
            }
            _initialLaunch = false;            
        }
    }
}
