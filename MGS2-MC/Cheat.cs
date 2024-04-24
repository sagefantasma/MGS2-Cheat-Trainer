using SimplifiedMemoryManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal static class CheatNativeMethods
    {
        // Declare OpenProcess
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        // Declare WriteProcessMemory with short
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref short lpBuffer, uint nSize, out int lpNumberOfBytesWritten);
        // and with bytes
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        // Declare ReadProcessMemory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out short lpBuffer, uint size, out int lpNumberOfBytesRead);
        // and with bytes
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesRead);

        // Declare CloseHandle
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpBaseAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
    }

    public struct Cheat
    {
        public string Name { get; private set; }
        public Action CheatAction;

        public Cheat(string name, Action action)
        {
            Name = name;
            CheatAction = action;
        }

        internal class CheatActions
        {
            private static void ReplaceWithInvalidCode(string aob, MemoryOffset offset, int bytesToReplace, int startIndexToReplace = 0)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForPattern(pattern);

                                if (memoryLocation != -1)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(memoryLocation + offset.Start, offset.Length);

                                    for (int i = startIndexToReplace; i < startIndexToReplace + bytesToReplace; i++)
                                    {
                                        memoryContent[i] = 0x90;
                                    }

                                    spp.ModifyProcessOffset(memoryLocation, memoryContent, true);
                                    successful = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static void ReplaceWithNewCode(string aob, MemoryOffset offset, byte[] newCode, int additionalOffset = 0)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForPattern(pattern);

                                if (memoryLocation != -1)
                                {
                                    byte[] memoryContent = spp.ReadProcessOffset(memoryLocation + offset.Start, offset.Length);

                                    spp.ModifyProcessOffset(memoryLocation + additionalOffset, newCode, true);
                                    successful = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static void ModifySingleByte(string aob, MemoryOffset offset, byte replacementValue)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForPattern(pattern);

                                if (memoryLocation != -1)
                                {
                                    spp.ModifyProcessOffset(memoryLocation + offset.Start, replacementValue, true);
                                    successful = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);
                }
            }

            private static byte[] ReadMemory(string aob, MemoryOffset offset)
            {
                lock (MGS2Monitor.MGS2Process)
                {
                    bool successful = false;
                    int retries = 5;
                    do
                    {
                        try
                        {
                            using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                            {
                                SimplePattern pattern = new SimplePattern(aob);
                                int memoryLocation = spp.ScanMemoryForPattern(pattern);

                                if(memoryLocation != -1)
                                    return spp.ReadProcessOffset(memoryLocation + offset.Start, offset.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            retries--;
                        }
                    } while (!successful && retries > 0);

                    return null;
                }
            }

            private static void SetMemoryOutsideGameBlock(byte[] bytes, int memoryLocation, IntPtr moduleBaseAddress)
            {
                //Process process = Process.GetProcessById(moduleId);
                //IntPtr moduleBaseAddress = process.MainModule.BaseAddress;
                lock (MGS2Monitor.MGS2Process)
                {
                    try
                    {
                        bool successful = false;
                        int retries = 5;
                        //byte[] bytesRead = new byte[MGS2Monitor.MGS2Process.MainModule.BaseAddress.ToInt64() + MGS2Monitor.MGS2Process.PrivateMemorySize64];

                        do
                        {
                            try
                            {
                                moduleBaseAddress = CheatNativeMethods.OpenProcess(0x1F0FFF, false, MGS2Monitor.MGS2Process.Id);

                                successful = CheatNativeMethods.WriteProcessMemory(moduleBaseAddress, IntPtr.Add(moduleBaseAddress, memoryLocation), bytes, (uint)bytes.Length, out int bytesWritten);
                            }
                            catch (Exception e)
                            {
                                retries--;
                            }
                        } while (!successful && retries > 0);
                    }
                    finally
                    {
                        if (moduleBaseAddress != IntPtr.Zero)
                            CheatNativeMethods.CloseHandle(moduleBaseAddress);
                    }
                }
            }

            private static byte[] ReadMemoryOutsideGameBlock(string aob, MemoryOffset offset, out int memoryLocation, out IntPtr moduleBaseAddress)
            {
                IntPtr moduleHandle = IntPtr.Zero;
                lock (MGS2Monitor.MGS2Process)
                {
                    try
                    {
                        bool successful = false;
                        int retries = 5;
                        //long arraySize = MGS2Monitor.MGS2Process.MainModule.BaseAddress.ToInt64() / 4;
                        //MGS2Monitor.MGS2Process.Modules.

                        
                        //int otherArraySize = MGS2Monitor.MGS2Process.MainModule.BaseAddress.ToInt32();
                        //List<byte[]> memoryArrays = new List<byte[]>();
                        
                            //memoryArrays.Add(new byte[module.ModuleMemorySize]);
                        //}
                        /*for(long i = 0; i < arraySize; i += 1073741824)
                        {
                            memoryArrays.Add(new byte[1073741824]);
                        }*/
                    //var bytesRead = Array.CreateInstance(typeof(byte), arraySize);
                    //bytesRead = new byte[arraySize];

                        foreach (Process module in Process.GetProcesses())
                        {
                            //if (module.ModuleName == "METAL GEAR SOLID2.exe")
                            //    continue;
                            //Process moduleProcess = Process.GetProcessesByName(module.ModuleName).First();
                            //moduleId = moduleProcess.Id;
                            /*
                             * 
                             * 
                             * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                             * 
                             * So all this crap here was my "best" idea at how to replicate Cheat Engine's
                             * "scan all memory" functionality, but this didn't work. it also took over 5
                             * minutes to go through all processes that were openable OMEGALUL
                             * 
                             * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                             * 
                             * 
                             */
                            try
                            {
                                moduleBaseAddress = module.MainModule.BaseAddress;
                            }
                            catch
                            {
                                continue;
                            }
                            byte[] moduleMemory = new byte[module.MainModule.ModuleMemorySize];
                            do
                            {
                                try
                                {
                                    moduleHandle = CheatNativeMethods.OpenProcess(0x1F0FFF, false, MGS2Monitor.MGS2Process.Id);
                                    

                                    successful = CheatNativeMethods.ReadProcessMemory(moduleHandle, moduleBaseAddress, moduleMemory, (uint)moduleMemory.Length, out int totalRead);
                                    
                                }
                                catch (Exception e)
                                {
                                    retries--;
                                }
                            } while (!successful && retries > 0);
                            if (successful)
                            {
                                using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                                {
                                    IntPtr guaranteed = new IntPtr(2101677557987);
                                    if(moduleBaseAddress.ToInt64() < guaranteed.ToInt64() && IntPtr.Add(moduleBaseAddress, moduleMemory.Length).ToInt64() > guaranteed.ToInt64())
                                    {
                                        int b = 420 + 69;
                                    }
                                    SimplePattern pattern = new SimplePattern(aob);
                                    /*int foundLocation = -1;
                                    for (long i = 0; i < arraySize; i += int.MaxValue)
                                    {
                                        byte[] tempArray = new byte[int.MaxValue];
                                        Array.Copy(bytesRead, i, tempArray, 0, int.MaxValue);
                                        foundLocation = spp.ScanMemoryForPattern(pattern, tempArray);
                                        if (foundLocation != -1)
                                            break;
                                    }*/
                                    memoryLocation = -1;
                                    try
                                    {
                                        memoryLocation = spp.ScanMemoryForPattern(pattern, moduleMemory);
                                    }
                                    catch {
                                        int a = 2 + 2;
                                    }
                                    //memoryLocation = 1;

                                    if (memoryLocation != -1)
                                    {
                                        byte[] specificBytesToRead = new byte[offset.Length];
                                        successful = CheatNativeMethods.ReadProcessMemory(moduleHandle, module.MainModule.BaseAddress, specificBytesToRead, (uint)offset.Length, out int totalRead);

                                        if (successful)
                                        {
                                            return specificBytesToRead;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                }
                            }
                        }

                        memoryLocation = -1;
                        moduleBaseAddress = IntPtr.Zero;
                        return null;
                    }
                    finally
                    {
                        if (moduleHandle != IntPtr.Zero)
                            CheatNativeMethods.CloseHandle(moduleHandle);
                    }
                }
            }

            public static void TurnScreenBlack() //works
            {
                ModifySingleByte(MGS2AoB.Camera, MGS2Offset.BLACK_SCREEN, 0x00);
            }

            public static void TurnOffBleedDamage() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBleedDamage, MGS2Offset.NO_BLEED_DMG, 7); 
            }

            public static void TurnOffBurnDamage() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.NoBurnDamage, MGS2Offset.NO_BURN_DMG, 7); 
            }

            internal static void InfiniteAmmo() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteAmmo, MGS2Offset.INFINITE_AMMO, 4); 
            }

            internal static void InfiniteLife() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteLife, MGS2Offset.INFINITE_LIFE, 4);
            }

            internal static void InfiniteOxygen() //works
            {
                ReplaceWithInvalidCode(MGS2AoB.InfiniteO2, MGS2Offset.INFINITE_O2, 4);
            }

            internal static void Letterboxing() //works
            {
                ModifySingleByte(MGS2AoB.Camera, MGS2Offset.LETTERBOX, 0x00);
            }

            internal static void AmmoNeverDepletes() //crashes
            {
                //ReplaceWithInvalidCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, 4); 
                ReplaceWithNewCode(MGS2AoB.NeverReload, MGS2Offset.NEVER_RELOAD, new byte[] { 0xE9, 0xA5, 0xFE, 0xA9, 0xFF, 0x0F, 0x1F, 0x00 }, -6);
            }

            internal static void NoClipNoGravity() //doesnt exist within the game's memory, so we can't use it atm
            {
                NoClip(false);
            }

            internal static void NoClipWithGravity() //doesnt exist within the game's memory, so we can't use it atm
            {
                NoClip(true);
            }

            private static void NoClip(bool gravity)
            {
                Constants.PlayableCharacter currentPc = MGS2MemoryManager.DetermineActiveCharacter();

                lock (MGS2Monitor.MGS2Process)
                {
                    string activeCharacterAoB;
                    using (SimpleProcessProxy spp = new SimpleProcessProxy(MGS2Monitor.MGS2Process))
                    {
                        switch (currentPc)
                        {
                            case Constants.PlayableCharacter.Raiden:
                                activeCharacterAoB = MGS2AoB.RaidenClipping;
                                break;
                            case Constants.PlayableCharacter.Snake:
                                activeCharacterAoB = MGS2AoB.SnakeClipping;
                                break;
                            default:
                                throw new NotImplementedException("This character is not (yet?) supported for no clip, sorry");
                        }
                    }

                    byte[] memoryContent = ReadMemoryOutsideGameBlock(activeCharacterAoB, MGS2Offset.NO_CLIP, out int memoryLocation, out IntPtr moduleBaseAddress);

                    if (gravity)
                    {
                        //set byte to either 15 or 25
                        if (memoryContent[1] == 0x24)
                        {
                            memoryContent[1] = 0x25;
                        }
                        else
                        {
                            memoryContent[1] = 0x15;
                        }
                    }
                    else
                    {
                        //set byte to either 13 or 23
                        if (memoryContent[1] == 0x24)
                        {
                            memoryContent[1] = 0x23;
                        }
                        else
                        {
                            memoryContent[1] = 0x13;
                        }
                    }
                    SetMemoryOutsideGameBlock(memoryContent, memoryLocation, moduleBaseAddress);
                }
            }

            internal static void ZoomIn()
            {
                Zoom(true);
            }

            internal static void ZoomOut()
            {
                Zoom(false);
            }

            private static void Zoom(bool zoomIn)
            {
                byte[] currentZoom = ReadMemory(MGS2AoB.Camera, MGS2Offset.ZOOM);

                if (currentZoom == null)
                    return;

                if (zoomIn)
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]++);
                else
                    ModifySingleByte(MGS2AoB.Camera, MGS2Offset.ZOOM, currentZoom[0]--);
            }
        }
    }    

    public class MGS2Cheat
    {
        public static Cheat BlackScreen { get; } = new Cheat("Black Screen", Cheat.CheatActions.TurnScreenBlack);
        public static Cheat NoBleedDamage { get; } = new Cheat("Bleeding Causes No Damage", Cheat.CheatActions.TurnOffBleedDamage);
        public static Cheat NoBurnDamage { get; } = new Cheat("Burning Causes No Damage", Cheat.CheatActions.TurnOffBurnDamage);
        public static Cheat InfiniteAmmo { get; } = new Cheat("Infinite Ammo", Cheat.CheatActions.InfiniteAmmo);
        public static Cheat InfiniteLife { get; } = new Cheat("Infinite Life", Cheat.CheatActions.InfiniteLife);
        public static Cheat InfiniteOxygen { get; } = new Cheat("Infinite Oxygen", Cheat.CheatActions.InfiniteOxygen);
        public static Cheat Letterboxing { get; } = new Cheat("Letterboxing", Cheat.CheatActions.Letterboxing);
        public static Cheat NoReload { get; } = new Cheat("Reloading Not Required", Cheat.CheatActions.AmmoNeverDepletes);
        public static Cheat NoClipWithGravity { get; } = new Cheat("Walk Through Walls (gravity)", Cheat.CheatActions.NoClipWithGravity);
        public static Cheat NoClipNoGravity { get; } = new Cheat("Walk Through Walls (no gravity)", Cheat.CheatActions.NoClipNoGravity);
        public static Cheat ZoomIn { get; } = new Cheat("Zoom In", Cheat.CheatActions.ZoomIn);
        public static Cheat ZoomOut { get; } = new Cheat("Zoom Out", Cheat.CheatActions.ZoomOut);

        private static List<Cheat> _cheatList = null;

        private static void BuildCheatList()
        {
            _cheatList = new List<Cheat>
            {
                BlackScreen, NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, Letterboxing,
                NoReload, NoClipWithGravity, NoClipNoGravity, ZoomIn, ZoomOut
            };
        }

        public static List<Cheat> CheatList
        {
            get
            {
                if (_cheatList == null)
                {
                    BuildCheatList();
                };

                return _cheatList;
            }
        }
    }
    
}
