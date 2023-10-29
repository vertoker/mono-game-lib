using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Models.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;

namespace UILib.Elements
{
    public class TextElement : UIElement
    {
        private CustomSpriteFont _font;

        public string Text { get; set; }
        public Color Color { get; set; } = Color.White;
        public TextAlignmentHorizontal Alignment { get; set; } = TextAlignmentHorizontal.Left;
        public bool Wrapping { get; set; } = true;

        public TextElement(SpriteFont font)
        {
            _font = new CustomSpriteFont(font);
        }
        public TextElement(CustomSpriteFont font)
        {
            _font = font;
        }
        public TextElement(SpriteFont font, string text)
        {
            _font = new CustomSpriteFont(font);
            Text = text;
        }
        public TextElement(CustomSpriteFont font, string text)
        {
            _font = font;
            Text = text;
        }

        public override void DrawElement(GameTime time)
        {
            UI.Batch.CameraTextRender(_font, Text, Color, Position, Rotation, Size, Anchor, Pivot, (int)UI.DepthEnumerator.Current, Alignment);
            UI.DepthEnumerator.MoveNext();
        }
    }
}
