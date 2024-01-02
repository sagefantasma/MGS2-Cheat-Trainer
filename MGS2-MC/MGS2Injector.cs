using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal static class MGS2Injector
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
        #endregion

        public class TrainerMenuArgs : EventArgs
        {
            public bool ActivateMenu;
        }

        static readonly ControllerHook ControllerHook = new ControllerHook();

        internal static async Task EnableInjector()
        {
            await Task.Run(StartControllerHook);
        }

        private static void StartControllerHook()
        {
            ControllerHook.TrainerMenu += OpenTrainerMenuEventHandler;
            ControllerHook.Start(CancellationToken.None); //TODO: maybe do a proper cancellation token in the future?
        }

        private static void OpenTrainerMenuEventHandler(object o, EventArgs e)
        {
            //TODO: open a GUI over MGS2 that lets the user do their desired modifications
            TrainerMenuArgs trainerMenuArgs = (TrainerMenuArgs)e;

            if (trainerMenuArgs.ActivateMenu)
            {
                SuspendMgs2();
            }
            else
            {
                ResumeMgs2();
            }
        }

        private static void SuspendMgs2()
        {
            //https://stackoverflow.com/a/71457 for how to do this
            foreach (ProcessThread mgs2Thread in Program.MGS2Process?.Threads)
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

        private static void ResumeMgs2()
        {
            //https://stackoverflow.com/a/71457 for how to do this
            foreach (ProcessThread mgs2Thread in Program.MGS2Process?.Threads)
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
    }
}
