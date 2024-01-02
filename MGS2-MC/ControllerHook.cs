using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.XInput;
using SharpDX.DirectInput;


namespace MGS2_MC
{


    internal class ControllerHook
    {
        private class Ps4Controller
        {
            public Guid Guid { get; set; }
            public string Name { get; set; }

            public Ps4Controller(Guid controllerGuid, string controllerName)
            {
                Guid = controllerGuid;
                Name = controllerName;
            }
        }

        //Controller[] controllerList = new[] {new Controller(UserIndex.One), ...}
        Controller[] xboxControllerList = new[] { new Controller(UserIndex.One), 
            new Controller(UserIndex.Two), new Controller(UserIndex.Three), 
            new Controller(UserIndex.Four), new Controller(UserIndex.Any) };
        DirectInput directInput = new DirectInput();
        private bool activeControllerFound = false;
        private object activeController;
        public event EventHandler TrainerMenu;

        private List<Ps4Controller> ScanForPs4Controllers()
        {
            var joystickGuid = Guid.Empty;
            List<Ps4Controller> ps4Controllers = new List<Ps4Controller>();
            foreach(DeviceInstance deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
            {
                ps4Controllers.Add(new Ps4Controller(deviceInstance.InstanceGuid, deviceInstance.ProductName));
            }

            return ps4Controllers;
        }

        private object FindActiveController()
        {
            List<Controller> activeXboxControllers = xboxControllerList.Where(controller => controller.IsConnected).ToList();
            List<Ps4Controller> activePs4Controllers = ScanForPs4Controllers();

            if(activeXboxControllers.Count == 0 && activePs4Controllers.Count == 0)
            {
                return null;
            }
            else if(activeXboxControllers.Count == 1 && activePs4Controllers.Count < 1)
            {
                return activeXboxControllers[0];
            }
            else if(activePs4Controllers.Count == 1 && activeXboxControllers.Count < 1)
            {
                return activePs4Controllers[0];
            }
            else
            {
                if(activePs4Controllers.Any(controller => controller.Name == "Wireless Controller"))
                {
                    return activePs4Controllers.FirstOrDefault(controller => controller.Name == "Wireless Controller");
                }
                //do something to figure out the "active" controller??
                return null;
            }
        }

        internal void Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!activeControllerFound)
                {
                    object controller = FindActiveController();
                    if (controller == null)
                    { 
                        continue; 
                    }
                    else
                    {
                        activeControllerFound = true;
                        activeController = controller;
                    }
                }
                else
                {
                    if (activeController.GetType() == typeof(Controller))
                    {
                        CheckXboxControllerInput();
                    }
                    else
                    {
                        CheckPs4ControllerInput();
                    }
                }
            }
        }

        private void CheckXboxControllerInput()
        {
            Controller xboxController = (Controller)activeController;
            State previousXboxControllerState = xboxController.GetState();
            while (xboxController.IsConnected)
            {
                State xboxControllerState = xboxController.GetState();
                if (previousXboxControllerState.PacketNumber != xboxControllerState.PacketNumber)
                {
                    if (IsXboxControllerMenuRequestCombination(xboxControllerState))
                    {
                        InvokeTrainerMenu(this, EventArgs.Empty); //trigger the event for the injector to leverage
                    }
                }
            }
        }

        private void CheckPs4ControllerInput()
        {
            Ps4Controller ps4Controller = (Ps4Controller)activeController;
            Joystick ps4Joy = new Joystick(directInput, ps4Controller.Guid);
            ps4Joy.Acquire();
            ps4Joy.Poll();
            JoystickState previousPs4ControllerState = ps4Joy.GetCurrentState();
            while (true)
            {
                //TODO: add some kind of way to determine if the controller is still connected?
                JoystickState ps4ControllerState = ps4Joy.GetCurrentState();

                if (previousPs4ControllerState != ps4ControllerState)
                {
                    if (IsPs4ControllerMenuRequestCombination(ps4ControllerState))
                    {
                        InvokeTrainerMenu(this, EventArgs.Empty);
                    }
                }
            }
            ps4Joy.Unacquire();
        }

        private bool IsPs4ControllerMenuRequestCombination(JoystickState controllerState)
        {
            //button 8 is share, 9 is options
            if (controllerState.Buttons[8] && controllerState.Buttons[9])
                return true;
            //if (controllerState.Buttons[])
            return false;
        }

        private bool IsXboxControllerMenuRequestCombination(State controllerState)
        {
            if(controllerState.Gamepad.Buttons == GamepadButtonFlags.Start && 
                controllerState.Gamepad.Buttons == GamepadButtonFlags.Back)
            {
                //this is just a placeholder case while I figure out how to do it for realsies. no way this works as is
                return true;
            }

            return false;
        }       

        protected virtual void InvokeTrainerMenu(object sender, EventArgs e)
        {
            TrainerMenu?.Invoke(sender, e);
        }
    }
}
