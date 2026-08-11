using Avalonia.Controls;

namespace MGS2_CheatTrainer_V2.Models
{
    //REWRITE STATUS: Not needed to update?
    internal class GuiObject
    {
        public string Name { get; protected set; }
        public Control AssociatedControl { get; protected set; }

        internal GuiObject(string objectName, Control objectControl)
        {
            Name = objectName;
            AssociatedControl = objectControl;
        }
    }
}
