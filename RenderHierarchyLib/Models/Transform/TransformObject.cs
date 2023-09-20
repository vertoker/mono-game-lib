using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Transform.Interfaces;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformObject : ITransformObject
    {
        public Vector2 Pos { get; private set; }
        public float Rot { get; private set; }
        public Vector2 Sca { get; private set; }

        public TransformObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;
        }
    }
}
