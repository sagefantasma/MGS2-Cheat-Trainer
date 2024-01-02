using Serilog;
using System;
using System.Collections;
using static MGS2_MC.MGS2Injector;

namespace MGS2_MC.Controllers
{
    internal interface IControllerManager
    {
        bool[] HeldButtons { get; set; }
        ILogger Logger { get; set; }
        bool TrainerMenuActive { get; set; }

        IEnumerable ScanForControllers();
        void CheckControllerInput(Action<object, EventArgs> action, object controller);
        bool IsMenuRequestCombination(object structure);
        bool HaveButtonInputsChanged(object previousState, object currentState);
        void NavigateGui(object currentState);
    }
}
