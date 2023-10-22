using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Text.Colors
{
    public class SimpleColorEnumerator : IEnumerator<Color>
    {
        public Color Color { get; set; }

        public SimpleColorEnumerator(Color color)
        {
            Color = color;
        }

        public Color Current => Color;
        object IEnumerator.Current => Color;

        public void Dispose()
        {

        }
        public bool MoveNext()
        {
            return false;
        }
        public void Reset()
        {

        }
    }
}
