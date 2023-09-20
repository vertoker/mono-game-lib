using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Transform.Interfaces;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformCamera : ITransformCamera
    {
        public Vector2 Pos { get; private set; }
        public float Rot { get; private set; }
        public float Sca { get; private set; }

        public TransformCamera() : this(2) { }
        public TransformCamera(float pixelScale)
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = pixelScale;
        }
    }
}
