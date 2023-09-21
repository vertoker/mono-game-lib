using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;
using RenderHierarchyLib.Models.Transform;
using System;

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

        public float PixelScale => Graphics.PreferredBackBufferHeight / Transform.Sca;

        public Vector2 GetScreenAnchorPoint(Anchor anchor)
        {
            return anchor switch
            {
                Anchor.Left_Top => new Vector2(0, 0),
                Anchor.Center_Top => new Vector2(Graphics.PreferredBackBufferWidth / 2f, 0),
                Anchor.Right_Top => new Vector2(Graphics.PreferredBackBufferWidth, 0),

                Anchor.Left_Middle => new Vector2(0, Graphics.PreferredBackBufferHeight / 2f),
                Anchor.Center_Middle => new Vector2(Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight / 2f),
                Anchor.Right_Middle => new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight / 2f),

                Anchor.Left_Bottom => new Vector2(0, Graphics.PreferredBackBufferHeight),
                Anchor.Center_Bottom => new Vector2(Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight),
                Anchor.Right_Bottom => new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight),

                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
