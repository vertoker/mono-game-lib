using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderObject
    {
        public Vector2 Pos = Vector2.Zero;
        public float Rot = 0;
        public Vector2 Sca = Vector2.One;

        public Vector2 Pivot = AnchorPresets.CenterMiddle;
        public Vector2 Anchor = AnchorPresets.CenterMiddle;
        public int Depth = 0;

        public RenderObject()
        {

        }
        public RenderObject(Vector2 pos)
        {
            Pos = pos;
        }
        public RenderObject(Vector2 pos, float rot)
        {
            Pos = pos;
            Rot = rot;
        }
        public RenderObject(Vector2 pos, float rot, Vector2 sca)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;
        }
        public RenderObject(Vector2 pos, float rot, Vector2 sca, Vector2 pivot)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
        }
        public RenderObject(Vector2 pos, float rot, Vector2 sca, Vector2 pivot, Vector2 anchor)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
            Anchor = anchor;
        }
        public RenderObject(Vector2 pos, float rot, Vector2 sca, Vector2 pivot, Vector2 anchor, int depth)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
            Anchor = anchor;
            Depth = depth;
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
