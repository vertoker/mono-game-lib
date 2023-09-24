using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformObject
    {
        public Vector2 Pos;
        public float Rot;
        public Vector2 Sca;

        public TransformObject()
        {
            Pos = Vector2.Zero;
            Rot = 0;
            Sca = Vector2.One;
        }
    }
}
