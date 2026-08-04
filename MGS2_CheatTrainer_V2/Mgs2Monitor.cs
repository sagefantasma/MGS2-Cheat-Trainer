using Serilog;
using System;
using System.Diagnostics;
using System.IO;
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
                                string? fileVersionString;
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(
                                        Mgs2Process.MainModule?.FileName!);
                                    fileVersionString = fileVersion.ToString();
                                }
                                else
                                {
                                    //Linux path
                                    string? gameExePath = FindGameExePath(process.Id);
                                    if (gameExePath != null && File.Exists(gameExePath))
                                        fileVersionString = GetVersionFromPeFile(gameExePath);
                                    else
                                        fileVersionString = "UNKNOWN!";
                                }

                                Logger?.Information($"MGS2 found and hooked, version: {fileVersionString}");

                                if (string.Compare(fileVersionString, DesiredVersion,
                                        StringComparison.InvariantCultureIgnoreCase) != 0
                                    && !_versionWarned)
                                {
                                    _versionWarned = true;
                                    InvalidVersionDetected(
                                        $"The version of MGS2 we have hooked({fileVersionString}) " +
                                        $"does not match expected({DesiredVersion})!");
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
        
        private static string? FindGameExePath(int pid)
        {
            try
            {
                foreach (string line in File.ReadLines($"/proc/{pid}/maps"))
                {
                    // Find the path by taking everything from the first '/' onwards
                    int pathStart = line.IndexOf('/');
                    if (pathStart < 0) continue;

                    string path = line.Substring(pathStart).Trim();

                    if (!path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) continue;

                    // Verify it's a readable mapping by checking the permissions field
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 2 || !parts[1].Contains('r')) continue;

                    return path;
                }
            }
            catch
            {
                //Squelch errors.
            }
            return null;
        }
        
        private static string? GetVersionFromPeFile(string exePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(exePath);

                // Search for the VS_VERSION_INFO signature
                byte[] signature = Encoding.Unicode.GetBytes("VS_VERSION_INFO");
                for (int i = 0; i < fileBytes.Length - signature.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < signature.Length; j++)
                    {
                        if (fileBytes[i + j] != signature[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        // Version numbers are at fixed offsets from VS_VERSION_INFO
                        // MS-DOS structure: after the wLength(2), wValueLength(2), wType(2), szKey
                        // then padding to DWORD boundary, then VS_FIXEDFILEINFO
                        int fixedInfoOffset = i + signature.Length + 2; // skip null terminator + padding
                        fixedInfoOffset = (fixedInfoOffset + 3) & ~3; // align to DWORD

                        if (fixedInfoOffset + 52 >= fileBytes.Length) continue;

                        // VS_FIXEDFILEINFO starts with dwSignature 0xFEEF04BD
                        uint sig = BitConverter.ToUInt32(fileBytes, fixedInfoOffset);
                        if (sig != 0xFEEF04BD) continue;

                        // FileVersion is at offset 8 in VS_FIXEDFILEINFO
                        ushort major = BitConverter.ToUInt16(fileBytes, fixedInfoOffset + 10);
                        ushort minor = BitConverter.ToUInt16(fileBytes, fixedInfoOffset + 8);
                        ushort build = BitConverter.ToUInt16(fileBytes, fixedInfoOffset + 14);
                        ushort revision = BitConverter.ToUInt16(fileBytes, fixedInfoOffset + 12);

                        return $"{major}.{minor}.{build}.{revision}";
                    }
                }
            }
            catch
            {
                //Squelch error
            }
            return null;
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

        internal static void EnableMonitor(CancellationToken cancellationToken)
        {
            MonitorCancellationToken = cancellationToken;
            MonitorCancellationToken.Register(TearDownMonitor);
            Logger?.Information("Starting MGS2 scanning thread...");
            Task.Run(ScanForMgs2,  cancellationToken);
        }
    }
}
