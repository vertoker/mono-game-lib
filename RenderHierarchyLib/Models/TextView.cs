using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Models.Text;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace RenderHierarchyLib.Models
{
    public class TextView
    {
        public string Text { get; set; }
        public CustomSpriteFont Font { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        public List<TextColorIndexes> Colors { get; set; } = new List<TextColorIndexes>();

        public TextView(string text, SpriteFont font)
        {
            Text = text;
            Font = new CustomSpriteFont(font);
        }
        public TextView(string text, CustomSpriteFont font)
        {
            Text = text;
            Font = font;
        }
        public TextView(string text, SpriteFont font, Color defaultColor) : this(text, font)
        {
            DefaultColor = defaultColor;
        }
        public TextView(string text, CustomSpriteFont font, Color defaultColor) : this(text, font)
        {
            DefaultColor = defaultColor;
        }
        public TextView(string text, SpriteFont font, Color defaultColor, List<TextColorIndexes> colors) : this(text, font, defaultColor)
        {
            Colors = colors;
        }
        public TextView(string text, CustomSpriteFont font, Color defaultColor, List<TextColorIndexes> colors) : this(text, font, defaultColor)
        {
            Colors = colors;
        }
    }
}
