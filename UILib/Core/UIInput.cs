using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public delegate void MouseEvent(int x, int y);
    public delegate void TouchEvent(int finger, int x, int y);
    public delegate void KeyboardEvent(Keys key);
    public delegate void GamepadEvent();

    public class UIInput : IUIUpdate, IUIDraw
    {
        public event MouseEvent MouseDown;
        public event MouseEvent MouseUp;
        public event MouseEvent MouseClick;
        public event MouseEvent MouseDoubleClick;
        public event MouseEvent MouseBeginDrag;
        public event MouseEvent MouseDrag;
        public event MouseEvent MouseEndDrag;

        public event TouchEvent TouchDown;
        public event TouchEvent TouchUp;
        public event TouchEvent TouchClick;
        public event TouchEvent TouchDoubleClick;
        public event TouchEvent TouchBeginDrag;
        public event TouchEvent TouchDrag;
        public event TouchEvent TouchEndDrag;

        public UIInput()
        {

        }

        public void Update(GameTime time)
        {

        }
        public void Draw(GameTime time)
        {

        }
    }
}
