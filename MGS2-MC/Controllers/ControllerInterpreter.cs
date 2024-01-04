namespace MGS2_MC.Controllers
{
    internal static class ControllerInterpreter
    {
        //using Playstation2 format as the "base" because MGS2 was originally a Ps2 game :^)
        public enum PressedButton
        {
            Cross,
            Circle,
            Triangle,
            Square,
            L1,
            L2,
            L3,
            R1,
            R2,
            R3,
            Select,
            Start,
            UpDirectional,
            RightDirectional,
            LeftDirectional,
            DownDirectional,
            None
        }

        internal static void CrossButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Cross);
        }

        internal static void CircleButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Circle);
        }

        internal static void SquareButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Square);
        }

        internal static void TriangleButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Triangle);
        }

        internal static void L1ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.L1);
        }

        internal static void L2ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.L2);
        }

        internal static void L3ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.L3);
        }

        internal static void R1ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.R1);
        }

        internal static void R2ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.R2);
        }

        internal static void R3ButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.R3);
        }

        internal static void StartButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Start);
        }

        internal static void SelectButtonPressed()
        {
            GUI.NavigateViaController(PressedButton.Select);
        }

        internal static void UpDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.UpDirectional);
        }

        internal static void UpAndRightDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.UpDirectional, PressedButton.RightDirectional);
        }

        internal static void RightDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.RightDirectional);
        }

        internal static void DownAndRightDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.DownDirectional, PressedButton.RightDirectional);
        }

        internal static void DownDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.DownDirectional);
        }

        internal static void DownAndLeftDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.DownDirectional, PressedButton.LeftDirectional);
        }

        internal static void LeftDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.LeftDirectional);
        }

        internal static void UpAndLeftDirectionalPressed()
        {
            GUI.NavigateViaController(PressedButton.UpDirectional, PressedButton.LeftDirectional);
        }
    }
}
