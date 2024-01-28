using System;
using System.Threading;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal static class ControllerInterface
    {
        public class TrainerMenuArgs : EventArgs
        {
            public bool ActivateMenu;
        }

        static readonly ControllerHook ControllerHook = new ControllerHook();

        internal static void EnableInjector()
        {
            Task.Run(StartControllerHook);
        }

        private static void StartControllerHook()
        {
            ControllerHook.TrainerMenu += OpenTrainerMenuEventHandler;
            ControllerHook.Start(CancellationToken.None); //TODO: maybe do a proper cancellation token in the future?
        }

        private static void OpenTrainerMenuEventHandler(object o, EventArgs e)
        {            
            TrainerMenuArgs trainerMenuArgs = (TrainerMenuArgs)e;

            if (trainerMenuArgs.ActivateMenu)
            {
                MGS2Monitor.SuspendMGS2();
                //bool gotMgs2Window = GetWindowRect(Program.MGS2Process.MainWindowHandle, out Rectangle mgs2WindowRectangle);
                //TODO: open a GUI over MGS2 that lets the user do their desired modifications... for now, just enable navigating the GUI w/ buttons
                GUI.CanNavigateWithController = true;
                GUI.ShowGui();
            }
            else
            {
                GUI.CanNavigateWithController = false;
                GUI.HideGui();
                MGS2Monitor.ResumeMGS2();
            }
        }
    }
}
