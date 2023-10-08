using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;

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

        public RenderText()
        {

        }
    }
}
