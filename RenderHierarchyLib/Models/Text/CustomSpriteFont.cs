using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Render.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Xna.Framework.Graphics.SpriteFont;

namespace RenderHierarchyLib.Models.Text
{
    public partial class CustomSpriteFont
    {
        public static class Errors
        {
            public const string TextContainsUnresolvableCharacters = "Text contains characters that cannot be resolved by this SpriteFont.";

            public const string UnresolvableCharacter = "Character cannot be resolved by this SpriteFont.";
        }
        public class CharComparer : IEqualityComparer<char>
        {
            public static readonly CharComparer Default = new CharComparer();

            public bool Equals(char x, char y)
            {
                return x == y;
            }

            public int GetHashCode(char b)
            {
                return b;
            }
        }

        public struct Glyph
        {
            public char Character;
            public Rectangle BoundsInTexture;

            public float LeftBearing;
            public float RightBearing;
            public float Height;
            public float Width;

            public float WidthIncludingBearings => LeftBearing + Width + RightBearing;

            public static readonly Glyph Empty;

            public Glyph(SpriteFont.Glyph glyph)
            {
                Character = glyph.Character;
                BoundsInTexture = glyph.BoundsInTexture;
                LeftBearing = glyph.LeftSideBearing;
                RightBearing = glyph.RightSideBearing;
                Height = glyph.Cropping.Height;
                Width = glyph.Width;
            }

            public override readonly string ToString()
            {
                return string.Join(',',
                    $"{nameof(Character)}={Character}",
                    $"{nameof(BoundsInTexture)}={BoundsInTexture}",
                    $"{nameof(LeftBearing)}={LeftBearing}",
                    $"{nameof(RightBearing)}={RightBearing}",
                    $"{nameof(Height)}={Height}",
                    $"{nameof(Width)}={Width}");
            }
        }

        private readonly Glyph[] _glyphs;
        private readonly CharacterRegion[] _regions;
        private char? _defaultCharacter;
        private int _defaultGlyphIndex = -1;
        private readonly Texture2D _texture;

        public Glyph[] Glyphs => _glyphs;
        public CharacterRegion[] Regions => _regions;
        public Texture2D Texture => _texture;
        public ReadOnlyCollection<char> Characters { get; private set; }

        public float WidthSpacing { get; set; }
        public float HeightSpacing { get; set; }

        public char? DefaultCharacter
        {
            get
            {
                return _defaultCharacter;
            }
            set
            {
                if (value.HasValue)
                {
                    if (!TryGetGlyphIndex(value.Value, out _defaultGlyphIndex))
                        throw new ArgumentException("Character cannot be resolved by this SpriteFont.");
                }
                else
                {
                    _defaultGlyphIndex = -1;
                }

                _defaultCharacter = value;
            }
        }

        public SpriteFont DefaultFont { get; set; } = null;


        public CustomSpriteFont(SpriteFont font)
        {
            Characters = font.Characters;
            _texture = font.Texture;
            HeightSpacing = font.LineSpacing;
            WidthSpacing = font.Spacing;
            _glyphs = font.GetGlyphsArray();
            _regions = font.GetCharacterRegions();
            DefaultCharacter = font.DefaultCharacter;
            DefaultFont = font;
        }

        public CustomSpriteFont(Texture2D texture, List<Rectangle> glyphBounds, List<Rectangle> cropping, List<char> characters, int lineSpacing, float spacing, List<Vector3> kerning, char? defaultCharacter)
        {
            Characters = new ReadOnlyCollection<char>(characters.ToArray());
            _texture = texture;
            HeightSpacing = lineSpacing;
            WidthSpacing = spacing;
            _glyphs = new Glyph[characters.Count];
            var stack = new Stack<CharacterRegion>();
            for (int i = 0; i < characters.Count; i++)
            {
                _glyphs[i] = new Glyph
                {
                    Character = characters[i],
                    BoundsInTexture = glyphBounds[i],
                    LeftBearing = kerning[i].X,
                    RightBearing = kerning[i].Z,
                    Width = kerning[i].Y,
                    Height = cropping[i].Height
                };
                if (stack.Count == 0 || characters[i] > stack.Peek().End + 1)
                {
                    stack.Push(new CharacterRegion(characters[i], i));
                    continue;
                }
                if (characters[i] == stack.Peek().End + 1)
                {
                    CharacterRegion item = stack.Pop();
                    item.End += '\u0001';
                    stack.Push(item);
                    continue;
                }

                throw new InvalidOperationException("Invalid SpriteFont. Character map must be in ascending order.");
            }

            _regions = stack.ToArray();
            Array.Reverse(_regions);
            DefaultCharacter = defaultCharacter;
        }

        public Dictionary<char, Glyph> GetGlyphs()
        {
            Dictionary<char, Glyph> dictionary = new Dictionary<char, Glyph>(_glyphs.Length, CharComparer.Default);
            Glyph[] glyphs = _glyphs;
            for (int i = 0; i < glyphs.Length; i++)
            {
                Glyph value = glyphs[i];
                dictionary.Add(value.Character, value);
            }

            return dictionary;
        }


        public unsafe void MeasureString(ref string text, out Vector2 size)
        {
            if (text.Length == 0)
            {
                size = Vector2.Zero;
                return;
            }

            float sizeX = 0f;
            float num2 = HeightSpacing;
            Vector2 zero = Vector2.Zero;
            var flagLines = true;
            fixed (Glyph* ptr = Glyphs)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    switch (c)
                    {
                        case '\n':
                            num2 = HeightSpacing;
                            zero.X = 0f;
                            zero.Y += HeightSpacing;
                            flagLines = true;
                            continue;
                        case '\r':
                            continue;
                    }

                    int glyphIndexOrDefault = GetGlyphIndexOrDefault(c);
                    Glyph* ptr2 = ptr + glyphIndexOrDefault;
                    if (flagLines)
                    {
                        zero.X = Math.Max(ptr2->LeftBearing, 0f);
                        flagLines = false;
                    }
                    else
                    {
                        zero.X += WidthSpacing + ptr2->LeftBearing;
                    }

                    zero.X += ptr2->Width;
                    float num3 = zero.X + Math.Max(ptr2->RightBearing, 0f);
                    if (num3 > sizeX)
                    {
                        sizeX = num3;
                    }

                    zero.X += ptr2->RightBearing;
                    if (ptr2->Height > num2)
                    {
                        num2 = ptr2->Height;
                    }
                }
            }

            size.X = sizeX;
            size.Y = zero.Y + num2;
        }
        public unsafe bool TryGetGlyphIndex(char c, out int index)
        {
            fixed (CharacterRegion* ptr = _regions)
            {
                int num = -1;
                int num2 = 0;
                int num3 = _regions.Length - 1;
                while (num2 <= num3)
                {
                    int num4 = num2 + num3 >> 1;
                    if (ptr[num4].End < c)
                    {
                        num2 = num4 + 1;
                        continue;
                    }

                    if (ptr[num4].Start > c)
                    {
                        num3 = num4 - 1;
                        continue;
                    }

                    num = num4;
                    break;
                }

                if (num == -1)
                {
                    index = -1;
                    return false;
                }

                index = ptr[num].StartIndex + (c - ptr[num].Start);
            }

            return true;
        }

        public int GetGlyphIndexOrDefault(char c)
        {
            if (!TryGetGlyphIndex(c, out var index))
            {
                if (_defaultGlyphIndex == -1)
                {
                    throw new ArgumentException("Text contains characters that cannot be resolved by this SpriteFont.", "text");
                }

                return _defaultGlyphIndex;
            }

            return index;
        }
    }
}
