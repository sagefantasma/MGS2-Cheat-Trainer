using Serilog;
using SharpDX.XInput;
using System;
using System.Collections;
using System.Linq;
using static MGS2_MC.ControllerInterface;

namespace MGS2_MC.Controllers
{
    internal class XboxControllerManager : IControllerManager
    {
        //TODO: implement. this more than likely does not work as is.
        public bool[] HeldButtons { get; set; }
        public ILogger Logger { get; set; }
        public bool TrainerMenuActive { get; set; }

        static readonly Controller[] xInputControllerList = new[] { new Controller(UserIndex.One),
            new Controller(UserIndex.Two), new Controller(UserIndex.Three),
            new Controller(UserIndex.Four), new Controller(UserIndex.Any) };

        public class Controller : SharpDX.XInput.Controller
        {
            internal Controller(UserIndex userIndex) : base(userIndex) { }
        }

        public IEnumerable ScanForControllers()
        {
            return xInputControllerList.Where(controller => controller.IsConnected).ToList();
        }

        public void CheckControllerInput(Action<object, EventArgs> TrainerMenu, object activeController)
        {
            Controller xboxController = (Controller)activeController;
            try
            {
                State previousXboxControllerState = xboxController.GetState();
                while (xboxController.IsConnected)
                {
                    State xboxControllerState = xboxController.GetState();
                    if (previousXboxControllerState.PacketNumber != xboxControllerState.PacketNumber)
                    {
                        if (IsMenuRequestCombination(xboxControllerState))
                        {
                            TrainerMenuActive = !TrainerMenuActive;
                            TrainerMenu.Invoke(this, new TrainerMenuArgs { ActivateMenu = TrainerMenuActive }); //TODO: verify
                        }
                    }
                }
                Logger.Verbose("Xbox controller disconnected!");
            }
            catch (Exception e)
            {
                Logger.Warning($"Something unexpected went wrong with the Xbox controller: {e}");
            }
        }

        public bool IsMenuRequestCombination(object state)
        {
            State controllerState = (State) state;

            if (controllerState.Gamepad.Buttons == GamepadButtonFlags.Start &&
                controllerState.Gamepad.Buttons == GamepadButtonFlags.Back)
            {
                //this is just a placeholder case while I figure out how to do it for realsies. no way this works as is
                Logger.Verbose("Trainer menu requested!");
                return true;
            }

            return false;
        }

        public bool HaveInputsChanged(object previous, object current)
        {
            //TODO: implement
            State previousState = (State) previous;
            State currentState = (State) current;

            return false;
        }

        public void NavigateGui(object state)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
