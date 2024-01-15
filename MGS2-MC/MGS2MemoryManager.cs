using Serilog;
using SharpDX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC
{
    internal class MGS2MemoryManager
    {
        #region Internals
        // PInvoke declarations
        public static class NativeMethods
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
        }

        static readonly Process mgs2Process = MGS2Monitor.MGS2Process;
        static IntPtr PROCESS_BASE_ADDRESS = IntPtr.Zero;
        static int[] LAST_KNOWN_PLAYER_OFFSETS = default;
        static int[] LAST_KNOWN_STAGE_OFFSETS = default;
        private const string loggerName = "MemoryManagerDebuglog.log";
        private static ILogger logger;

        private enum ActiveCharacter
        {
            Snake,
            Raiden
        }

        internal static void StartLogger()
        {
            logger = Logging.InitializeNewLogger(loggerName);
            logger.Information($"Memory Manager for version {Program.AppVersion} initialized...");
            logger.Verbose($"Instance ID: {Program.InstanceID}");
        }

        #endregion

        #region Private methods
        private static ActiveCharacter DetermineActiveCharacter()
        {
            string stageName = GetStageName();

            if (stageName.Contains("tnk")) 
            {
                return ActiveCharacter.Snake;
            }
            else if(stageName.Contains("plt"))
            {
                return ActiveCharacter.Raiden;
            }
            else
            {
                throw new NotImplementedException("Unknown stage! Can't safely determine what the active character is");
            }
        }

        private static void CheckIfUsable(MGS2Object mgs2Object)
        {
            switch (DetermineActiveCharacter())
            {
                case ActiveCharacter.Snake:
                    if (!Snake.UsableObjects.Contains(mgs2Object))
                    {
                        throw new InvalidOperationException($"Snake cannot use {mgs2Object.Name}");
                    }
                    break;
                case ActiveCharacter.Raiden:
                    if (!Raiden.UsableObjects.Contains(mgs2Object))
                    {
                        throw new InvalidOperationException($"Raiden cannot use {mgs2Object.Name}");
                    }
                    break;
            }
        }

        private static Process GetMGS2Process()
        {
            if (mgs2Process == null || mgs2Process.HasExited) //if MGS2 was started separately from the trainer, get the process.
            {
                Process process = Process.GetProcessesByName(Constants.MGS2_PROCESS_NAME).FirstOrDefault();
                if (process == default)
                {
                    throw new NullReferenceException();
                }
                return process;
            }
            else 
            { 
                return mgs2Process; 
            }
        }

        private static int[] GetStageOffsets(Process mgs2Process, IntPtr processHandle)
        {
            byte[] gameMemoryBuffer = LoadGameMemory(mgs2Process.PrivateMemorySize64, processHandle);

            //if we've retrieved a stage offset before, check the old one first
            if (LAST_KNOWN_STAGE_OFFSETS != default)
            {
                if (ValidateLastKnownOffsets(gameMemoryBuffer, LAST_KNOWN_STAGE_OFFSETS, MGS2Offsets.StageInfoAoB))
                {
                    return LAST_KNOWN_STAGE_OFFSETS;
                }
            }

            LAST_KNOWN_STAGE_OFFSETS = new int[2];

            List<int> stageOffset = FindUniqueOffset(gameMemoryBuffer, MGS2Offsets.StageInfoAoB);

            //most of the time we only get 2 results, but sometimes we may get 3. we always want the final two.
            stageOffset = stageOffset.GetRange(stageOffset.Count - 2, 2);

            Array.Copy(stageOffset.ToArray(), LAST_KNOWN_STAGE_OFFSETS, 2);
            return LAST_KNOWN_STAGE_OFFSETS;
        }

        private static byte[] LoadGameMemory(long gameMemorySize, IntPtr processHandle)
        {
            byte[] buffer = new byte[gameMemorySize];
            try
            {
                NativeMethods.ReadProcessMemory(processHandle, PROCESS_BASE_ADDRESS, buffer, (uint)buffer.Length, out int numBytesRead);
            }
            catch (Exception e)
            {
                logger.Error($"Failed to read memory of process {Constants.MGS2_PROCESS_NAME}");
                throw new AggregateException($"Failed to read `{Constants.MGS2_PROCESS_NAME}`. Is it running?", e);
            }

            return buffer;
        }

        private static bool ValidateLastKnownOffsets(byte[] memoryBuffer, int[] lastKnownOffsets, byte[] finderAoB)
        {
            try
            {
                bool offsetIsValid = true;
                foreach (int previousOffset in lastKnownOffsets)
                {
                    byte[] previousOffsetBuffer = new byte[finderAoB.Length];
                    Array.Copy(memoryBuffer, previousOffset, previousOffsetBuffer, 0, finderAoB.Length);
                    for (int i = 0; i < previousOffsetBuffer.Length; i++)
                    {
                        if (previousOffsetBuffer[i] != finderAoB[i])
                        {
                            //if ANY byte does not match exactly to the offsetBytes, we know the offset has moved
                            offsetIsValid = false;
                        }
                    }
                }

                return offsetIsValid;
            }
            catch (Exception e)
            {
                logger.Warning($"Something unexpected went wrong when looking at the last known player offsets: {e}");
                //we failed to look at the last known player offsets, which isn't fatal.
                return false;
            }
        }

        private static List<int> FindUniqueOffset(byte[] gameMemoryBuffer, byte[] finderAoB, int resultLimit = -1)
        {
            int byteCount = 0;
            List<int> stageOffsets = new List<int>();
            try
            {
                while (byteCount + 2 < gameMemoryBuffer.Length)
                {
                    for (int position = 0; position < MGS2Offsets.StageInfoAoB.Length; position++)
                    {
                        //now filter any out that don't match with the StageInfo bytes
                        if (gameMemoryBuffer[byteCount + position] != MGS2Offsets.StageInfoAoB[position])
                            break;
                        //if we get all the way through the scan without finding anything "wrong", we have a match
                        else if (position == MGS2Offsets.StageInfoAoB.Length - 1)
                        {
                            stageOffsets.Add(byteCount);
                        }
                    }

                    if (stageOffsets.Count == resultLimit)
                        return stageOffsets;

                    byteCount += 2; //2 bytes seems to be the maximum we can reliably go without missing offsets
                }
            }
            catch (Exception e)
            {
                logger.Error($"Failed to find stage offset: {e}");
                throw new AggregateException($"Failed to find stage offset: ", e);
            }

            return stageOffsets;
        }

        private static List<int> FindNewPlayerOffsets(byte[] buffer)
        {
            int byteCount = 0;
            List<int> playerOffsets = new List<int>();
            try
            {
                while (byteCount + 152 < buffer.Length) //this can _probably just be 144 or 148, but i want to be safe
                {
                    bool mightBeValid = false;
                    for (int position = 0; position < MGS2Offsets.PlayerInfoFinderAoB.Length; position++)
                    {
                        //the "playerOffsetBytes" is very common within the game's memory. (~60-90 matches)
                        //HOWEVER, if we limit the playerOffsets bytes by the _relative position_, we get VERY few results!
                        //#1 if you have fired 0 shots, you will have 4 of these blocks of memory.
                        //#2 if you have fired 1+ shots but HAVE NOT checkpointed, you will have 3 blocks
                        //#3 if you have fired 1+ shots and HAVE checkpointed, you will have 2 blocks

                        //the playerOffsetBytes will have a mirrored result: one directly between the currentAmmo array and
                        //maxAmmo array, and another one directly before the start of currentItem array. They will be separated
                        //by EXACTLY 72 bytes(difference between currentAmmo and maxAmmo). Ignore any sets where the current
                        //byte value does not match with the byte value 72 bytes ahead.
                        if (buffer[byteCount + position + 72] != buffer[byteCount + position])
                            break;
                        //now filter any out that don't match with the playerOffsetBytes
                        if (buffer[byteCount + position] != MGS2Offsets.PlayerInfoFinderAoB[position])
                            break;
                        //if we get all the way through the scan without finding anything "wrong", we have a possible match
                        else if (position == MGS2Offsets.PlayerInfoFinderAoB.Length - 1)
                        {
                            mightBeValid = true;
                        }
                    }

                    if (mightBeValid)
                    {
                        byte[] bufferBeingExamined = new byte[MGS2Offsets.PlayerInfoFinderAoB.Length];
                        Array.Copy(buffer, byteCount + 144, bufferBeingExamined, 0, MGS2Offsets.PlayerInfoFinderAoB.Length);

                        //to filter out scenarios #1 and #2 above, for all of the possible matches, check 144 bytes ahead.
                        //ONLY if we are matching on a file with 0 shots OR 0 shots at last checkpoint can there be a full match
                        //144 bytes ahead. if at ANY point in the 144 bytes after each position in the offset array we're scanning
                        //there is a value that DOES NOT MATCH, then we know we have a real player offset.
                        for (int position = 0; position < MGS2Offsets.PlayerInfoFinderAoB.Length; position++)
                        {
                            if (bufferBeingExamined[position] != MGS2Offsets.PlayerInfoFinderAoB[position])
                            {
                                playerOffsets.Add(byteCount);
                                break;
                            }
                        }
                    }

                    if (playerOffsets.Count == 2) //if we have found 2 offsets, then we have found the player offset & the checkpoint
                        break;

                    byteCount += 4; //8 bytes can result in missed offsets, 4 bytes is sufficient in accuracy & speed.
                }

                return playerOffsets;
            }
            catch (Exception e)
            {
                logger.Error($"Failed to find player offset: {e}");
                throw new AggregateException($"Failed to find player offset: ", e);
            }
        }

        private static int[] GetPlayerOffsets(Process mgs2Process, IntPtr processHandle)
        {   
            byte[] gameMemoryBuffer = LoadGameMemory(mgs2Process.PrivateMemorySize64, processHandle);

            //if we've retrieved a player offset before, check the old one first
            if (LAST_KNOWN_PLAYER_OFFSETS != default)
            {
                if (ValidateLastKnownOffsets(gameMemoryBuffer, LAST_KNOWN_PLAYER_OFFSETS, MGS2Offsets.PlayerInfoFinderAoB))
                {
                    return LAST_KNOWN_PLAYER_OFFSETS;
                }
            }

            LAST_KNOWN_PLAYER_OFFSETS = new int[2];
            List<int> playerOffsets = FindNewPlayerOffsets(gameMemoryBuffer);            

            Array.Copy(playerOffsets.ToArray(), LAST_KNOWN_PLAYER_OFFSETS, 2);
            return LAST_KNOWN_PLAYER_OFFSETS;
        }

        private static byte[] ReadValueFromMemory(IntPtr processHandle, IntPtr objectAddress, byte[] bytesToRead = null)
        {
            if(bytesToRead == null)
            {
                bytesToRead = new byte[2];
            }

            bool success = NativeMethods.ReadProcessMemory(processHandle, objectAddress, bytesToRead, (uint) bytesToRead.Length, out int bytesRead);
            if (!success || bytesRead != bytesToRead.Length)
            {
                if (!success)
                {
                    logger.Debug("Failed to read memory...");
                }
                if(bytesRead != bytesToRead.Length)
                {
                    logger.Debug($"Expected to read {bytesToRead.Length}, but we actually read {bytesRead}");
                }
                throw new FileLoadException($"Failed to read value at offset {processHandle}+{objectAddress}.");
            }

            return bytesToRead;
        }

        private static void InvertBooleanValue(IntPtr processHandle, int playerOffset, int objectOffset)
        {
            int combinedOffset = playerOffset + objectOffset;
            IntPtr booleanAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, combinedOffset);
            byte[] currentValue = ReadValueFromMemory(processHandle, booleanAddress, new byte[sizeof(short)]);

            byte[] valueToWrite;
            
            if (Enumerable.SequenceEqual(currentValue, BitConverter.GetBytes((short)1)))
            {
                valueToWrite = BitConverter.GetBytes((short)0);
            }
            else
            {
                valueToWrite = BitConverter.GetBytes((short)1);
            }
             //= currentValue == BitConverter.GetBytes((short)0xFF) ? BitConverter.GetBytes((short)0xFF) : BitConverter.GetBytes((short)1);

            try
            {
                ModifyByteValueObject(objectOffset, valueToWrite);
            }
            catch(Exception ex)
            {
                logger.Error($"Failed to write boolean value at offset {processHandle}+{objectOffset}: {ex}");
                throw new AggregateException($"Failed to write boolean value at offset {processHandle}+{objectOffset}", ex);
            }
        }

        private static void ModifyByteValueObject(int objectOffset, byte[] value)
        {
            Process process;

            try
            {
                process = GetMGS2Process();
            }
            catch
            {
                logger.Error($"Failed to get process {Constants.MGS2_PROCESS_NAME}");
                throw new AggregateException($"Cannot find process `{Constants.MGS2_PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] playerOffset = GetPlayerOffsets(process, processHandle);
                int bytesWritten;

                byte[] buffer = value; // Value to write
                // Adjust offsets to add base address
                IntPtr playerAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, playerOffset[0] + objectOffset);
                IntPtr checkpointAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, playerOffset[1] + objectOffset);

                bool playerSuccess = NativeMethods.WriteProcessMemory(processHandle, playerAddress, buffer, (uint)buffer.Length, out bytesWritten);
                bool checkpointSuccess = NativeMethods.WriteProcessMemory(processHandle, checkpointAddress, buffer, (uint)buffer.Length, out bytesWritten);

                if ((!playerSuccess && !checkpointSuccess) || bytesWritten != buffer.Length)
                {
                    if (!playerSuccess)
                    {
                        logger.Debug("Failed to write player's memory...");
                    }
                    if (!checkpointSuccess)
                    {
                        logger.Debug("Failed to write checkpoint memory...");
                    }
                    if(bytesWritten != buffer.Length)
                    {
                        logger.Debug($"We tried to write {buffer.Length} bytes, but ended up writing {bytesWritten}");
                    }
                    throw new InvalidOperationException($"Failed to write memory at {objectOffset} with value {value}.");
                }
            }
            catch (Exception e)
            {
                logger.Error("Something went wrong when trying to modify the game's memory!");
                throw new AggregateException($"Something unexpected went wrong when trying to modify the game's memory! {e}");
            }
            finally
            {
                if(processHandle != default)
                {
                    NativeMethods.CloseHandle(processHandle);
                }
            }
        }

        private static byte[] GetCurrentValue(int valueOffset, int valueSize, Func<Process, IntPtr, int[]> aobFinderMethod, int indexToRead = 0)
        {
            Process process;

            try
            {
                process = GetMGS2Process();
            }
            catch
            {
                logger.Error($"Failed to get process {Constants.MGS2_PROCESS_NAME}");
                throw new AggregateException($"Cannot find process `{Constants.MGS2_PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] aobOffsets = aobFinderMethod(process, processHandle);
                IntPtr memoryAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, aobOffsets[indexToRead] + valueOffset);

                byte[] bytesRead = new byte[valueSize];
                ReadValueFromMemory(processHandle, memoryAddress, bytesRead);

                return bytesRead;
            }
            catch (Exception e)
            {
                logger.Debug($"Failed to get current value at {PROCESS_BASE_ADDRESS}+{valueOffset}");
                throw new AggregateException($"Something unexpected went wrong when trying to get current value! {e}");
            }
            finally
            {
                if (processHandle != default)
                {
                    NativeMethods.CloseHandle(processHandle);
                }
            }
        }
        #endregion

        public static string GetStageName()
        {
            byte[] stageNameBytes = GetCurrentValue(MGS2Offsets.CURRENT_STAGE.Start, MGS2Offsets.CURRENT_STAGE.Length, GetStageOffsets);
            return Encoding.UTF8.GetString(stageNameBytes);
        }

        public static byte[] GetPlayerInfoBasedValue(int valueOffset, int sizeToRead)
        {
            return GetCurrentValue(valueOffset, sizeToRead, GetPlayerOffsets);
        }

        public static void UpdateObjectBaseValue(MGS2Object mgs2Object, short value)
        {
            CheckIfUsable(mgs2Object);

            switch (mgs2Object)
            {
                case StackableItem stackableItem:
                    ModifyByteValueObject(stackableItem.CurrentCountOffset, BitConverter.GetBytes(value));
                    break;
                case DurabilityItem durabilityItem:
                    ModifyByteValueObject(durabilityItem.DurabilityOffset, BitConverter.GetBytes(value));
                    break;
                case AmmoWeapon ammoWeapon:
                    ModifyByteValueObject(ammoWeapon.CurrentAmmoOffset, BitConverter.GetBytes(value));
                    break;
                case SpecialWeapon specialWeapon:
                    ModifyByteValueObject(specialWeapon.SpecialOffset, BitConverter.GetBytes(value));
                    break;
                case LevelableItem levelableItem:
                    ModifyByteValueObject(levelableItem.LevelOffset, BitConverter.GetBytes(value));
                    break;
            }
        }

        public static void UpdateObjectMaxValue(MGS2Object mgs2Object, short count)
        {
            CheckIfUsable(mgs2Object);

            switch (mgs2Object)
            {
                case StackableItem stackableItem:
                    ModifyByteValueObject(stackableItem.MaxCountOffset, BitConverter.GetBytes(count)); 
                    break;
                case AmmoWeapon ammoWeapon:
                    ModifyByteValueObject(ammoWeapon.MaxAmmoOffset, BitConverter.GetBytes(count));
                    break;
            }
        }

        public static void ToggleObject(MGS2Object mgs2Object)
        {
            CheckIfUsable(mgs2Object);
            int objectOffset = mgs2Object.InventoryOffset;
            Process process;

            try
            {
                process = GetMGS2Process();
            }
            catch
            {
                logger.Error($"Failed to get process {Constants.MGS2_PROCESS_NAME}");
                throw new AggregateException($"Cannot find process `{Constants.MGS2_PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] playerOffset = GetPlayerOffsets(process, processHandle);

                InvertBooleanValue(processHandle, playerOffset[0], objectOffset);
            }
            catch (Exception e)
            {
                logger.Debug($"Could not toggle {mgs2Object.Name}");
                throw new AggregateException("Failed to toggle object.", e);
            }
            finally
            {
                if (processHandle != default)
                {
                    NativeMethods.CloseHandle(processHandle);
                }
            }
        }
    }
}
