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
        XboxControllerManager xboxControllerManager = new XboxControllerManager();
        Ps4ControllerManager ps4ControllerManager = new Ps4ControllerManager();
        private bool activeControllerFound = false;
        private object activeController;
        public event EventHandler TrainerMenu;
        private static ILogger logger;
        private const string loggerName = "MGS2CheatTrainerControllerDebuglog.log";

        //TODO: realistically, we shouldn't have separate controller managers with the interface, so do that.

        private object FindActiveController()
        {
            List<XboxControllerManager.Controller> activeXboxControllers = (List<XboxControllerManager.Controller>) xboxControllerManager.ScanForControllers();
            List<Ps4ControllerManager.Controller> activePs4Controllers = (List<Ps4ControllerManager.Controller>) ps4ControllerManager.ScanForControllers();

            if(activeXboxControllers.Count == 0 && activePs4Controllers.Count == 0)
            {
                logger.Information("No controllers connected");
                return null;
            }
            else if(activeXboxControllers.Count == 1 && activePs4Controllers.Count < 1)
            {
                logger.Information("One Xbox controller connected, using that as the active controller");
                return activeXboxControllers[0];
            }
            else if(activePs4Controllers.Count == 1 && activeXboxControllers.Count < 1)
            {
                logger.Information("One PS4 controller connected, using that as the active controller");
                return activePs4Controllers[0];
            }
            else
            {
                if(activePs4Controllers.Any(controller => controller.Name == "Wireless Controller"))
                {
                    logger.Information("Multiple DirectInput controllers detected, using the one named 'Wireless Controller' as the active controller");
                    return activePs4Controllers.FirstOrDefault(controller => controller.Name == "Wireless Controller");
                }
                //do something to figure out the "active" controller??
                logger.Debug("We see multiple controllers(but no Ps4 controller) connected and don't know what to do yet. Connect a Ps4 controller or connect only 1 controller");
                return null;
            }
        }

        internal void Start(CancellationToken cancellationToken)
        {
            logger = Logging.InitializeLogger(loggerName, "Verbose");
            ps4ControllerManager.Logger = logger;
            xboxControllerManager.Logger = logger;
            logger.Information("Controller hook starting...");
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
                    if (activeController.GetType() == typeof(XboxControllerManager.Controller))
                    {
                        xboxControllerManager.CheckControllerInput(TrainerMenuEventHandler, activeController);
                    }
                    else
                    {
                        ps4ControllerManager.CheckControllerInput(TrainerMenuEventHandler, activeController);
                    }
                }
            }
            logger.Information("Controller hook ending...");
        }              

        protected virtual void TrainerMenuEventHandler(object sender, EventArgs e)
        {
            TrainerMenu?.Invoke(sender, e);
        }
    }
}
