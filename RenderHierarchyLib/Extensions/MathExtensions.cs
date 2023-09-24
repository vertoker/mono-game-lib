using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;
using System;

namespace RenderHierarchyLib.Extensions
{
    public static class MathExtensions
    {
        public const float Deg2Rad = MathF.PI * 2F / 360F;
        public const float Rad2Deg = 1F / Deg2Rad;

        public static Vector2 RotateVector(this Vector2 vector, float angle)
        {
            var rad = angle * Deg2Rad;
            var sin = MathF.Sin(rad);
            var cos = MathF.Cos(rad);
            return vector.RotateVector(sin, cos);
        }
        public static Vector2 RotateVector(this Vector2 vector, Vector2 parent, float angle)
        {
            var rad = angle * Deg2Rad;
            var sin = MathF.Sin(rad);
            var cos = MathF.Cos(rad);
            return vector.RotateVector(parent, sin, cos);
        }

        public static Vector2 RotateVector(this Vector2 vector, float sin, float cos)
        {
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
        public static Vector2 RotateVector(this Vector2 vector, Vector2 parent, float sin, float cos)
        {
            return new Vector2(parent.X + vector.X * cos - vector.Y * sin, parent.Y + vector.X * sin + vector.Y * cos);
        }

        public static Vector2 RotateVector(float x, float y, float sin, float cos)
        {
            return new Vector2(x * cos - y * sin, x * sin + y * cos);
        }
        public static Vector2 RotateVector(float x, float y, Vector2 parent, float sin, float cos)
        {
            return new Vector2(parent.X + x * cos - y * sin, parent.Y + x * sin + y * cos);
        }

        public static Vector2 GetCenterRectangle(this Anchor anchor, Vector2 rectangleSize)
        {
            return anchor switch
            {
                Anchor.Left_Top      => new(                    0,                     0),
                Anchor.Center_Top    => new( rectangleSize.X / 2f,                     0),
                Anchor.Right_Top     => new(      rectangleSize.X,                     0),
                
                Anchor.Left_Middle   => new(                    0,  rectangleSize.Y / 2f),
                Anchor.Center_Middle => new( rectangleSize.X / 2f,  rectangleSize.Y / 2f),
                Anchor.Right_Middle  => new(      rectangleSize.X,  rectangleSize.Y / 2f),

                Anchor.Left_Bottom   => new(                    0,       rectangleSize.Y),
                Anchor.Center_Bottom => new( rectangleSize.X / 2f,       rectangleSize.Y),
                Anchor.Right_Bottom  => new(      rectangleSize.X,       rectangleSize.Y),

                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static void GetBordersRectangleByPivot(this Vector2 pixelSize, Anchor pivot, out Vector2 TL, out Vector2 BR)
        {
            switch (pivot)
            {
                case Anchor.Left_Top:
                    TL = new(                0,                 0);
                    BR = new(      pixelSize.X,       pixelSize.Y);
                    return;
                case Anchor.Center_Top:
                    TL = new( -pixelSize.X / 2,                 0);
                    BR = new(  pixelSize.X / 2,       pixelSize.Y);
                    return;
                case Anchor.Right_Top:
                    TL = new(     -pixelSize.X,                 0);
                    BR = new(                0,       pixelSize.Y);
                    return;

                case Anchor.Left_Middle:
                    TL = new(                0,  -pixelSize.Y / 2);
                    BR = new(      pixelSize.X,   pixelSize.Y / 2);
                    return;
                case Anchor.Center_Middle:
                    TL = new( -pixelSize.X / 2,  -pixelSize.Y / 2);
                    BR = new(  pixelSize.X / 2,   pixelSize.Y / 2);
                    return;
                case Anchor.Right_Middle:
                    TL = new(     -pixelSize.X,  -pixelSize.Y / 2);
                    BR = new(                0,   pixelSize.Y / 2);
                    return;

                case Anchor.Left_Bottom:
                    TL = new(                0,      -pixelSize.Y);
                    BR = new(      pixelSize.X,                 0);
                    return;
                case Anchor.Center_Bottom:
                    TL = new( -pixelSize.X / 2,      -pixelSize.Y);
                    BR = new(  pixelSize.X / 2,                 0);
                    return;
                case Anchor.Right_Bottom:
                    TL = new(     -pixelSize.X,      -pixelSize.Y);
                    BR = new(                0,                 0);
                    return;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
