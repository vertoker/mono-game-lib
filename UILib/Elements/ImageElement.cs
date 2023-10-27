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
        public Texture2D Texture { get; set; }
        public Vector2 ViewStart { get; set; } = Vector2.Zero;
        public Vector2 ViewEnd { get; set; } = Vector2.One;
        public Color Color { get; set; } = Color.White;

        public ImageElement(Texture2D texture)
        {
            Texture = texture;
        }
        public ImageElement(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            ViewStart = new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width);
            ViewEnd = new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width);
        }
        public ImageElement(Texture2D texture, Vector2 viewStart, Vector2 viewEnd)
        {
            Texture = texture;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
        }

        public ImageElement(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }
        public ImageElement(Texture2D texture, Color color, Rectangle rectangle)
        {
            Texture = texture;
            Color = color;
            ViewStart = new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width);
            ViewEnd = new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width);
        }
        public ImageElement(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd)
        {
            Texture = texture;
            Color = color;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
        }

        public override void DrawElement(GameTime time)
        {
            UI.Batch.CameraRender(Texture, Color, ViewStart, ViewEnd, Position, Rotation, Size, Anchor, Pivot, (int)UI.DepthEnumerator.Current);
            UI.DepthEnumerator.MoveNext();
        }
    }
}
