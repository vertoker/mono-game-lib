using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UILib
{
    public delegate void ElementEvent(UIElement sender);

    public class UIElement
    {
        public event ElementEvent Hovering;
        public event ElementEvent RightPressed;
        public event ElementEvent Hover;
        public event ElementEvent HoverLeft;
        public event ElementEvent Clicked;
        public event ElementEvent RightClicked;
        public event ElementEvent ResetState;
        public event ElementEvent Disabled;
        public event ElementEvent Enabled;
    }
}
