using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;
using RenderHierarchyLib.Models.Transform.Interfaces;

namespace RenderHierarchyLib.Models.Transform
{
    public struct RenderTransformObject : IHierarchyTransformObject
    {
        public Vector2 Pos { get; set; }
        public float Rot { get; set; }
        public Vector2 Sca { get; set; }

        public Anchor Anchor { get; set; }
        public Anchor Pivot { get; set; }
        public int Depth { get; set; }

        public RenderTransformObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;

            Anchor = Anchor.Center_Middle;
            Pivot = Anchor.Center_Middle;
            Depth = 0;
        }

        public static readonly RenderTransformObject Empty = new RenderTransformObject();
        public static readonly RenderTransformObject LeftTop = new RenderTransformObject() { Anchor = Anchor.Left_Top };
        public static readonly RenderTransformObject CenterTop = new RenderTransformObject() { Anchor = Anchor.Center_Top };
        public static readonly RenderTransformObject RightTop = new RenderTransformObject() { Anchor = Anchor.Right_Top };
        public static readonly RenderTransformObject LeftMiddle = new RenderTransformObject() { Anchor = Anchor.Left_Middle };
        public static readonly RenderTransformObject CenterMiddle = new RenderTransformObject() { Anchor = Anchor.Center_Middle };
        public static readonly RenderTransformObject RightMiddle = new RenderTransformObject() { Anchor = Anchor.Right_Middle };
        public static readonly RenderTransformObject LeftBottom = new RenderTransformObject() { Anchor = Anchor.Left_Bottom };
        public static readonly RenderTransformObject CenterBottom = new RenderTransformObject() { Anchor = Anchor.Center_Bottom };
        public static readonly RenderTransformObject RightBottom = new RenderTransformObject() { Anchor = Anchor.Right_Bottom };
    }
}
