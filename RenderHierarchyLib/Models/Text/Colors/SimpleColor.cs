using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RenderHierarchyLib.Models.Text.Colors
{
    public class SimpleColor : IEnumerable<Color>
    {
        public Color Color { get; set; }

        public SimpleColor(Color color)
        {
            Color = color;
        }

        public IEnumerator<Color> GetEnumerator()
        {
            return new SimpleColorEnumerator(Color);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SimpleColorEnumerator(Color);
        }
    }
}
