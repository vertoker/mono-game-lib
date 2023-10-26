using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;

namespace UILib.Interfaces.Container
{
    public interface IElementRect
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Size { get; set; }

        public Vector2 Pivot { get; set; }
        public Vector2 Anchor { get; set; }

        public void UpdateRect();
    }
}
