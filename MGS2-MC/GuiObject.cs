using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC
{
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
