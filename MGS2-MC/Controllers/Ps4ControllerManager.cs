using Serilog;
using SharpDX.DirectInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MGS2_MC.ControllerInterface;

namespace MGS2_MC.Controllers
{
    internal class Ps4ControllerManager : IControllerManager
    {
        private bool[] _heldButtons { get; set; }
        public bool[] HeldButtons 
        { 
            get 
            {
                if (_heldButtons == null)
                    _heldButtons = new bool[13];

                return _heldButtons;
            } 
            set 
            {
                if (_heldButtons == null)
                    _heldButtons = new bool[13];
                else
                    return;
            } 
        }
        public ILogger Logger { get; set; }
        public bool TrainerMenuActive { get; set; }

        private static readonly DirectInput directInput = new DirectInput();

        internal enum Ps4Button
        {
            //button 0 is Square, button 1 is Cross, button 2 is Circle, button 3 is Triangle
            //button 4 is L1, button 5 is R1, button 6 is L2, button 7 is R2
            //button 8 is share, 9 is options, button 10 is L3, button 11 is R3
            //button 12 is PS button, button 13 is touchpad
            Square = 0,
            Cross = 1,
            Circle = 2,
            Triangle = 3,
            L1 = 4,
            R1 = 5,
            L2 = 6,
            R2 = 7,
            Share = 8,
            Options = 9,
            L3 = 10,
            R3 = 11,
            PS = 12,
            Touch = 13
        }

        internal enum DirectionalPad
        {
            Up = 0,
            UpRight = 4500,
            Right = 9000,
            DownRight = 13500,
            Down = 18000,
            DownLeft = 22500,
            Left = 27000,
            UpLeft = 31500
        }

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

        public bool HaveInputsChanged(object previous, object current)
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

            for(int i = 0; i < previousState.PointOfViewControllers.Length; i++)
            {
                if (previousState.PointOfViewControllers[i] != currentState.PointOfViewControllers[i])
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMenuRequestCombination(object state)
        {
            JoystickState controllerState = (JoystickState) state;
            if (controllerState.Buttons[(int) Ps4Button.Share] && controllerState.Buttons[(int) Ps4Button.Options] && //share & options are pressed
               (HeldButtons[(int) Ps4Button.Share] == false || HeldButtons[(int) Ps4Button.Options] == false)) //and they are not being held
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

                        if (HaveInputsChanged(previousState, currentState))
                        {
                            if (IsMenuRequestCombination(currentState))
                            {
                                TrainerMenuActive = !TrainerMenuActive;
                                TrainerMenu.Invoke(this, new TrainerMenuArgs { ActivateMenu = TrainerMenuActive }); //TODO: verify
                            }
                            else if (TrainerMenuActive)
                            {
                                NavigateGui(currentState);
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

        public void NavigateGui(object currentState)
        {
            JoystickState joystickState = currentState as JoystickState;
            if (joystickState.Buttons[(int)Ps4Button.Square])
            {
                ControllerInterpreter.SquareButtonPressed();
            }
            else if (joystickState.Buttons[(int) Ps4Button.Cross])
            {
                ControllerInterpreter.CrossButtonPressed();
            }
            else if (joystickState.Buttons[(int) Ps4Button.Circle])
            {
                ControllerInterpreter.CircleButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.Triangle])
            {
                ControllerInterpreter.TriangleButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.L1])
            {
                ControllerInterpreter.L1ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.R1])
            {
                ControllerInterpreter.R1ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.L2])
            {
                ControllerInterpreter.L2ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.R2])
            {
                ControllerInterpreter.R2ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.L3])
            {
                ControllerInterpreter.L3ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.R3])
            {
                ControllerInterpreter.R3ButtonPressed();
            }
            else if (joystickState.Buttons[(int)Ps4Button.PS])
            {
                //nothing planned
            }
            else if (joystickState.Buttons[(int)Ps4Button.Touch])
            {
                //nothing planned
            }
            else if (joystickState.PointOfViewControllers[0] != -1) 
            {
                switch (joystickState.PointOfViewControllers[0])
                {
                    case (int)DirectionalPad.Up:
                        ControllerInterpreter.UpDirectionalPressed();
                        break;
                    case (int)DirectionalPad.UpRight:
                        ControllerInterpreter.UpAndRightDirectionalPressed();
                        break;
                    case (int)DirectionalPad.Right:
                        ControllerInterpreter.RightDirectionalPressed();
                        break;
                    case (int)DirectionalPad.DownRight:
                        ControllerInterpreter.DownAndRightDirectionalPressed();
                        break;
                    case (int)DirectionalPad.Down:
                        ControllerInterpreter.DownDirectionalPressed();
                        break;
                    case (int)DirectionalPad.DownLeft:
                        ControllerInterpreter.DownAndLeftDirectionalPressed();
                        break;
                    case (int)DirectionalPad.Left:
                        ControllerInterpreter.LeftDirectionalPressed();
                        break;
                    case (int)DirectionalPad.UpLeft:
                        ControllerInterpreter.UpAndLeftDirectionalPressed();
                        break;
                }
            }
            //else if (joystickState.X // this is left/right on left analog (33288 is unmoving, less is left, more is right)
            //else if (joystickState.Y // this is up/down on left analog (32255 is unmoving, less is up, more is down)
            //else if (joystickState.RotationZ AND joystickState.Z // this is right stick
        }
    }
}
