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
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using SimplifiedMemoryManager;

namespace MGS2_CheatTrainer_V2
{
    internal static class Mgs2Monitor
    {
        #region Private members, fields, and functions
        #region Members & fields
        private const string LoggerName = "MGS2MonitorDebuglog.log";
        private const string Mgs2ProcessName = "METAL GEAR SOLID2";
        private const string DesiredVersion = "2.1.0.0";
        private static bool _versionWarned = false;

        private static Process _mgs2Process;

        private static bool InitialLaunch { get; set; } = true;
        private static CancellationToken MonitorCancellationToken { get; set; }
        private static CancellationTokenSource Mgs2CancellationTokenSource { get; set; } = new CancellationTokenSource();
        private static ILogger Logger { get; set; }
        private static Thread ScanningThread { get; set; }
        private static Task UpdateStatsTask { get; set; }
        private static Stage LastKnownStage { get; set; }
        #endregion

        #region Functions
        #region Event Handlers & Delegates
        /// <summary>
        /// !!!NOTE!!! This WILL NOT WORK if you are running this program in a debugger and use the "Stop Debugging" feature.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseMgs2EventHandler(object sender, EventArgs e)
        {
            try
            {
                Mgs2Process?.CloseMainWindow();
                Mgs2Process?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to close MGS2: {ex}");
            }
        }

        private static void TearDownMonitor()
        { 
            ScanningThread.Abort();
        }
        #endregion

        #region Threads
        private static void ScanForMgs2()
        {
            while (!MonitorCancellationToken.IsCancellationRequested) //this loop should only end when the program ends.
            {
                Process process = null;
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
                    process = Process.GetProcessesByName(Mgs2ProcessName).FirstOrDefault();
                else
                {
                    Process[] processes = Process.GetProcessesByName("METAL");
                    if (processes.Length == 1)
                        process = processes[0];
                    else
                    {
                        foreach (Process p in processes)
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(p))
                            {
                                nint signifyingMemory = 1234;
                                long bytesToRead = 4;
                                string determinantString = "ASDF";
                                try
                                {
                                    if (determinantString.Equals(
                                            Encoding.UTF8.GetString(
                                                spp.ReadProcessOffset(signifyingMemory, bytesToRead))))
                                        process = p;
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (process != null)
                {
                    if (Mgs2Process != process)
                    {
                        Mgs2Process = process;
                        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(Mgs2Process.MainModule.FileName);
                        Logger.Debug($"MGS2 found and hooked, version:\n{fileVersion}");
                        if (string.Compare(fileVersion.ProductVersion, DesiredVersion) != 0 && !_versionWarned) 
                        {
                            _versionWarned = true;
                            IMsBox<ButtonResult> msgBox = MessageBoxManager.GetMessageBoxStandard("Incompatible game version detected!",
                                $"The version of MGS2 we have hooked into({fileVersion.ProductVersion}) does not match what we expect({DesiredVersion})! Expect issues if you continue without updating the game.",
                                ButtonEnum.Ok);
                        }
                    }
                    Thread.Sleep(60 * Constants.MILLISECONDS_IN_SECOND); //scan every 60 seconds to see if MGS2 is still running
                }
                else
                {
                    Mgs2Process = null;
                    Thread.Sleep(10 * Constants.MILLISECONDS_IN_SECOND); //scan every 10 seconds if we know MGS2 IS NOT running
                }
            }
        }

        #region In-game Stats
        
        private static async Task MonitorScoringStats()
        {
            Mgs2CancellationTokenSource = new CancellationTokenSource();
            CancellationToken mgs2CancellationToken = Mgs2CancellationTokenSource.Token;
            await PeriodicTask.Run(UpdateScoringStats, TimeSpan.FromSeconds(1), mgs2CancellationToken);   
        }

        private static void UpdateScoringStats()
        {
            try
            {
                if (EnableGameStats)
                {
                    Stage currentStage = Mgs2MemoryManager.GetStage(); //Always found, or error is thrown.
                    if(currentStage?.Name != LastKnownStage?.Name)
                    {
                        Logger.Debug($"User is now in stage: {currentStage}");
                        LastKnownStage = currentStage!;
                    }
                    //if we're in a main menu, we shouldn't try to find stats right now.
                    if (!StageNames.MenuStages.StageList.Contains(currentStage!))
                    {
                        Mgs2MemoryManager.GameStats currentGameStats = Mgs2MemoryManager.ReadGameStats();
                        Difficulty currentDifficulty = Mgs2MemoryManager.ReadCurrentDifficulty();
                        //GameType currentGameType = MGS2MemoryManager.ReadGameType(); //TODO: finish determining how to determine what gametype we're in
                        //GUI.StaticGuiReference.UpdateGameStats(currentGameStats, currentDifficulty); //TODO: reimplement
                    }
                }
                else
                {
                    UpdateStatsTask.Dispose();
                }
            }
            catch(Exception e)
            {
                if (_mgs2Process != null)
                {
                    //only write to log when we are actually in a game, and should have some stats to grab
                    Logger.Error($"Failed to update scoring stats! Error encountered: {e}");
                }
            }
        }
        #endregion
        #endregion
        #endregion
        #endregion

        #region Constructor & Process Encapsulator
        static Mgs2Monitor()
        {
            Logger = Logging.InitializeNewLogger(LoggerName);
            Logger.Information($"MGS2 Monitor for version {Program.AppVersion} initialized...");
            Logger.Verbose($"Instance ID: {Program.InstanceId}");
        }

        public static bool EnableGameStats { get; set; } = false;

        public static Process Mgs2Process
        {
            get 
            {
                if (_mgs2Process != null && _mgs2Process.HasExited == false)
                {
                    return _mgs2Process;
                }

                try
                {
                    Mgs2CancellationTokenSource.Cancel();
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
                UpdateStatsTask = Task.Factory.StartNew(MonitorScoringStats);
                _mgs2Process = value;
            }
        }
        #endregion

        #region Config
        //TODO: replace with non-custom config system.
        #endregion

        internal static void EnableMonitor(CancellationToken cancellationToken)
        {
            MonitorCancellationToken = cancellationToken;
            MonitorCancellationToken.Register(TearDownMonitor);
            Logger.Information("Starting MGS2 scanning thread...");
            ScanningThread = new Thread(() => ScanForMgs2())
            {
                Name = "MGS2 Scanning Thread"
            };
            ScanningThread.Start();
            InitialLaunch = false;            
        }
    }
}
