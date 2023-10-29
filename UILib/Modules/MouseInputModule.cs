using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderHierarchyLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;
using UILib.Interfaces.Core;

namespace UILib.Modules
{
    public class MouseInputModule : IUIModule
    {
        public bool DrawCursor { get; set; } = true;
        public Texture2D CursorTexture { get; }
        public UI UI { get; private set; }

        public MouseInputModule(Texture2D cursorTexture)
        {
            CursorTexture = cursorTexture;
            //var cursor = MouseCursor.FromTexture2D(CursorTexture);
        }

        public void Initialize(UI ui)
        {
            UI = ui;
        }
        public void Dispose()
        {
            UI = null;
        }

        public void Draw(GameTime time)
        {
            var state = Mouse.GetState();
            //var pos = new Vector2(state.X, state.Y) / UI.Batch.Camera.GraphicsManager.PreferredBackBufferHeight * UI.Batch.Camera.PixelScale;
            var pos = new Vector2((float)time.TotalGameTime.TotalSeconds, 0);
            Debug.WriteLine(pos);

            UI.Batch.CameraRender(CursorTexture, Color.Red, Vector2.Zero, Vector2.One, pos, 0, Vector2.One, 
                AnchorPresets.CenterMiddle, PivotPresets.CenterMiddle, (int)UI.DepthEnumerator.Current);
            UI.DepthEnumerator.MoveNext();
        }
        public void Update(GameTime time)
        {

        }
    }
}
