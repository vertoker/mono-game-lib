using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Text
{
    public class RichTextParser : IEnumerator<Color>
    {
        private const char OpenCurlyBrackets = '{';
        private const char CloseCurlyBrackets = '}';

        private const int DefaultTextCapacity = 25;
        private const int DefaultEditCapacity = 9;
        private const int DefaultColorsCapacity = 3;

        public string Text { get; set; }
        public Dictionary<int, Color> Colors = new(DefaultColorsCapacity);
        public Color DefaultColor { get; private set; } = Color.White;

        private readonly StringBuilder _textBuilder = new(DefaultTextCapacity);
        private readonly StringBuilder _editBuilder = new(DefaultEditCapacity);

        public RichTextParser(Color defaultColor)
        {
            DefaultColor = defaultColor;
        }
        public RichTextParser(string text)
        {
            SetText(text);
        }
        public RichTextParser(string text, Color defaultColor)
        {
            SetText(text, defaultColor);
        }

        public void SetText(string text, Color defaultColor)
        {
            DefaultColor = defaultColor;
            SetText(text);
        }
        public void SetText(string text)
        {
            _textBuilder.Clear();
            _textBuilder.EnsureCapacity(text.Length);
            _editBuilder.Clear();
            Colors.Clear();

            char cachedChar = default;
            var editMode = false;

            foreach (var c in text)
            {
                if (c == OpenCurlyBrackets)
                {
                    if (cachedChar == OpenCurlyBrackets)
                    {
                        cachedChar = default;
                        editMode = true;
                    }
                    else
                    {
                        cachedChar = OpenCurlyBrackets;
                    }
                }
                else if (c == CloseCurlyBrackets)
                {
                    if (cachedChar == CloseCurlyBrackets)
                    {
                        cachedChar = default;
                        editMode = false;
                        ParseHex();
                    }
                    else
                    {
                        cachedChar = CloseCurlyBrackets;
                    }
                }
                else if (editMode)
                {
                    _editBuilder.Append(c);
                }
                else
                {
                    _textBuilder.Append(c);
                }
            }

            Text = _textBuilder.ToString();
        }

        private void ParseHex()
        {
            var colorcode = _editBuilder.ToString();
            Color color = _editBuilder.Length == 9 
                ? new Color(
                    int.Parse(colorcode.Substring(1, 2), NumberStyles.HexNumber),
                    int.Parse(colorcode.Substring(3, 2), NumberStyles.HexNumber),
                    int.Parse(colorcode.Substring(5, 2), NumberStyles.HexNumber),
                    int.Parse(colorcode.Substring(7, 2), NumberStyles.HexNumber))
                : _editBuilder.Length == 7 ? new Color(
                    int.Parse(colorcode.Substring(1, 2), NumberStyles.HexNumber),
                    int.Parse(colorcode.Substring(3, 2), NumberStyles.HexNumber),
                    int.Parse(colorcode.Substring(5, 2), NumberStyles.HexNumber))
                : DefaultColor;
            Colors.Add(_textBuilder.Length, color);
            _editBuilder.Clear();
        }

        public Color Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
