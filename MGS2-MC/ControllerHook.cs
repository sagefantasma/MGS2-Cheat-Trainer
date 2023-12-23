using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.XInput;


namespace MGS2_MC
{

    internal class ControllerHook
    {
        //Controller[] controllerList = new[] {new Controller(UserIndex.One), ...}
        Controller controller = new Controller(UserIndex.Any);
        public event EventHandler TrainerMenu;

        internal void Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                State previousControllerState = controller.GetState();
                while (controller.IsConnected)
                {
                    State controllerState = controller.GetState();
                    if(previousControllerState.PacketNumber != controllerState.PacketNumber)
                    {
                        if (IsMenuRequestCombination(controllerState))
                        {
                            OnTrainerMenu(EventArgs.Empty); //trigger the event for the injector to leverage
                        }
                    }
                }
            }
        }

        private bool IsMenuRequestCombination(State controllerState)
        {
            if(controllerState.Gamepad.Buttons == GamepadButtonFlags.Start && 
                controllerState.Gamepad.Buttons == GamepadButtonFlags.Back)
            {
                //this is just a placeholder case while I figure out how to do it for realsies. no way this works as is
                return true;
            }

            return false;
        }       

        protected virtual void OnTrainerMenu(EventArgs e)
        {
            TrainerMenu?.Invoke(this, e);
        }
    }
}
