using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Transform.Interfaces;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformObject : ITransformObject
    {
        public Vector2 Pos { get; set; }
        public float Rot { get; set; }
        public Vector2 Sca { get; set; }

        public TransformObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;
        }
    }
}
