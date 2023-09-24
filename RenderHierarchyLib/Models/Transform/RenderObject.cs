using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderObject
    {
        public Vector2 Pos;
        public float Rot;
        public Vector2 Sca;

        public Anchor Anchor;
        public Anchor Pivot;
        public int Depth;

        public RenderObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;

            Anchor = Anchor.Center_Middle;
            Pivot = Anchor.Center_Middle;
            Depth = 0;
        }

        public static readonly RenderObject Empty = new();
        public static readonly RenderObject LeftTop = new() { Anchor = Anchor.Left_Top };
        public static readonly RenderObject CenterTop = new() { Anchor = Anchor.Center_Top };
        public static readonly RenderObject RightTop = new() { Anchor = Anchor.Right_Top };
        public static readonly RenderObject LeftMiddle = new() { Anchor = Anchor.Left_Middle };
        public static readonly RenderObject CenterMiddle = new() { Anchor = Anchor.Center_Middle };
        public static readonly RenderObject RightMiddle = new() { Anchor = Anchor.Right_Middle };
        public static readonly RenderObject LeftBottom = new() { Anchor = Anchor.Left_Bottom };
        public static readonly RenderObject CenterBottom = new() { Anchor = Anchor.Center_Bottom };
        public static readonly RenderObject RightBottom = new() { Anchor = Anchor.Right_Bottom };
    }
}
