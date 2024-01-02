using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal static class MGS2Injector
    {
        static ControllerHook ControllerHook = new ControllerHook();

        internal static async Task EnableInjector()
        {
            await Task.Run(StartControllerHook);
        }

        private static void StartControllerHook()
        {
            ControllerHook.TrainerMenu += OpenTrainerMenuEventHandler;
            ControllerHook.Start(System.Threading.CancellationToken.None); //TODO: maybe do a proper cancellation token in the future?
        }

        private static void OpenTrainerMenuEventHandler(object o, EventArgs e)
        {
            //TODO: open a GUI over MGS2 that lets the user do their desired modifications
        }
    }
}
