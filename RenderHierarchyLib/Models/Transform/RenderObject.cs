using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderObject
    {
        public Vector2 Pos;
        public float Rot;
        public Vector2 Sca;

        public Vector2 Anchor;
        public Vector2 Pivot;
        public int Depth;

        public RenderObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;

            Anchor = AnchorPresets.CenterMiddle;
            Pivot = AnchorPresets.CenterMiddle;
            Depth = 0;
        }

        public static readonly RenderObject Empty = new();
        public static readonly RenderObject LeftTop = new() { Anchor = AnchorPresets.LeftTop };
        public static readonly RenderObject CenterTop = new() { Anchor = AnchorPresets.CenterTop };
        public static readonly RenderObject RightTop = new() { Anchor = AnchorPresets.RightTop };
        public static readonly RenderObject LeftMiddle = new() { Anchor = AnchorPresets.LeftMiddle };
        public static readonly RenderObject CenterMiddle = new() { Anchor = AnchorPresets.CenterMiddle };
        public static readonly RenderObject RightMiddle = new() { Anchor = AnchorPresets.RightMiddle };
        public static readonly RenderObject LeftBottom = new() { Anchor = AnchorPresets.LeftBottom };
        public static readonly RenderObject CenterBottom = new() { Anchor = AnchorPresets.CenterBottom };
        public static readonly RenderObject RightBottom = new() { Anchor = AnchorPresets.RightBottom };
    }
}
