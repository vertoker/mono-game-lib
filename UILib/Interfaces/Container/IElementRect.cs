using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;

namespace UILib.Interfaces.Container
{
    public interface IElementRect
    {
        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }
        public Vector2 LocalSize { get; set; }

        public Vector2 LocalPivot { get; set; }
        public Vector2 LocalAnchor { get; set; }

        public Vector2 Position { get; }
        public float Rotation { get; }
        public Vector2 Size { get; }

        public Vector2 Pivot { get; }
        public Vector2 Anchor { get; }

        public void UpdateRect();
    }
}
