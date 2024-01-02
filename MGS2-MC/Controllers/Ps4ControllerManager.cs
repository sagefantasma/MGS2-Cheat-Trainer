using Serilog;
using SharpDX.DirectInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MGS2_MC.MGS2Injector;

namespace MGS2_MC.Controllers
{
    internal class Ps4ControllerManager : IControllerManager
    {
        public bool[] HeldButtons 
        { 
            get 
            { 
                return (bool[]) HeldButtons.Clone(); //this is safe as booleans are value types, not references types
            } 
            set 
            {
                if (HeldButtons == null)
                    HeldButtons = new bool[13];
                else
                    return;
            } 
        }
        public ILogger Logger { get; set; }
        public bool TrainerMenuActive { get; set; }

        private static readonly DirectInput directInput = new DirectInput();

        public class Controller
        {
            public Guid Guid { get; set; }
            public string Name { get; set; }

            internal Controller(Guid controllerGuid, string controllerName)
            {
                Guid = controllerGuid;
                Name = controllerName;
            }
        }

        public IEnumerable ScanForControllers()
        {
            List<Controller> ps4Controllers = new List<Controller>();
            foreach (DeviceInstance deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
            {
                ps4Controllers.Add(new Controller(deviceInstance.InstanceGuid, deviceInstance.ProductName));
            }

            return ps4Controllers;
        }

        public bool HaveButtonInputsChanged(object previous, object current)
        {
            JoystickState previousState = previous as JoystickState;
            JoystickState currentState = current as JoystickState;

            for (int i = 0; i < previousState.Buttons.Count(); i++)
            {
                if (previousState.Buttons[i] != currentState.Buttons[i])
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMenuRequestCombination(object state)
        {
            JoystickState controllerState = (JoystickState) state;
            //button 0 is Square, button 1 is Cross, button 2 is Circle, button 3 is Triangle
            //button 4 is L1, button 5 is R1, button 6 is L2, button 7 is R2
            //button 8 is share, 9 is options, button 10 is L3, button 11 is R3
            //button 12 is PS button, button 13 is touchpad
            if (controllerState.Buttons[8] && controllerState.Buttons[9] && //share & options are pressed
               (HeldButtons[8] == false || HeldButtons[9] == false)) //and they are not being held
            {
                Logger.Verbose("Trainer menu requested!");
                return true;
            }

            return false;
        }

        public void CheckControllerInput(Action<object, EventArgs> TrainerMenu, object activeController)
        {
            Controller ps4Controller = (Controller)activeController;
            try
            {
                using (Joystick ps4Joy = new Joystick(directInput, ps4Controller.Guid))
                {
                    ps4Joy.Acquire();
                    ps4Joy.Poll();
                    JoystickState previousState = ps4Joy.GetCurrentState();
                    while (true)
                    {
                        ps4Joy.Acquire();
                        ps4Joy.Poll();
                        JoystickState currentState = ps4Joy.GetCurrentState();
                        UpdateHolds(previousState, currentState);

                        if (HaveButtonInputsChanged(previousState, currentState))
                        {
                            if (IsMenuRequestCombination(currentState))
                            {
                                TrainerMenuActive = !TrainerMenuActive;
                                TrainerMenu.Invoke(this, new TrainerMenuArgs { ActivateMenu = TrainerMenuActive }); //TODO: verify
                            }
                        }

                        previousState = currentState;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Verbose("PS4 controller disconnected!"); //TODO: get the EXACT exception for this case and put it in its own safe catch
                Logger.Warning($"Something unexpected went wrong with the PS4 controller: {e}");
            }
        }

        private void UpdateHolds(JoystickState previousState, JoystickState state)
        {
            for(int i = 0; i < HeldButtons.Length; i++)
            {
                if (previousState.Buttons[i] && state.Buttons[i])
                    HeldButtons[i] = true;
                else
                    HeldButtons[i] = false;
            }
        }
    }
}
