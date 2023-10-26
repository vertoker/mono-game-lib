using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;

namespace UILib.Elements
{
    public class ImageElement : UIElement
    {
        private TextureView _view;

        public ImageElement(Texture2D texture)
        {
            _view = new TextureView(texture);
        }
        public ImageElement(TextureView view)
        {
            _view = view;
        }

        public override void Enable()
        {
            Debug.WriteLine("Enable");
        }

        public override void DrawElement(GameTime time)
        {
            UI.Batch.CameraRender(_view, Position, Rotation, Size, Anchor, Pivot, (int)UI.DepthEnumerator.Current);
            UI.DepthEnumerator.MoveNext();
        }
    }
}
