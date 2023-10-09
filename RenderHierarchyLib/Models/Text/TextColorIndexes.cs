using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RenderHierarchyLib.Models.Text
{
    public class TextColorIndexes
    {
        public Color Color = Color.White;
        public List<int> Indexes = new();

        public TextColorIndexes(Color color)
        {
            Color = color;
        }
        public TextColorIndexes(List<int> indexes)
        {
            Indexes = indexes;
        }
        public TextColorIndexes(Color color, List<int> indexes)
        {
            Color = color;
            Indexes = indexes;
        }
    }
}
