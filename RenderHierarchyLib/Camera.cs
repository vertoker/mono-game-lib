using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Transform;

namespace RenderHierarchyLib
{
    public class Camera
    {
        public readonly TransformCamera Transform;
        public readonly GraphicsDeviceManager Graphics;

        public Camera(TransformCamera transform, GraphicsDeviceManager graphics)
        {
            Transform = transform;
            Graphics = graphics;
        }

        public int ScreenX => Graphics.PreferredBackBufferWidth;
        public int ScreenY => Graphics.PreferredBackBufferHeight;
        public float PixelScale => Graphics.PreferredBackBufferHeight / Transform.Sca;
    }
}
