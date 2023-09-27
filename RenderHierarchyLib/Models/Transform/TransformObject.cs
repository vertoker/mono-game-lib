using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformObject
    {
        public Vector2 Pos = Vector2.Zero;
        public float Rot = 0;
        public Vector2 Sca = Vector2.One;

        public TransformObject()
        {

        }
        public TransformObject(Vector2 pos)
        {
            Pos = pos;
        }
        public TransformObject(Vector2 pos, float rot)
        {
            Pos = pos;
            Rot = rot;
        }
        public TransformObject(Vector2 pos, float rot, Vector2 sca)
        {
            Pos = pos;
            Rot = rot;
            Sca = sca;
        }
    }
}
