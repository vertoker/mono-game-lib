﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public class UIContainer : UIElement
    {
        public UIContainer(UI ui)
        {
            UI = ui;
        }

        public override bool IsChild() => false;
        public override bool IsChild(out IElementChild child)
        {
            child = null;
            return false;
        }
    }
}
