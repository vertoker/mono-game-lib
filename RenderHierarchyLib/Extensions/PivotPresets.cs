using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Extensions
{
    public static class PivotPresets
    {
        public static readonly Vector2 LeftTop = new(-1, 1);
        public static readonly Vector2 CenterTop = new(0, 1);
        public static readonly Vector2 RightTop = new(1, 1);
        public static readonly Vector2 LeftMiddle = new(-1, 0);
        public static readonly Vector2 CenterMiddle = new(0, 0);
        public static readonly Vector2 RightMiddle = new(1, 0);
        public static readonly Vector2 LeftBottom = new(-1, -1);
        public static readonly Vector2 CenterBottom = new(0, -1);
        public static readonly Vector2 RightBottom = new(1, -1);
    }
}