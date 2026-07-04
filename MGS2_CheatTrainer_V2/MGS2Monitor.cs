using Newtonsoft.Json.Linq;
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
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_CheatTrainer_V2
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
        private const string DesiredVersion = "2.1.0.0";
        private static bool _versionWarned = false;

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

        #region Threads
        private static void ScanForMGS2()
        {
            while (!_monitorCancellationToken.IsCancellationRequested) //this loop should only end when the program ends.
            {
                Process process = Process.GetProcessesByName(MGS2ProcessName).FirstOrDefault();
                if (process != null)
                {
                    if (MGS2Process != process)
                    {
                        MGS2Process = process;
                        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(MGS2Process.MainModule.FileName);
                        _logger.Debug($"MGS2 found and hooked, version:\n{fileVersion}");
                        if (string.Compare(fileVersion.ProductVersion, DesiredVersion) != 0 && !_versionWarned) 
                        {
                            _versionWarned = true;
                            MessageBox.Show($"The version of MGS2 we have hooked into({fileVersion.ProductVersion}) does not match what we expect({DesiredVersion})! Expect issues if you continue without updating the game.",
                                "Incompatible game version detected!", MessageBoxButtons.OK);
                        }
                    }
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
                else
                {
                    _updateStatsTask.Dispose();
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

        public static bool EnableGameStats { get; set; } = false;

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
            _initialLaunch = false;            
        }
    }
}
