using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Models.Text;
using RenderHierarchyLib.Render.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Extensions
{
    public static class SpriteFontExtensions
    {
        public static CustomSpriteFont.Glyph[] GetGlyphsArray(this SpriteFont font)
        {
            var length = font.Glyphs.Length;
            var glyphs = new CustomSpriteFont.Glyph[length];

            for ( var i = 0; i < length; i++)
                glyphs[i] = new CustomSpriteFont.Glyph(font.Glyphs[i]);

            return glyphs;
        }

        public static CharacterRegion[] GetCharacterRegions(this SpriteFont font)
        {
            var stack = new Stack<CharacterRegion>();

            for (int i = 0; i < stack.Count; i++)
            {
                var glyph = font.Glyphs[i];
                if (stack.Count == 0 || glyph.Character > stack.Peek().End + 1)
                {
                    stack.Push(new CharacterRegion(glyph.Character, i));
                    continue;
                }

                if (glyph.Character == stack.Peek().End + 1)
                {
                    CharacterRegion item = stack.Pop();
                    item.End += '\u0001';
                    stack.Push(item);
                    continue;
                }

                throw new InvalidOperationException("Invalid SpriteFont. Character map must be in ascending order.");
            }

            var regions = stack.ToArray();
            Array.Reverse(regions);
            return regions;
        }
    }
}
