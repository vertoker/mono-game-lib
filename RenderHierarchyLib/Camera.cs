using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Models.Transform;
using System;

namespace RenderHierarchyLib
{
    public class Camera
    {
        public TransformCamera Transform;
        public readonly GraphicsDeviceManager GraphicsManager;

        public Vector2 ScreenSize { get; private set; }
        public Vector2 HalfSize { get; private set; }
        public Vector2 AnchorCenter { get; private set; }
        public Vector2 AnchorCenterInverse { get; private set; }
        public Vector2 AnchorX { get; private set; }
        public Vector2 AnchorY { get; private set; }

        public Camera(float pixelSca, GraphicsDeviceManager manager)
        {
            Transform = new TransformCamera(pixelSca);
            GraphicsManager = manager;
        }
        public Camera(TransformCamera transform, GraphicsDeviceManager manager)
        {
            Transform = transform;
            GraphicsManager = manager;
        }

        public void UpdateAnchors() => UpdateAnchors(PixelScale);
        public void UpdateAnchors(float pixelScale)
        {
            ScreenSize = new Vector2(ScreenX, ScreenY);
            HalfSize = ScreenSize / 2f;

            var sin = MathF.Sin(Transform.Rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(Transform.Rot * MathExtensions.Deg2Rad);
            var pos = Transform.Pos.RotateVector(-Transform.Rot);

            AnchorCenter = new Vector2(pos.X * pixelScale, pos.Y * pixelScale) + HalfSize;
            AnchorCenterInverse = new Vector2(pos.X * -pixelScale, pos.Y * pixelScale) + HalfSize;
            AnchorX = MathExtensions.RotateVector(HalfSize.X, 0, sin, cos);
            AnchorY = MathExtensions.RotateVector(0, HalfSize.Y, sin, cos);
        }

        public Vector2 GetAnchorPosCamera(Vector2 anchor)
        {
            return new Vector2()
            {
                X = HalfSize.X + HalfSize.X * anchor.X,
                Y = HalfSize.Y + HalfSize.Y * anchor.Y
            };
        }
        public Vector2 GetAnchorPosCameraInverse(Vector2 anchor)
        {
            return new Vector2()
            {
                X = HalfSize.X + HalfSize.X * anchor.X,
                Y = HalfSize.Y - HalfSize.Y * anchor.Y
            };
        }
        public Vector2 GetAnchorPosWorld(Vector2 anchor)
        {
            return new Vector2()
            {
                X = AnchorCenter.X + AnchorX.X * anchor.X + AnchorY.X * anchor.Y,
                Y = AnchorCenter.Y + AnchorX.Y * anchor.X + AnchorY.Y * anchor.Y
            };
        }
        public Vector2 GetAnchorPosWorldInverse(Vector2 anchor)
        {
            return new Vector2()
            {
                X = AnchorCenterInverse.X + AnchorX.X * anchor.X + AnchorY.X * anchor.Y,
                Y = AnchorCenterInverse.Y + AnchorX.Y * anchor.X + AnchorY.Y * anchor.Y
            };
        }

        public int ScreenX => GraphicsManager.PreferredBackBufferWidth;
        public int ScreenY => GraphicsManager.PreferredBackBufferHeight;
        public float PixelScale => GraphicsManager.PreferredBackBufferHeight / Transform.PixelSca;
    }
}
