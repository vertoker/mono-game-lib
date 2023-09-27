using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models.Transform
{
    public struct TransformCamera
    {
        public Vector2 Pos = Vector2.Zero;
        public float Rot = 0;
        public float PixelSca = 10;

        public TransformCamera()
        {

        }
        public TransformCamera(float pixelSca)
        {
            PixelSca = pixelSca;
        }
        public TransformCamera(Vector2 pos)
        {
            Pos = pos;
        }
        public TransformCamera(Vector2 pos, float rot)
        {
            Pos = pos;
            Rot = rot;
        }
        public TransformCamera(Vector2 pos, float rot, float pixelSca)
        {
            Pos = pos;
            Rot = rot;
            PixelSca = pixelSca;
        }
    }
}
