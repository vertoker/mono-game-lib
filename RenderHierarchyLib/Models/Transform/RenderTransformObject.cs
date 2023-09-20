using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;
using RenderHierarchyLib.Models.Transform.Interfaces;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderTransformObject : IHierarchyTransformObject
    {
        public Vector2 Pos { get; private set; }
        public float Rot { get; private set; }
        public Vector2 Sca { get; private set; }

        public Anchor Anchor { get; private set; }
        public Anchor Pivot { get; private set; }
        public Color Color { get; private set; }

        public RenderTransformObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;

            Anchor = Anchor.Center_Middle;
            Pivot = Anchor.Center_Middle;
            Color = Color.White;
        }
    }
}
