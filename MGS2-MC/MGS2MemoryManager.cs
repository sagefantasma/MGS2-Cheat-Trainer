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

        static IntPtr PROCESS_BASE_ADDRESS = IntPtr.Zero;
        static int[] LAST_KNOWN_PLAYER_OFFSETS = default;
        #endregion

        #region Private methods
        private static Process GetMGS2Process()
        {
            Process process = Process.GetProcessesByName(MGS2Constants.PROCESS_NAME).FirstOrDefault(); //looks like NET framework doesn't support the ? operator here
            if (process == default)
            {
                throw new NullReferenceException();
            }
            return process;
        }

        private static int[] GetCurrentPlayerOffset(Process mgs2Process, IntPtr processHandle)
        {
            byte[] buffer = new byte[mgs2Process.PrivateMemorySize64];
            try
            {
                NativeMethods.ReadProcessMemory(processHandle, PROCESS_BASE_ADDRESS, buffer, (uint)buffer.Length, out int numBytesRead);
            }
            catch(Exception e)
            {
                //TODO: add logging :)
                throw new AggregateException($"Failed to read `{MGS2Constants.PROCESS_NAME}`. Is it running?", e);
            }

            //if we've retrieved a player offset before, check the old one first
            if (LAST_KNOWN_PLAYER_OFFSETS != default)
            {
                try
                {
                    bool offsetHasMoved = false;
                    foreach (int previousOffset in LAST_KNOWN_PLAYER_OFFSETS)
                    {
                        byte[] previousOffsetBuffer = new byte[MGS2Constants.PlayerOffsetBytes.Length];
                        Array.Copy(buffer, previousOffset, previousOffsetBuffer, 0, MGS2Constants.PlayerOffsetBytes.Length);
                        for (int i = 0; i < previousOffsetBuffer.Length; i++)
                        {
                            if (previousOffsetBuffer[i] != MGS2Constants.PlayerOffsetBytes[i])
                            {
                                //if ANY byte does not match exactly to the offsetBytes, we know the offset has moved
                                offsetHasMoved = true;
                            }
                        }
                    }
                    if (!offsetHasMoved)
                    {
                        return LAST_KNOWN_PLAYER_OFFSETS;
                    }
                }
                catch (Exception e)
                {
                    //TODO: add logging :)
                    //we failed to look at the last known player offsets, which isn't fatal.
                }
            }            

            int byteCount = 0;
            List<int> playerOffset = new List<int>();
            try
            {
                while (byteCount + 152 < buffer.Length) //this can _probably just be 144 or 148, but i want to be safe
                {
                    bool mightBeValid = false;
                    for (int position = 0; position < MGS2Constants.PlayerOffsetBytes.Length; position++)
                    {
                        //the "playerOffsetBytes" is very common within the game's memory. (~60-90 matches)
                        //HOWEVER, if we limit the playerOffset bytes by the _relative position_, we get VERY few results!
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
                        if (buffer[byteCount + position] != MGS2Constants.PlayerOffsetBytes[position])
                            break;
                        //if we get all the way through the scan without finding anything "wrong", we have a possible match
                        else if (position == MGS2Constants.PlayerOffsetBytes.Length - 1)
                        {
                            mightBeValid = true;
                        }
                    }

                    if (mightBeValid)
                    {
                        byte[] bufferBeingExamined = new byte[MGS2Constants.PlayerOffsetBytes.Length];
                        Array.Copy(buffer, byteCount + 144, bufferBeingExamined, 0, MGS2Constants.PlayerOffsetBytes.Length);

                        //to filter out scenarios #1 and #2 above, for all of the possible matches, check 144 bytes ahead.
                        //ONLY if we are matching on a file with 0 shots OR 0 shots at last checkpoint can there be a full match
                        //144 bytes ahead. if at ANY point in the 144 bytes after each position in the offset array we're scanning
                        //there is a value that DOES NOT MATCH, then we know we have a real player offset.
                        for (int position = 0; position < MGS2Constants.PlayerOffsetBytes.Length; position++)
                        {
                            if (bufferBeingExamined[position] != MGS2Constants.PlayerOffsetBytes[position])
                            {
                                playerOffset.Add(byteCount);
                                break;
                            }
                        }
                    }

                    if (playerOffset.Count == 2) //if we have found 2 offsets, then we have found the player offset & the checkpoint
                        break;

                    byteCount += 4; //8 bytes can result in missed offsets, 4 bytes is sufficient in accuracy & speed.
                }
            }
            catch(Exception e)
            {
                //TODO: add logging :)
                throw new AggregateException($"Failed to find player offset: ", e);
            }

            Array.Copy(playerOffset.ToArray(), LAST_KNOWN_PLAYER_OFFSETS, 2);
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
                throw new FileLoadException($"Failed to read value at offset {processHandle}+{objectAddress}.");
            }

            return bytesToRead;
        }

        private static void ReadWriteBooleanValue(IntPtr processHandle, int playerOffset, int objectOffset)
        {
            int combinedOffset = playerOffset + objectOffset;
            IntPtr booleanAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, combinedOffset);
            byte[] currentValue = ReadValueFromMemory(processHandle, booleanAddress, new byte[sizeof(short)]);

            byte[] valueToWrite = currentValue == BitConverter.GetBytes((short)0) ? BitConverter.GetBytes((short)0) : BitConverter.GetBytes((short)1);

            try
            {
                ModifyByteValueObject(objectOffset, valueToWrite);
            }
            catch(Exception ex)
            {
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
                throw new AggregateException($"Cannot find process `{MGS2Constants.PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] playerOffset = GetCurrentPlayerOffset(process, processHandle);
                int bytesWritten;

                byte[] buffer = value; // Value to write
                // Adjust offsets to add base address
                IntPtr playerAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, playerOffset[0] + objectOffset);
                IntPtr checkpointAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, playerOffset[1] + objectOffset);

                bool playerSuccess = NativeMethods.WriteProcessMemory(processHandle, playerAddress, buffer, (uint)buffer.Length, out bytesWritten);
                bool checkpointSuccess = NativeMethods.WriteProcessMemory(processHandle, checkpointAddress, buffer, (uint)buffer.Length, out bytesWritten);

                if ((!playerSuccess && !checkpointSuccess) || bytesWritten != buffer.Length)
                {
                    throw new InvalidOperationException($"Failed to write memory at {objectOffset} with value {value}.");
                }
            }
            catch (Exception e)
            {
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
        #endregion

        public static byte[] GetCurrentValue(int valueOffset, int sizeToRead)
        {
            Process process;

            try
            {
                process = GetMGS2Process();
            }
            catch
            {
                throw new AggregateException($"Cannot find process `{MGS2Constants.PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] playerOffset = GetCurrentPlayerOffset(process, processHandle);
                IntPtr playerAddress = IntPtr.Add(PROCESS_BASE_ADDRESS, playerOffset[0] + valueOffset); // Adjusted to add base address

                byte[] bytesRead = new byte[sizeToRead];
                ReadValueFromMemory(processHandle, playerAddress, bytesRead);

                return bytesRead;
            }
            catch(Exception e)
            {
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

        public static void UpdateObjectBaseValue(MGS2Object mgs2Object, short value)
        {
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

        public static void ToggleObject(int objectOffset)
        {
            Process process;

            try
            {
                process = GetMGS2Process();
            }
            catch
            {
                throw new AggregateException($"Cannot find process `{MGS2Constants.PROCESS_NAME}` - is it running?");
            }

            PROCESS_BASE_ADDRESS = process.MainModule.BaseAddress;
            IntPtr processHandle = default;
            try
            {
                processHandle = NativeMethods.OpenProcess(0x1F0FFF, false, process.Id);
                int[] playerOffset = GetCurrentPlayerOffset(process, processHandle);

                ReadWriteBooleanValue(processHandle, playerOffset[0], objectOffset); //set player
                ReadWriteBooleanValue(processHandle, playerOffset[1], objectOffset); //set checkpoint
            }
            catch (Exception e)
            {
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
