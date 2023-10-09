using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Models.Text;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderText
    {
        public Vector2 Pos = Vector2.Zero;
        public float Rot = 0;
        public Vector2 Sca = Vector2.One;

        public Vector2 Pivot = AnchorPresets.CenterMiddle;
        public Vector2 Anchor = AnchorPresets.CenterMiddle;
        public int Depth = 0;

        public TextAlignmentHorizontal Alignment = TextAlignmentHorizontal.Left;

        public RenderText()
        {

        }
        public RenderText(Vector2 pos)
        {
            Pos = pos;
        }
        public RenderText(Vector2 pos, float rot)
        {
            Pos = pos;
            Rot = rot;
        }
        public RenderText(Vector2 pos, float rot, Vector2 sca)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;
        }
        public RenderText(Vector2 pos, float rot, Vector2 sca, Vector2 pivot)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
        }
        public RenderText(Vector2 pos, float rot, Vector2 sca, Vector2 pivot, Vector2 anchor)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
            Anchor = anchor;
        }
        public RenderText(Vector2 pos, float rot, Vector2 sca, Vector2 pivot, Vector2 anchor, int depth)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
            Anchor = anchor;
            Depth = depth;
        }
        public RenderText(Vector2 pos, float rot, Vector2 sca, Vector2 pivot, Vector2 anchor, int depth, TextAlignmentHorizontal alignment)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;

            Pivot = pivot;
            Anchor = anchor;
            Depth = depth;

            Alignment = alignment;
        }

        public static readonly RenderText Empty = new();

        public static readonly RenderText LeftAlignment = new() { Alignment = TextAlignmentHorizontal.Left };
        public static readonly RenderText CenterAlignment = new() { Alignment = TextAlignmentHorizontal.Center };
        public static readonly RenderText RightAlignment = new() { Alignment = TextAlignmentHorizontal.Right };

        public static readonly RenderText LeftTop = new() { Anchor = AnchorPresets.LeftTop };
        public static readonly RenderText CenterTop = new() { Anchor = AnchorPresets.CenterTop };
        public static readonly RenderText RightTop = new() { Anchor = AnchorPresets.RightTop };
        public static readonly RenderText LeftMiddle = new() { Anchor = AnchorPresets.LeftMiddle };
        public static readonly RenderText CenterMiddle = new() { Anchor = AnchorPresets.CenterMiddle };
        public static readonly RenderText RightMiddle = new() { Anchor = AnchorPresets.RightMiddle };
        public static readonly RenderText LeftBottom = new() { Anchor = AnchorPresets.LeftBottom };
        public static readonly RenderText CenterBottom = new() { Anchor = AnchorPresets.CenterBottom };
        public static readonly RenderText RightBottom = new() { Anchor = AnchorPresets.RightBottom };
    }
}
