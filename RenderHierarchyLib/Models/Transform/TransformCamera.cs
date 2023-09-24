using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformCamera
    {
        public Vector2 Pos;
        public float Rot;
        public float Sca;

        public TransformCamera() : this(2) { }
        public TransformCamera(float pixelScale)
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = pixelScale;
        }
    }
}
