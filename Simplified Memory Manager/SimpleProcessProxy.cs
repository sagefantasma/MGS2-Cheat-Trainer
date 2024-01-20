using Simplified_Memory_Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SimplifiedMemoryManager
{
    public class SimpleProcessProxy : IDisposable
    {
        #region Internals
        private const int AllAccess = 0x1F0FFF;
        private bool disposedValue;

        public static Process ProcessToProxy { get; set; }
        private static string ProcessName { get; set; }
        private static IntPtr ProcessBaseAddress { get; set; }
        private static IntPtr OpenedProcessHandle { get; set; }

        public SimpleProcessProxy(Process process)
        {
            ProcessToProxy = process ?? throw new SimpleProcessProxyException("You must provide a process to modify.");
            ProcessBaseAddress = process.MainModule.BaseAddress;
            ProcessName = process.ProcessName;
        }

        private void OpenProcess()
        {
            ValidateProcessToProxy();

            OpenedProcessHandle = NativeMethods.OpenProcess(AllAccess, false, ProcessToProxy.Id);
        }

        private void ValidateProcessToProxy()
        {
            if (ProcessToProxy == null || ProcessToProxy.HasExited)
            {
                ProcessToProxy = Process.GetProcessesByName(ProcessName).FirstOrDefault();
                if (ProcessToProxy == default)
                {
                    throw new SimpleProcessProxyException($"Failed to find process {ProcessName} to proxy");
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ProcessName = null;
                ProcessToProxy = null; //TODO: determine if this does what I want
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SimpleProcessProxy()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private (support) methods
        private byte[] GetProcessSnapshot(long processSize)
        {
            byte[] buffer = new byte[processSize];
            try
            {
                OpenProcess();
                NativeMethods.ReadProcessMemory(OpenedProcessHandle, ProcessBaseAddress, buffer, (uint)buffer.Length, out int numBytesRead);
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to read process `{ProcessName}`. Is it running?", e);
            }

            return buffer;
        }

        private byte[] GetMemory(int offset, long valueSize)
        {
            try
            {
                OpenProcess();

                IntPtr addressToRead = IntPtr.Add(ProcessBaseAddress, offset);

                byte[] bytesRead = new byte[valueSize];
                ReadBytesFromMemory(addressToRead, bytesRead);

                return bytesRead;
            }
            finally
            {
                if (OpenedProcessHandle != default)
                {
                    NativeMethods.CloseHandle(OpenedProcessHandle);
                    OpenedProcessHandle = default;
                }
            }
        }

        private void SetMemory(int desiredOffset, byte[] value)
        {
            try
            {
                OpenProcess();

                IntPtr addressToModify = IntPtr.Add(ProcessBaseAddress, desiredOffset);

                WriteBytesToMemory(addressToModify, value);
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Something unexpected went wrong when trying to modify the process' memory! {e}");
            }
            finally
            {
                if (OpenedProcessHandle != default)
                {
                    NativeMethods.CloseHandle(OpenedProcessHandle);
                    OpenedProcessHandle = default;
                }
            }
        }

        private void WriteBytesToMemory(IntPtr objectAddress, byte[] bytesToWrite)
        {
            bool modificationSuccess = NativeMethods.WriteProcessMemory(OpenedProcessHandle, objectAddress, bytesToWrite, (uint)bytesToWrite.Length, out int bytesWritten);

            if (!modificationSuccess || bytesWritten != bytesToWrite.Length)
            {
                if (!modificationSuccess)
                {
                    throw new SimpleProcessProxyException("Failed to write to process memory.");
                }
                if (bytesWritten != bytesToWrite.Length)
                {
                    throw new SimpleProcessProxyException($"We tried to write {bytesToWrite.Length} bytes, but ended up writing {bytesWritten}");
                }
                throw new SimpleProcessProxyException($"Failed to write memory at {OpenedProcessHandle}+{objectAddress} with value {bytesToWrite}.");
            }
        }

        private byte[] ReadBytesFromMemory(IntPtr objectAddress, byte[] bytesToRead)
        {
            bool success = NativeMethods.ReadProcessMemory(OpenedProcessHandle, objectAddress, bytesToRead, (uint)bytesToRead.Length, out int bytesRead);

            if (!success || bytesRead != bytesToRead.Length)
            {
                if (!success)
                {
                    throw new SimpleProcessProxyException("Failed to read from process memory.");
                }
                if (bytesRead != bytesToRead.Length)
                {
                    throw new SimpleProcessProxyException($"Expected to read {bytesToRead.Length}, but we actually read {bytesRead}");
                }
                throw new SimpleProcessProxyException($"Failed to read value at {OpenedProcessHandle}+{objectAddress}.");
            }

            return bytesToRead;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Opens the proxied process, gets the current value of the designated offset, and attempts to invert its state.
        /// 
        /// If the attempt to invert the boolean fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">The offset, from index 0 of the proxied process' memory, that holds the boolean you want to invert.</param>
        /// <param name="booleanSize">If your process stores booleans with more than 1 byte, specify the byte-size here.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void InvertBooleanValue(int memoryOffset, int booleanSize = 1)
        {
            byte[] currentValue = GetMemory(memoryOffset, booleanSize);

            byte[] valueToWrite = new byte[booleanSize];

            //TODO: validate this
            if (Enumerable.SequenceEqual(currentValue, BitConverter.GetBytes(true)))
            {
                BitConverter.GetBytes(false).CopyTo(valueToWrite, 0);
            }
            else
            {
                BitConverter.GetBytes(true).CopyTo(valueToWrite, 0);
            }
            //= currentValue == BitConverter.GetBytes((short)0xFF) ? BitConverter.GetBytes((short)0xFF) : BitConverter.GetBytes((short)1);

            try
            {
                SetMemory(memoryOffset, valueToWrite);
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write boolean value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        #region ModifyProcessOffset methods
        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, short offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch(Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, int offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, long offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, double offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, float offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, bool offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, char offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, BitConverter.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, byte[] offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, offsetValueToWrite);
            }
            catch(Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Opens the proxied process and attempts to modify its memory at the designated offset with the provided value.
        /// 
        /// If the attempt to modify memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin the modification.</param>
        /// <param name="offsetValueToWrite">Value to set in memory, beginning at the memoryOffset and extending for the byte-length of the value.</param>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public void ModifyProcessOffset(int memoryOffset, string offsetValueToWrite)
        {
            try
            {
                SetMemory(memoryOffset, Encoding.Default.GetBytes(offsetValueToWrite));
            }
            catch (Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to write int value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }
        #endregion

        /// <summary>
        /// Returns the bytes found at the provided offset within the proxied process.
        /// 
        /// If the attempt to read memory fails, an exception is thrown.
        /// </summary>
        /// <param name="memoryOffset">Where in the proxied process' memory to begin reading from.</param>
        /// <param name="bytesToRead">Amount of bytes to read in and return.</param>
        /// <returns></returns>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public byte[] ReadProcessOffset(int memoryOffset, long bytesToRead)
        {
            try
            {
                return GetMemory(memoryOffset, bytesToRead);
            }
            catch(Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to read value at {OpenedProcessHandle}+{memoryOffset}", e);
            }
        }

        /// <summary>
        /// Returns a byte array representing the entirety of the proxied process' memory.
        /// </summary>
        /// <returns>Array of bytes containing the proxied process' current memory.</returns>
        /// <exception cref="SimpleProcessProxyException"></exception>
        public byte[] GetProcessSnapshot()
        {
            try
            {                
                return GetProcessSnapshot(ProcessToProxy.PrivateMemorySize64);
            }
            catch(Exception e)
            {
                throw new SimpleProcessProxyException($"Failed to get full-state of process", e);
            }
        }

        public List<IntPtr> ScanMemoryForPattern(SimplePattern pattern, byte[] memoryToScan = null)
        {
            List<IntPtr> results = new List<IntPtr>();

            if(memoryToScan == null)
                memoryToScan = GetProcessSnapshot();

            

            return results;
        }


        #endregion
    }
}
