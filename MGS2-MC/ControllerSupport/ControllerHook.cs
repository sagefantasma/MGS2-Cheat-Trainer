using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Serilog;
using MGS2_MC.Controllers;


namespace MGS2_MC
{
    internal class ControllerHook
    {
        public event EventHandler TrainerMenu;
        private const string loggerName = "ControllerDebuglog.log";

        static XboxControllerManager _xboxControllerManager { get; set; } = new XboxControllerManager();
        static Ps4ControllerManager _ps4ControllerManager { get; set; } = new Ps4ControllerManager();
        private bool _activeControllerFound { get; set; } = false;
        private object _activeController { get; set; }
        private static ILogger _logger { get; set; }

        //TODO: realistically, we shouldn't have separate controller managers with the interface, so do that.

        private object FindActiveController()
        {
            List<XboxControllerManager.Controller> activeXboxControllers = (List<XboxControllerManager.Controller>) _xboxControllerManager.ScanForControllers();
            List<Ps4ControllerManager.Controller> activePs4Controllers = (List<Ps4ControllerManager.Controller>) _ps4ControllerManager.ScanForControllers();

            if(activeXboxControllers.Count == 0 && activePs4Controllers.Count == 0)
            {
                _logger.Information("No controllers connected");
                return null;
            }
            else if(activeXboxControllers.Count == 1 && activePs4Controllers.Count < 1)
            {
                _logger.Information("One Xbox controller connected, using that as the active controller");
                return activeXboxControllers[0];
            }
            else if(activePs4Controllers.Count == 1 && activeXboxControllers.Count < 1)
            {
                _logger.Information("One PS4 controller connected, using that as the active controller");
                return activePs4Controllers[0];
            }
            else
            {
                if(activePs4Controllers.Any(controller => controller.Name == "Wireless Controller"))
                {
                    _logger.Information("Multiple DirectInput controllers detected, using the one named 'Wireless Controller' as the active controller");
                    return activePs4Controllers.FirstOrDefault(controller => controller.Name == "Wireless Controller");
                }
                //do something to figure out the "active" controller??
                _logger.Debug("We see multiple controllers(but no Ps4 controller) connected and don't know what to do yet. Connect a Ps4 controller or connect only 1 controller");
                return null;
            }
        }

        internal void Start(CancellationToken cancellationToken)
        {
            _logger = Logging.InitializeNewLogger(loggerName);
            _ps4ControllerManager.Logger = _logger;
            _xboxControllerManager.Logger = _logger;
            _logger.Information($"Controller hook for version {Program.AppVersion} initialized...");
            _logger.Verbose($"Instance ID: {Program.InstanceID}");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_activeControllerFound)
                {
                    object controller = FindActiveController();
                    if (controller == null)
                    { 
                        continue; 
                    }
                    else
                    {
                        _activeControllerFound = true;
                        _activeController = controller;
                    }
                }
                else
                {
                    if (_activeController.GetType() == typeof(XboxControllerManager.Controller))
                    {
                        _xboxControllerManager.CheckControllerInput(TrainerMenuEventHandler, _activeController);
                    }
                    else
                    {
                        _ps4ControllerManager.CheckControllerInput(TrainerMenuEventHandler, _activeController);
                    }
                }
            }
            _logger.Information("Controller hook ending...");
        }              

        protected virtual void TrainerMenuEventHandler(object sender, EventArgs e)
        {
            TrainerMenu?.Invoke(sender, e);
        }
    }
}
