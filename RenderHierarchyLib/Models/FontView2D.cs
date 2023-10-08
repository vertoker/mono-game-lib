using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderHierarchyLib.Models
{
    public class FontView2D
    {
        public SpriteFont Font { get; set; }

        public Color ColorTL { get; private set; } = Color.White;
        public Color ColorTR { get; private set; } = Color.White;
        public Color ColorBL { get; private set; } = Color.White;
        public Color ColorBR { get; private set; } = Color.White;


        public FontView2D(SpriteFont font)
        {
            Font = font;
        }
        public FontView2D(SpriteFont font, Color color)
            : this(font, color, color, color, color) { }
        public FontView2D(SpriteFont font, Color colorTL, Color colorTR, Color colorBL, Color colorBR)
        {
            Font = font;
            ColorTL = colorTL;
            ColorTR = colorTR;
            ColorBL = colorBL;
            ColorBR = colorBR;
        }
    }
}
