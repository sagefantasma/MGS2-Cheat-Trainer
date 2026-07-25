using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimplifiedMemoryManager;
using Constants = MGS2_CheatTrainer_V2.Models.Constants;

namespace MGS2_CheatTrainer_V2
{
    internal static class Mgs2Monitor
    {
        #region Private members, fields, and functions
        #region Members & fields
        private const string Mgs2ProcessName = "METAL GEAR SOLID2";
        private const string DesiredVersion = "2.1.0.0";
        private static bool _versionWarned;

        private static Process? _mgs2Process;

        private static CancellationToken MonitorCancellationToken { get; set; }
        private static CancellationTokenSource Mgs2CancellationTokenSource { get; } = new ();
        private static ILogger? Logger => Logging.Logger;
        private static Thread? ScanningThread { get; set; }
        
        public static event EventHandler<bool>? OnGameHooked;
        public static event EventHandler<string>? OnInvalidVersionDetected;
        #endregion
        
        #region Functions
        #region Event Handlers & Delegates

        private static void GameHooked(bool hooked)
        {
            OnGameHooked?.Invoke(null, hooked);
        }
        
        private static void TearDownMonitor()
        {
            OnGameHooked?.Invoke(null, false);
        }

        private static void InvalidVersionDetected(string message)
        {
            OnInvalidVersionDetected?.Invoke(null, message);
        }
        #endregion

        #region Threads
        private static void ScanForMgs2()
        {
            while (!MonitorCancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (Mgs2Process == null)
                    {
                        Process? process = null;

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            process = Process.GetProcessesByName(Mgs2ProcessName).FirstOrDefault();
                        else
                        {
                            Process[] processes = Process.GetProcessesByName("METAL");
                            if (processes.Length == 1)
                                process = processes[0];
                            else if (processes.Length == 0)
                                processes = Process.GetProcesses();

                            if (process is null)
                            {
                                foreach (Process p in processes)
                                {
                                    if (!p.ProcessName.Contains("METAL")) continue;
                                    using SimpleProcessProxy spp = new SimpleProcessProxy(p);
                                    nint signifyingMemory = 0x72F2E0;
                                    long bytesToRead = 18;
                                    string determinantString = "METAL GEAR SOLID 2";
                                    try
                                    {
                                        byte[] memory = spp.ReadProcessOffset(signifyingMemory, bytesToRead);
                                        string decodedString = Encoding.UTF8.GetString(memory);
                                        if (determinantString.Equals(decodedString))
                                            process = p;
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                            }
                        }

                        if (process != null)
                        {
                            // Bug fix: only update if process actually changed
                            if (Mgs2Process?.Id != process.Id)
                            {
                                Mgs2Process = process;
                                GameHooked(true);
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(
                                        Mgs2Process.MainModule?.FileName!);
                                    Logger?.Information($"MGS2 found and hooked, version:\n{fileVersion}");

                                    if (string.Compare(fileVersion.ProductVersion, DesiredVersion,
                                            StringComparison.InvariantCultureIgnoreCase) != 0
                                        && !_versionWarned)
                                    {
                                        //TODO: Is there a way to make this work when using Proton? Because the FileVersionInfo
                                        //when using Proton is the FileVersionInfo FOR Proton... hmmj
                                        _versionWarned = true;
                                        InvalidVersionDetected(
                                            $"The version of MGS2 we have hooked({fileVersion.ProductVersion}) " +
                                            $"does not match expected({DesiredVersion})!");
                                    }
                                }
                                else
                                {
                                    //Linux path
                                    Logger?.Information("MGS2 found and hooked");
                                }
                            }

                            Thread.Sleep(60 * Constants.MillisecondsInSecond);
                        }
                        else
                        {
                            // Bug fix: only clear if we previously had a process
                            if (Mgs2Process != null)
                                Mgs2Process = null;
                            Thread.Sleep(10 * Constants.MillisecondsInSecond);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger?.Error($"Something went wrong in ScanningThread: {e}");
                }
            }
        }
        #endregion
        #endregion
        #endregion

        #region Constructor & Process Encapsulator
        static Mgs2Monitor()
        {
            Logger?.Information($"MGS2 Monitor for version {Program.AppVersion} initialized...");
        }

        public static Process? Mgs2Process
        {
            get
            {
                if (_mgs2Process != null && !_mgs2Process.HasExited)
                    return _mgs2Process;

                try { Mgs2CancellationTokenSource.Cancel(); }
                catch { 
                    //ignored
                }
                _mgs2Process = null;
                return null;
            }
            private set
            {
                // Bug fix: only start monitoring task when setting a real process,
                // and only if it's actually a different process than what we have
                if (value != null && value != _mgs2Process)
                {
                    // Cancel any existing monitoring task before starting a new one
                    try { Mgs2CancellationTokenSource.Cancel(); }
                    catch
                    {
                        // ignored
                    }

                    _mgs2Process = value;
                }
                else if (value == null)
                {
                    // Just clear the process, don't start a new task
                    _mgs2Process = null;
                }
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
            Logger?.Information("Starting MGS2 scanning thread...");
            Task.Run(ScanForMgs2,  cancellationToken);
        }
    }
}
